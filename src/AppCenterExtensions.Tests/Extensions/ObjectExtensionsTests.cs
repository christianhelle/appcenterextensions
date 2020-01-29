using System.Runtime.Serialization;
using AutoFixture.Xunit2;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions;
using FluentAssertions;
using Xunit;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Extensions
{
    public class ObjectExtensionsTests
    {
        [Theory, AutoData]
        public void ToDictionary_Returns_NotNull(ComplexObject obj) 
            => obj.ToDictionary().Should().NotBeNull();

        [Theory, AutoData]
        public void ToDictionary_Returns_NotEmpty(ComplexObject obj)
            => obj.ToDictionary().Should().ContainKey(nameof(ComplexObject.Name));

        [Theory, AutoData]
        public void ToDictionary_Respects_IgnoreDataMember(ComplexObject obj)
            => obj.ToDictionary().Should().NotContainKey(nameof(ComplexObject.IgnoredProperty));

        public class ComplexObject
        {
            public string Name { get; set; }
            [IgnoreDataMember]
            public string IgnoredProperty { get; set; }
        }
    }
}