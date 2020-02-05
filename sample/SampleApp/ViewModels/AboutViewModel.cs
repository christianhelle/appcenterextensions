using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using AppCenterExtensions.Commands;
using AppCenterExtensions.Extensions;
using AppCenterExtensions.Http;
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
                nameof(LearnMoreTappedCommand).ToTrackingEventName());

            CrashTestTappedCommand = new TrackingCommand(
                () => Crashes.GenerateTestCrash(),
                nameof(CrashTestTappedCommand).ToTrackingEventName());

            ButtonTappedCommand = new TrackingCommand<string>(
                p => Debug.WriteLine(p),
                nameof(ButtonTappedCommand).ToTrackingEventName());

            OpenBadUrlTappedCommand = new AsyncTrackingCommand(
                OpenNotFoundUrl,
                nameof(OpenBadUrlTappedCommand).ToTrackingEventName());
        }

        public ICommand LearnMoreTappedCommand { get; }

        public ICommand CrashTestTappedCommand { get; }
        
        public ICommand ButtonTappedCommand { get; }

        public ICommand OpenBadUrlTappedCommand { get; }

        private async Task OpenNotFoundUrl()
        {
            try
            {
                var httpClient = new HttpClient(new DiagnosticDelegatingHandler());
                var response = await httpClient.GetAsync("https://entbpr4b9bdpo.x.pipedream.net/");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                ex.Report();
            }
        }
    }
}