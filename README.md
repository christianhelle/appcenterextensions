[![Build status](https://dev.azure.com/christianhelle/AppCenter%20Extensions/_apis/build/status/CI%20Build)](https://dev.azure.com/christianhelle/AppCenter%20Extensions/_build/latest?definitionId=-1)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=christianhelle_appcenterextensions&metric=alert_status)](https://sonarcloud.io/dashboard?id=christianhelle_appcenterextensions)
[![NuGet](https://img.shields.io/nuget/v/appcenterextensions.svg?style=flat-square)]( https://www.nuget.org/packages/appcenterextensions)

# AppCenterExtensions
A set of convenience classes and extension methods to simplify Crash Reporting and Analytics using AppCenter

## Features
- User interaction reporting using `ICommand` implementations
- Automatic page tracking in **Xamarin.Forms** including time spent on screen
- Extension methods for crash reporting
- Anonymous user information configuration

## Getting Started

This library is configured almost the same way as the AppCenter SDK. You provide the AppCenter secrets, and specify whether to anonymize the user information

```
TrackingApplication.Initialize(
    "[iOS AppCenter secret]",
    "[Android AppCenter secret]",
    anonymizeAppCenterUser: true);
```

## ITrackingCommand

This library provides 3 convenience implementations of `ICommand` that will report the action to AppCenter Analytics after successfully invoking the execute callback method

- ***TrackingCommand*** - This implementation accepts an `Action` as the Execute callback and a `Func<bool>` as the CanExecute callback
- ***TrackingCommand<T>*** - This implementation accepts an `Action<T>` as the Execute callback and a `Func<T, bool>` as the CanExecute callback
- ***AsyncTrackingCommand*** - This implementation accepts a `Func<Task>` as the execute callback and a `Func<bool>` as the CanExecute callback. This also exposes a `CompletionTask` property that the consumer can `await` if desired. The `Execute(object parameter)` method here is a non-blocking call

Example:

```
using System.Threading.Tasks;
using System.Windows.Input;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Commands;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;

namespace SampleApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            AsyncButtonTappedCommand = new AsyncTrackingCommand(
                OnAsyncButtonTapped,
                nameof(AsyncButtonTappedCommand).ToTrackingEventName(),
                nameof(AboutViewModel).ToTrackingEventName());

            ButtonOneTappedCommand = new TrackingCommand(
                OnButtonOneTapped,
                nameof(ButtonOneTappedCommand).ToTrackingEventName(),
                nameof(AboutViewModel).ToTrackingEventName());

            ButtonTwoTappedCommand = new TrackingCommand<string>(
                OnButtonTapped,
                nameof(ButtonTwoTappedCommand).ToTrackingEventName(),
                nameof(AboutViewModel).ToTrackingEventName());
        }

        public ICommand AsyncButtonTappedCommand { get; }
        public ICommand ButtonOneTappedCommand { get; }
        public ICommand ButtonTwoTappedCommand { get; }

        private Task OnAsyncButtonTapped()
            => Browser.OpenAsync("https://xamarin.com");

        private void OnButtonOneTapped() { }

        private void OnButtonTwoTapped(string obj) { }
    }
}
```

## Automatic Page Tracking

Automatic page tracking is enabled by replacing the base class of the `ContentPage` to classes to use `TrackingContentPage` class. By doing so the library will send page tracking information to AppCenter after leaving every page. Currently, the library will send the page Type, Title, and the duration spent on the screen. The library is rather opinionated on how to log information, and this will only change if I get a request to do so. Duration spent on screen is calculated using a `Stopwatch` that is started upon Page `OnAppearing` and is reported to Analytics upon `OnDisappearing`. The event name is based on the `Type` name of the `Page` and is split into multiple words based on pascal case rules and afterwards removes words like `Page`, `View`, `Model`, `Async`. For example: `UserSettingsPage` or `UserSettingsView` becomes **User Settings**

XAML Example:

```
<?xml version="1.0" encoding="utf-8"?>

<ext:TrackingContentPage 
    xmlns="http://xamarin.com/schemas/2014/forms" 
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
    xmlns:d="http://xamarin.com/schemas/2014/forms/design" 
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:ext="clr-namespace:ChristianHelle.DeveloperTools.AppCenterExtensions.XamarinForms;assembly=AppCenterExtensions.XamarinForms"
    mc:Ignorable="d" 
    x:Class="SampleApp.Views.ItemDetailPage" 
    Title="{Binding Title}">

    <StackLayout Spacing="20" Padding="15">
        <Label Text="Text:" FontSize="Medium" />
        <Label Text="{Binding Item.Text}" d:Text="Item name" FontSize="Small" />
        <Label Text="Description:" FontSize="Medium" />
        <Label Text="{Binding Item.Description}" d:Text="Item description" FontSize="Small" />
    </StackLayout>

</ext:TrackingContentPage>
```