using System;
using System.Collections.Generic;
using AppCenterExtensions.Extensions;
using AppCenterExtensions.Tests.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

namespace AppCenterExtensions.Tests.Extensions
{
    public class ExceptionTests
    {
        [Theory, AutoMoqData]
        public void Invokes_TrackError(
            Exception exception,
            IDictionary<string, string> properties,
            ICrashes sut)
        {
            exception.Report(properties, sut);
            Mock.Get(sut).Verify(c => c.TrackError(exception, properties));
        }

        [Theory, AutoMoqData]
        public void Invokes_Exception_Thrown_By_TrackError(
            Exception exception,
            Exception thrown,
            IDictionary<string, string> properties,
            ICrashes sut)
        {
            var mock = Mock.Get(sut);
            mock.Setup(c => c.TrackError(exception, properties))
                .Throws(thrown);
            exception.Report(properties, sut);
            mock.Verify(c => c.TrackError(thrown, null));
        }

        [Theory, AutoMoqData]
        public void Invokes_Uses_Default_AppCenterCrashes(
            Exception exception,
            IDictionary<string, string> properties)
        {
            new Action(() => exception.Report(properties))
                .Should()
                .NotThrow();
        }
    }
}