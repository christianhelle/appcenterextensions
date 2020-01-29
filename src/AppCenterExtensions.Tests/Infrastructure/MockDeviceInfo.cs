using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Infrastructure
{
    internal class MockDeviceInfo : DeviceInfo
    {
        public override Size PixelScreenSize => throw new NotImplementedException();

        public override Size ScaledScreenSize => throw new NotImplementedException();

        public override double ScalingFactor => throw new NotImplementedException();
    }
}