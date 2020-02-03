using System;
using System.Diagnostics;
using AutoFixture.Xunit2;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Infrastructure;
using ChristianHelle.DeveloperTools.AppCenterExtensions.XamarinForms;
using FluentAssertions;
using Microsoft.AppCenter;
using Moq;
using Xunit;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.XamarinForms
{
    public class TrackingApplicationTests
    {
        public TrackingApplicationTests() => MockXamarinForms.Init();

        [Fact]
        public void AppLaunchTime_NotNull()
            => TrackingApplication.AppLaunchTime.Should().NotBeNull();

        [Theory, AutoData]
        public void Requires_ApppleSecret(string secret)
            => new Action(() => TrackingApplication.Initialize(
                    null,
                    secret,
                    false))
                .Should()
                .ThrowExactly<ArgumentNullException>();

        [Theory, AutoData]
        public void Requires_AndroidSecret(string secret)
            => new Action(() => TrackingApplication.Initialize(
                    secret,
                    null,
                    false))
                .Should()
                .ThrowExactly<ArgumentNullException>();

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
            TrackingApplication.Initialize(appleSecret, androidSecret, anonymizeUser, appCenterSetup);
            Mock.Get(appCenterSetup).Verify(c => c.Start(appleSecret, androidSecret, anonymizeUser));
        }
    }
}