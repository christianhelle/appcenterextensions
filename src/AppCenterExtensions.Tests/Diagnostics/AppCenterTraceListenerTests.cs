using System;
using System.Collections.Generic;
using AppCenterExtensions.Diagnostics;
using AppCenterExtensions.Tests.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

namespace AppCenterExtensions.Tests.Diagnostics
{
    public class AppCenterTraceListenerTests
    {
        [Theory, AutoMoqData]
        public void Constructor_Sets_Analytics(IAnalytics analytics, ICrashes crashes)
            => new AppCenterTraceListener(analytics, crashes)
                .Analytics
                .Should()
                .Be(analytics);

        [Theory, AutoMoqData]
        public void Constructor_Sets_Crashes(IAnalytics analytics, ICrashes crashes)
            => new AppCenterTraceListener(analytics, crashes)
                .Crashes
                .Should()
                .Be(crashes);

        [Theory, AutoMoqData]
        public void Write_Null_Does_Nothing(
            AppCenterTraceListener sut)
        {
            sut.Write(null as object);
            AssertDoesNothing(sut);
        }

        [Theory, AutoMoqData]
        public void WriteLine_Null_Does_Nothing(
            AppCenterTraceListener sut)
        {
            sut.WriteLine(null as object);
            AssertDoesNothing(sut);
        }

        [Theory, AutoMoqData]
        public void Write_With_Category_Null_Does_Nothing(
            AppCenterTraceListener sut,
            string category)
        {
            sut.Write(null as object, category);
            AssertDoesNothing(sut);
        }

        [Theory, AutoMoqData]
        public void WriteLine_With_Category_Null_Does_Nothing(
            AppCenterTraceListener sut,
            string category)
        {
            sut.WriteLine(null as object, category);
            AssertDoesNothing(sut);
        }

        private static void AssertDoesNothing(AppCenterTraceListener sut)
        {
            Mock.Get(sut.Crashes)
                .Verify(
                    c => c.TrackError(
                        It.IsAny<Exception>(),
                        It.IsAny<IDictionary<string, string>>()),
                    Times.Never);

            Mock.Get(sut.Analytics)
                .Verify(c => c.TrackEvent(
                        It.IsAny<string>(),
                        It.IsAny<IDictionary<string, string>>()),
                    Times.Never);
        }

        [Theory, AutoMoqData]
        public void Write_Exception_Invokes_Crashes_TrackError(
            AppCenterTraceListener sut,
            Exception exception)
        {
            sut.Write(exception);
            Mock.Get(sut.Crashes)
                .Verify(c => c.TrackError(exception, null));
        }

        [Theory, AutoMoqData]
        public void WriteLine_Exception_Invokes_Crashes_TrackError(
            AppCenterTraceListener sut,
            Exception exception)
        {
            sut.WriteLine(exception);
            Mock.Get(sut.Crashes)
                .Verify(c => c.TrackError(exception, null));
        }

        [Theory, AutoMoqData]
        public void Write_With_Category_Exception_Invokes_Crashes_TrackError(
            AppCenterTraceListener sut,
            Exception exception,
            string category)
        {
            sut.Write(exception, category);
            Mock.Get(sut.Crashes)
                .Verify(c => c.TrackError(exception, null));
        }

        [Theory, AutoMoqData]
        public void WriteLine_With_Category_Exception_Invokes_Crashes_TrackError(
            AppCenterTraceListener sut,
            Exception exception,
            string category)
        {
            sut.WriteLine(exception, category);
            Mock.Get(sut.Crashes)
                .Verify(c => c.TrackError(exception, null));
        }

        [Theory, AutoMoqData]
        public void Write_AnalyticsEvent_Invokes_Analytics_TrackEvent(
            AppCenterTraceListener sut,
            AnalyticsEvent analyticsEvent)
        {
            sut.Write(analyticsEvent);
            Mock.Get(sut.Analytics)
                .Verify(c => c.TrackEvent(analyticsEvent.EventName, analyticsEvent.Properties));
        }

        [Theory, AutoMoqData]
        public void Write_With_Category_AnalyticsEvent_Invokes_Analytics_TrackEvent(
            AppCenterTraceListener sut,
            AnalyticsEvent analyticsEvent,
            string category)
        {
            sut.Write(analyticsEvent, category);
            Mock.Get(sut.Analytics)
                .Verify(c => c.TrackEvent(analyticsEvent.EventName, analyticsEvent.Properties));
        }

        [Theory, AutoMoqData]
        public void WriteLine_AnalyticsEvent_Invokes_Analytics_TrackEvent(
            AppCenterTraceListener sut,
            AnalyticsEvent analyticsEvent)
        {
            sut.WriteLine(analyticsEvent);
            Mock.Get(sut.Analytics)
                .Verify(c => c.TrackEvent(analyticsEvent.EventName, analyticsEvent.Properties));
        }

        [Theory, AutoMoqData]
        public void WriteLine_With_Category_AnalyticsEvent_Invokes_Analytics_TrackEvent(
            AppCenterTraceListener sut,
            AnalyticsEvent analyticsEvent,
            string category)
        {
            sut.WriteLine(analyticsEvent, category);
            Mock.Get(sut.Analytics)
                .Verify(c => c.TrackEvent(analyticsEvent.EventName, analyticsEvent.Properties));
        }
    }
}