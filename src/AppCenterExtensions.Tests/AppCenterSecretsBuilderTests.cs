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
        public void Build_Returns_NotNull(
            AppCenterSecretsBuilder sut,
            string appleSecret,
            string androidSecret,
            string uwpSecret)
            => sut
                .SetAppleSecret(appleSecret)
                .SetAndroidSecret(androidSecret)
                .SetUwpSecret(uwpSecret)
                .Build()
                .Should()
                .NotBeNullOrWhiteSpace();

        [Theory, AutoMoqData]
        public void Build_Returns_AppCenterSecret(
            AppCenterSecretsBuilder sut,
            string appleSecret,
            string androidSecret,
            string uwpSecret)
            => sut
                .SetAppleSecret(appleSecret)
                .SetAndroidSecret(androidSecret)
                .SetUwpSecret(uwpSecret)
                .Build()
                .Should()
                .Be($"ios={appleSecret};android={androidSecret};uwp={uwpSecret};");

        [Theory, AutoMoqData]
        public void Static_Build_Returns_AppCenterSecret(
            string appleSecret,
            string androidSecret,
            string uwpSecret)
            => AppCenterSecretsBuilder
                .Build(appleSecret, androidSecret, uwpSecret)
                .Should()
                .Be($"ios={appleSecret};android={androidSecret};uwp={uwpSecret};");
    }
}