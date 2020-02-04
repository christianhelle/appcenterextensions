using AppCenterExtensions.Extensions;
using FluentAssertions;
using Xunit;

namespace AppCenterExtensions.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("SomeButtonTappedCommand")]
        [InlineData("SomeViewScrolledCommand")]
        [InlineData("SettingsViewModel")]
        [InlineData("LoginViewModel")]
        [InlineData("SettingsPage")]
        [InlineData("LoginPage")]
        [InlineData("RefreshDataAsync")]
        [InlineData("SubmitDataAsync")]
        public void ToTrackingEventName_Returns_NotNullOrEmpty(string className)
            => className.ToTrackingEventName().Should().NotBeNullOrEmpty();
        
        [Theory]
        [InlineData("SomeButtonTappedCommand")]
        [InlineData("SomeViewScrolledCommand")]
        [InlineData("SettingsViewModel")]
        [InlineData("LoginViewModel")]
        [InlineData("SettingsPage")]
        [InlineData("LoginPage")]
        [InlineData("RefreshDataAsync")]
        [InlineData("SubmitDataAsync")]
        public void ToTrackingEventName_Splits_Class_Name_To_Words(string className)
            => className.ToTrackingEventName().Split(" ").Should().NotBeNullOrEmpty();

        [Fact]
        public void TrimNames_NotEmpty()
            => StringExtension.TrimNames.Should().NotBeNullOrEmpty();
    }
}
