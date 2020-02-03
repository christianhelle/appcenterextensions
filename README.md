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

## NuGet Packages

This library is distributed as 2 nuget packages
- [AppCenterExtensions](https://www.nuget.org/packages/appcenterextensions) - This contains extension methods, `ICommand` implementations, and convenience classes for initializing and configuring AppCenter. This package depends on [Microsoft.AppCenter.Analytics](https://www.nuget.org/packages/Microsoft.AppCenter.Analytics/) and [Microsoft.AppCenter.Crashes](https://www.nuget.org/packages/Microsoft.AppCenter.Crashes/) version 2.6.4
- [AppCenterExtensions.XamarinForms](https://www.nuget.org/packages/appcenterextensions.xamarinforms) - This contains components required for automatic page tracking using [Xamarin.Forms](https://github.com/xamarin/Xamarin.Forms). This package depends on [AppCenterExtensions](https://www.nuget.org/packages/appcenterextensions) and [Xamarin.Forms](https://www.nuget.org/packages/Xamarin.Forms/) version 4.0.0

## Getting Started

This library is configured almost the same way as the AppCenter SDK. You provide the AppCenter secrets, and specify whether to anonymize the user information. Both Crash Reporting and Analytics are **always** enabled when using `AppCenterSetup`.

```
AppCenterSetup
    .Start(
        "[iOS AppCenter secret]",
        "[Android AppCenter secret]",
        anonymizeAppCenterUser: true);
```

or

```
await AppCenterSetup
    .StartAsync(
        "[iOS AppCenter secret]",
        "[Android AppCenter secret]",
        anonymizeAppCenterUser: true);
```

The reason for the `async` API here is because `anonymizeAppCenterUser` internally relies on an `async` API. The synchronize API's for starting AppCenter are non-blocking methods that do a fire-and-forget call to `StartAsync(string,bool)`.

## Anonymous User Information

The component `AppCenterSetup` exposes a method called `UseAnonymousUserIdAsync()` which sets the UserId in AppCenter to the first 8 characters a GUID that is unique per app installation. This can be used as a **support key** for uniquely identifying application users for instrumentation and troubleshooting

## Error Reporting

The library exposes extension methods to the `Exception` class for conveniently reporting Exceptions to AppCenter

Example:

```
try
{
    // Something that blows up
    explosives.Detonate();
}
catch (Exception e)
{
    // Safely handle error then report
    e.Report();
}
```

## HTTP Error Logging

The library provides a `HttpMessageHandler` implementation that logs non-successfuly HTTP results to AppCenter Analytics. This component will also attach HTTP headers describing the AppCenter SDK Version, Install ID, and a support key to all HTTP requests. The logged failed responses will contain the Endpoint URL (including the HTTP verb), Response status code, how the duration of the HTTP call. This will be logged under the event name **HTTP Error**

You will in most (if not all) cases would want to keep a singleton instance of the `HttpClient`. The `DiagnosticDelegatingHandler` is designed with unit testing in mind and accepts an `IAnalytics` and `IAppCenterSetup` interface, it also accepts an inner `HttpMessageHandler` if you wish to chain multiple delegating handlers.

Example:

```
var httpClient = new HttpClient(new DiagnosticDelegatingHandler());
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