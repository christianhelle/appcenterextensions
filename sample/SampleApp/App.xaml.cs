using Xamarin.Forms;
using SampleApp.Services;
using SampleApp.Views;
using ChristianHelle.DeveloperTools.AppCenterExtensions;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Diagnostics;
using System.Diagnostics;
using System.Collections.Generic;

namespace SampleApp
{
    public partial class App : Application
    {
        private static readonly Stopwatch stopwatch = Stopwatch.StartNew();

        public App()
        {
            AppCenterSetup.Instance.Start(
                "9f8ed7ec-e9d8-4de2-8909-e9a01493c006",
                "cd665281-8d72-4e4e-bce9-bca552776eb5",
                true);

            Trace.Listeners.Add(new AppCenterTraceListener());
            
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
            => Trace.Write(
                new AnalyticsEvent(
                    nameof(Application),
                    new Dictionary<string, string>
                    {
                        { "State", nameof(OnStart) }
                    }));

        protected override void OnSleep()
            => Trace.Write(
                new AnalyticsEvent(
                    nameof(Application),
                    new Dictionary<string, string>
                    {
                        { "State", nameof(OnSleep) }
                    }));

        protected override void OnResume()
            => Trace.Write(
                new AnalyticsEvent(
                    nameof(Application),
                    new Dictionary<string, string>
                    {
                        { "State", nameof(OnResume) }
                    }));
    }
}
