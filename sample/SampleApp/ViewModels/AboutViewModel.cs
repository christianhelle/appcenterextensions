using System;
using System.Windows.Input;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SampleApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
            CrashCommand = new Command(() => Crashes.GenerateTestCrash());
        }

        public ICommand OpenWebCommand { get; }

        public ICommand CrashCommand { get; }
    }
}