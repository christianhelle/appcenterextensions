using ChristianHelle.DeveloperTools.AppCenterExtensions.XamarinForms;
using Xamarin.Forms;
using SampleApp.Services;
using SampleApp.Views;
using ChristianHelle.DeveloperTools.AppCenterExtensions;

namespace SampleApp
{
    public partial class App : Application
    {
        public App()
        {
            AppCenterSetup.Instance.Start(
                "9f8ed7ec-e9d8-4de2-8909-e9a01493c006",
                "cd665281-8d72-4e4e-bce9-bca552776eb5",
                true);
            
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
