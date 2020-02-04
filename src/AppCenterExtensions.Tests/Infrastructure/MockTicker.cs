using System.Diagnostics.CodeAnalysis;
using Xamarin.Forms.Internals;

namespace AppCenterExtensions.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    internal class MockTicker : Ticker
    {
        bool _enabled;

        protected override void EnableTimer()
        {
            _enabled = true;

            while (_enabled)
            {
                SendSignals(16);
            }
        }

        protected override void DisableTimer()
        {
            _enabled = false;
        }
    }
}