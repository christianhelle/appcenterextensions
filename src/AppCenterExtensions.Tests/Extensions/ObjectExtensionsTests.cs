using System.Runtime.Serialization;
using AppCenterExtensions.Extensions;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace AppCenterExtensions.Tests.Extensions
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

        [Theory, AutoData]
        public void ToDictionary_Ignores_String(string str)
            => str.ToDictionary().Should().BeEmpty();

        public class ComplexObject
        {
            public string Name { get; set; }
            [IgnoreDataMember]
            public string IgnoredProperty { get; set; }
        }
    }
}