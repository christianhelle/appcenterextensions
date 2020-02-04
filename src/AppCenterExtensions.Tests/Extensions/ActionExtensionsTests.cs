using System;
using AppCenterExtensions.Extensions;
using FluentAssertions;
using Xunit;

namespace AppCenterExtensions.Tests.Extensions
{
    public class ActionExtensionsTests
    {
        [Fact]
        public void SafeInvoke_Should_NotThrow_Exceptions()
        {
            var action = new Action(() => throw new ApplicationException("Test"));
            new Action(() => action.SafeInvoke()).Should().NotThrow<ApplicationException>();
        }

        [Fact]
        public void SafeInvoke_Calls_OnError_Upon_Exception()
        {
            Exception callbackException = null;
            var action = new Action(() => throw new ApplicationException("Test"));
            action.SafeInvoke(new Action<Exception>(e => callbackException = e));
            callbackException.Should().BeOfType<ApplicationException>();
            callbackException.Message.Should().Be("Test");
        }
    }
}
