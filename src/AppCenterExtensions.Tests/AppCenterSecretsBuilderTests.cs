using System;
using AppCenterExtensions.Tests.Infrastructure;
using FluentAssertions;
using Xunit;

namespace AppCenterExtensions.Tests
{
    public class AppCenterSecretsBuilderTests
    {
        [Theory, AutoMoqData]
        public void Requires_AppleSecret(AppCenterSecretsBuilder sut)
            => new Action(() => sut.SetAppleSecret(null))
                .Should()
                .ThrowExactly<ArgumentNullException>();

        [Theory, AutoMoqData]
        public void Requires_AndroidSecret(AppCenterSecretsBuilder sut)
            => new Action(() => sut.SetAndroidSecret(null))
                .Should()
                .ThrowExactly<ArgumentNullException>();

        [Theory, AutoMoqData]
        public void Requires_UwpSecret(AppCenterSecretsBuilder sut)
            => new Action(() => sut.SetUwpSecret(null))
                .Should()
                .ThrowExactly<ArgumentNullException>();

        [Theory, AutoMoqData]
        public void Requires_MacOSSecret(AppCenterSecretsBuilder sut)
            => new Action(() => sut.SetMacOSSecret(null))
                .Should()
                .ThrowExactly<ArgumentNullException>();

        [Theory, AutoMoqData]
        public void Requires_WindowsDesktopSecret(AppCenterSecretsBuilder sut)
            => new Action(() => sut.SetWindowsDesktopSecret(null))
                .Should()
                .ThrowExactly<ArgumentNullException>();

        [Theory, AutoMoqData]
        public void Build_Returns_NotNull(
            AppCenterSecretsBuilder sut,
            string appleSecret,
            string androidSecret,
            string uwpSecret,
            string macosSecret,
            string windowsDesktopSecret)
            => sut
                .SetAppleSecret(appleSecret)
                .SetAndroidSecret(androidSecret)
                .SetUwpSecret(uwpSecret)
                .SetMacOSSecret(macosSecret)
                .SetWindowsDesktopSecret(windowsDesktopSecret)
                .Build()
                .Should()
                .NotBeNullOrWhiteSpace();

        [Theory, AutoMoqData]
        public void Build_Returns_AppCenterSecret(
            AppCenterSecretsBuilder sut,
            string appleSecret,
            string androidSecret,
            string uwpSecret,
            string macosSecret,
            string windowsDesktopSecret)
            => sut
                .SetAppleSecret(appleSecret)
                .SetAndroidSecret(androidSecret)
                .SetUwpSecret(uwpSecret)
                .SetMacOSSecret(macosSecret)
                .SetWindowsDesktopSecret(windowsDesktopSecret)
                .Build()
                .Should()
                .Be($"ios={appleSecret};android={androidSecret};uwp={uwpSecret};macos={macosSecret};windowsdesktop={windowsDesktopSecret};");

        [Theory, AutoMoqData]
        public void Static_Build_Returns_AppCenterSecret(
            string appleSecret,
            string androidSecret,
            string uwpSecret,
            string macosSecret,
            string windowsDesktopSecret)
            => AppCenterSecretsBuilder
                .Build(appleSecret, androidSecret, uwpSecret, macosSecret, windowsDesktopSecret)
                .Should()
                .Be($"ios={appleSecret};android={androidSecret};uwp={uwpSecret};macos={macosSecret};windowsdesktop={windowsDesktopSecret};");
    }
}