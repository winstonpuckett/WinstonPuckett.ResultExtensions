using Xunit;
using System;

namespace WinstonPuckett.ResultExtensions.Tests
{
    public class ErrorTFuncTU_HappyPath_Tests
    {
        private readonly static string _initialErrorMessage = "I am the initial error message.";
        private readonly IResult<bool> _startingProperty = new Error<bool>(new Exception(_initialErrorMessage));
        private bool Flip(bool b) 
            => !b;
        private bool ThrowNotImplementedException(bool _) { throw new NotImplementedException(); }
     
        [Fact(DisplayName = "IResult is Error<T>.")]
        public void ReturnsError()
        {
            var r = _startingProperty.Bind(Flip);
            Assert.True(r is Error<bool>);
        }

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
