using AutoFixture;
using AutoFixture.Xunit2;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Infrastructure
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(CreateCustomizedFixture)
        {
        }

        private static IFixture CreateCustomizedFixture()
            => new Fixture()
                .Customize(
                    new AutoMoqCompositeCustomization());
    }
}
