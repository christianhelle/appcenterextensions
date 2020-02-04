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
        public void Build_Returns_NotNull(
            AppCenterSecretsBuilder sut,
            string appleSecret,
            string androidSecret)
            => sut
                .SetAppleSecret(appleSecret)
                .SetAndroidSecret(androidSecret)
                .Build()
                .Should()
                .NotBeNullOrWhiteSpace();

        [Theory, AutoMoqData]
        public void Build_Returns_AppCenterSecret(
            AppCenterSecretsBuilder sut,
            string appleSecret,
            string androidSecret)
            => sut
                .SetAppleSecret(appleSecret)
                .SetAndroidSecret(androidSecret)
                .Build()
                .Should()
                .Be($"ios={appleSecret};android={androidSecret}");

        [Theory, AutoMoqData]
        public void Static_Build_Returns_AppCenterSecret(
            string appleSecret,
            string androidSecret)
            => AppCenterSecretsBuilder
                .Build(appleSecret, androidSecret)
                .Should()
                .Be($"ios={appleSecret};android={androidSecret}");
    }
}