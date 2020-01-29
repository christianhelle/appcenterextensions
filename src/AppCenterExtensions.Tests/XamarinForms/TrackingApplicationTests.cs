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

        [Fact]
        public void Requires_AppSecrets()
            => new Action(() => new TrackingApplication(
                    null,
                    false,
                    new Mock<IAppCenterSetup>().Object))
                .Should()
                .ThrowExactly<ArgumentNullException>();

        [Theory, AutoMoqData]
        public void Invokes_AppCenterSetup_Start(string secrets, IAppCenterSetup appCenterSetup)
        {
            new TrackingApplication(secrets, false, appCenterSetup);
            Mock.Get(appCenterSetup).Verify(c => c.Start(secrets));
        }

        [Theory, AutoMoqData]
        public void Invokes_AppCenterSetup_LogLevel(string secrets, IAppCenterSetup appCenterSetup)
        {
            new TrackingApplication(secrets, false, appCenterSetup);
            Mock.Get(appCenterSetup).VerifySet(c => c.LogLevel = LogLevel.Verbose);
        }

        [Theory, AutoMoqData]
        public void Invokes_AppCenterSetup_AnonymizeUser(string secrets, IAppCenterSetup appCenterSetup)
        {
            new TrackingApplication(secrets, true, appCenterSetup);
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
            new TrackingApplication(appleSecret, androidSecret, true, appCenterSetup);
            Mock.Get(appCenterSetup).Verify(c => c.Start(appSecrets));
        }

        [Theory, AutoMoqData]
        public void Initialize_Invokes_Constructor(
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