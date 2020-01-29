using System;
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

        [Theory, AutoData]
        public void Requires_ApppleSecret(string secret)
            => new Action(() => TrackingApplication.Initialize(
                    null,
                    secret,
                    false,
                    new Mock<IAppCenterSetup>().Object))
                .Should()
                .ThrowExactly<ArgumentNullException>();

        [Theory, AutoData]
        public void Requires_AndroidSecret(string secret)
            => new Action(() => TrackingApplication.Initialize(
                    secret,
                    null,
                    false,
                    new Mock<IAppCenterSetup>().Object))
                .Should()
                .ThrowExactly<ArgumentNullException>();

        [Theory, AutoMoqData]
        public void Invokes_AppCenterSetup_Start(string secrets, IAppCenterSetup appCenterSetup)
        {
            TrackingApplication.Initialize(secrets, false, appCenterSetup);
            Mock.Get(appCenterSetup).Verify(c => c.Start(secrets));
        }

        [Theory, AutoMoqData]
        public void Invokes_AppCenterSetup_LogLevel(string secrets, IAppCenterSetup appCenterSetup)
        {
            TrackingApplication.Initialize(secrets, false, appCenterSetup);
            Mock.Get(appCenterSetup).VerifySet(c => c.LogLevel = LogLevel.Verbose);
        }

        [Theory, AutoMoqData]
        public void Invokes_AppCenterSetup_AnonymizeUser(string secrets, IAppCenterSetup appCenterSetup)
        {
            TrackingApplication.Initialize(secrets, true, appCenterSetup);
            Mock.Get(appCenterSetup).Verify(c => c.UseAnonymousUserIdAsync());
        }

        [Fact]
        public void AppLaunchTime_NotNull()
            => TrackingApplication.AppLaunchTime.Should().NotBeNull();

        [Theory, AutoMoqData]
        public void Invokes_AppCenterSetup_Start_For_iOS_Android(
            string appleSecret,
            string androidSecret,
            IAppCenterSetup appCenterSetup)
        {
            var appSecrets = $"ios={appleSecret};android={androidSecret}";
            TrackingApplication.Initialize(appleSecret, androidSecret, true, appCenterSetup);
            Mock.Get(appCenterSetup).Verify(c => c.Start(appSecrets));
        }
    }
}