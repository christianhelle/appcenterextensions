using System;
using AppCenterExtensions.Tests.Infrastructure;
using AppCenterExtensions.XamarinForms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Xunit;

namespace AppCenterExtensions.Tests.XamarinForms
{
    public class TrackingApplicationTests
    {
        public TrackingApplicationTests() => MockXamarinForms.Init();

        [Fact]
        public void AppLaunchTime_NotNull()
            => TrackingApplication.AppLaunchTime.Should().NotBeNull();

        [Theory, AutoMoqData]
        public void Invokes_AppCenterSetup_Start(
            string secrets,
            bool anonymizeUser,
            IAppCenterSetup appCenterSetup)
        {
            TrackingApplication.Initialize(secrets, anonymizeUser, appCenterSetup);
            Mock.Get(appCenterSetup).Verify(c => c.Start(secrets, anonymizeUser));
        }


        [Theory, AutoMoqData]
        public void Invokes_AppCenterSetup_Start_With_iOS_Android(
            string appleSecret,
            string androidSecret,
            bool anonymizeUser,
            IAppCenterSetup appCenterSetup)
        {
            TrackingApplication.Initialize(appleSecret, androidSecret, null, null, null, anonymizeUser, appCenterSetup);
            Mock.Get(appCenterSetup)
                .Verify(c => c.Start(appleSecret, androidSecret, null, null, null, anonymizeUser));
        }
    }
}