using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AppCenterExtensions.Diagnostics
{
    public class AppCenterTraceListener : TraceListener
    {
        public AppCenterTraceListener(IAnalytics analytics = null, ICrashes crashes = null)
        {
            Analytics = analytics ?? AppCenterAnalytics.Instance;
            Crashes = crashes ?? AppCenterCrashes.Instance;
        }

        public IAnalytics Analytics { get; }
        public ICrashes Crashes { get; }

        [ExcludeFromCodeCoverage]
        public override void Write(string message)
        {
        }

        public override void Write(object o)
            => WriteInternal(o);

        public override void Write(object o, string category)
            => WriteInternal(o);

        [ExcludeFromCodeCoverage]
        public override void WriteLine(string message)
        {
        }

        public override void WriteLine(object o)
            => WriteInternal(o);

        public override void WriteLine(object o, string category)
            => WriteInternal(o);

        private void WriteInternal(object o)
        {
            if (o == null)
                return;

            switch (o)
            {
                case Exception exception:
                    Crashes.TrackError(exception);
                    break;

                case AnalyticsEvent analyticsEvent:
                    Analytics.TrackEvent(analyticsEvent.EventName, analyticsEvent.Properties);
                    break;
            }
        }
    }
}