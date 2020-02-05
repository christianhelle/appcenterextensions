using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AppCenterExtensions.Diagnostics
{
    /// <summary>
    /// A Trace Listener implementation that automatically sends Exceptions
    /// to AppCenter Diagnostics and instances of AnalyticsEvent to AppCenter Analytics
    /// </summary>
    public class AppCenterTraceListener : TraceListener
    {
        /// <summary>
        /// Creates an instance of AppCenterTraceListener
        /// </summary>
        /// <param name="analytics">Keep this as NULL to use the default implementation. This is only exposed for unit testing purposes</param>
        /// <param name="crashes">Keep this as NULL to use the default implementation. This is only exposed for unit testing purposes</param>
        public AppCenterTraceListener(IAnalytics analytics = null, ICrashes crashes = null)
        {
            Analytics = analytics ?? AppCenterAnalytics.Instance;
            Crashes = crashes ?? AppCenterCrashes.Instance;
        }

        /// <summary>
        /// Gets the IAnalytics implementation in use
        /// </summary>
        public IAnalytics Analytics { get; }

        /// <summary>
        /// Gets the ICrashes implementation in use
        /// </summary>
        public ICrashes Crashes { get; }

        /// <summary>When overridden in a derived class, writes the specified message to the listener you create in the derived class.</summary>
        /// <param name="message">A message to write.</param>
        [ExcludeFromCodeCoverage]
        public override void Write(string message)
        {
        }

        /// <summary>Writes the value of the object's <see cref="M:System.Object.ToString"></see> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"></see> class.</summary>
        /// <param name="o">An <see cref="T:System.Object"></see> whose fully qualified class name you want to write.</param>
        public override void Write(object o)
            => WriteInternal(o);

        /// <summary>Writes a category name and the value of the object's <see cref="M:System.Object.ToString"></see> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"></see> class.</summary>
        /// <param name="o">An <see cref="T:System.Object"></see> whose fully qualified class name you want to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
        public override void Write(object o, string category)
            => WriteInternal(o);

        /// <summary>When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.</summary>
        /// <param name="message">A message to write.</param>
        [ExcludeFromCodeCoverage]
        public override void WriteLine(string message)
        {
        }

        /// <summary>Writes the value of the object's <see cref="M:System.Object.ToString"></see> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"></see> class, followed by a line terminator.</summary>
        /// <param name="o">An <see cref="T:System.Object"></see> whose fully qualified class name you want to write.</param>
        public override void WriteLine(object o)
            => WriteInternal(o);

        /// <summary>Writes a category name and the value of the object's <see cref="M:System.Object.ToString"></see> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener"></see> class, followed by a line terminator.</summary>
        /// <param name="o">An <see cref="T:System.Object"></see> whose fully qualified class name you want to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
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