using System;
using Xamarin.Forms;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Infrastructure
{
    public static class MockXamarinForms
    {
        private static volatile bool initialized;

        public static void Init()
        {
            if (initialized)
                return;
            initialized = true;

            Device.Info = new MockDeviceInfo();
            Device.PlatformServices = new MockPlatformServices { RuntimePlatform = Guid.NewGuid().ToString() };
            DependencyService.Register<MockResourcesProvider>();
            DependencyService.Register<MockDeserializer>();
        }
    }
}
