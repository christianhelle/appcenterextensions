using AutoFixture;
using AutoFixture.AutoMoq;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Tests.Infrastructure
{
    public class AutoMoqCompositeCustomization : CompositeCustomization
    {
        public AutoMoqCompositeCustomization()
            : base(new AutoMoqCustomization()) { }
    }
}