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
            Title = "About";

            LearnMoreTappedCommand = new AsyncTrackingCommand(
                () => Browser.OpenAsync("https://xamarin.com"),
                nameof(LearnMoreTappedCommand).ToTrackingEventName(),
                nameof(AboutViewModel).ToTrackingEventName());

            CrashTestTappedCommand = new TrackingCommand(
                () => Crashes.GenerateTestCrash(),
                nameof(CrashTestTappedCommand).ToTrackingEventName(),
                nameof(AboutViewModel).ToTrackingEventName());
        }

        public ICommand LearnMoreTappedCommand { get; }

        public ICommand CrashTestTappedCommand { get; }
    }
}