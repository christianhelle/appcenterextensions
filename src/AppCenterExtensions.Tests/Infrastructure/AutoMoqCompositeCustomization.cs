using AutoFixture;
using AutoFixture.AutoMoq;

namespace AppCenterExtensions.Tests.Infrastructure
{
    public class AutoMoqCompositeCustomization : CompositeCustomization
    {
        public AutoMoqCompositeCustomization()
            : base(new AutoMoqCustomization()) { }
    }
}