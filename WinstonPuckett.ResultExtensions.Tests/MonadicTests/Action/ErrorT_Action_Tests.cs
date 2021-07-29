using Xunit;
using System;
using WinstonPuckett.ResultExtensions;

namespace Monads.Actions.Tests
{
    public class ErrorTAction_HappyPath_Tests
    {
        private readonly static string _initialErrorMessage = "I am the initial error message.";
        private readonly IResult<bool> _startingProperty = new Error<bool>(new Exception(_initialErrorMessage));
        private void ThrowNotImplementedException(bool _) { throw new NotImplementedException(); }

        [Fact(DisplayName = "IResult does not call after Error")]
        public void DoesNotContainNewError()
        {
            var r = _startingProperty.Bind(ThrowNotImplementedException);
            Assert.False(r is NotImplementedException);
        }

        [Fact(DisplayName = "IResult contains original Error")]
        public void ContainsOriginalError()
        {
            var r = _startingProperty.Bind(ThrowNotImplementedException);
            Assert.Equal(_initialErrorMessage, ((Error<bool>)r).Exception.Message);
        }
    }
}
