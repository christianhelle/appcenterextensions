[![Build status](https://dev.azure.com/christianhelle/AppCenter%20Extensions/_apis/build/status/CI%20Build)](https://dev.azure.com/christianhelle/AppCenter%20Extensions/_build/latest?definitionId=-1)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=christianhelle_appcenterextensions&metric=alert_status)](https://sonarcloud.io/dashboard?id=christianhelle_appcenterextensions)
[![NuGet](https://img.shields.io/nuget/v/appcenterextensions.svg?style=flat-square)]( https://www.nuget.org/packages/appcenterextensions)

# AppCenterExtensions
A set of convenience classes and extension methods to simplify Crash Reporting and Analytics using AppCenter

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
