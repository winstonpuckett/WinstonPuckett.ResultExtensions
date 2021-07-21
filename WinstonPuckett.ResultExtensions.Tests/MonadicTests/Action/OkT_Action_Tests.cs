using Xunit;
using System;
using WinstonPuckett.ResultExtensions;

namespace Monads.Actions.Tests
{
    public class OkTAction_HappyPath_Tests
    {
        private readonly IResult<bool> _startingProperty = new Ok<bool>(false);
        private void DoNothing(bool _) { }


        [Fact(DisplayName = "Value contains original value.")]
        public void ReturnsValueWrappedInIResult()
        {
            var r = _startingProperty.Bind(DoNothing);
            Assert.False(((Ok<bool>)r).Value);
        }
     
        [Fact(DisplayName = "IResult is Ok<T>.")]
        public void ReturnsOk()
        {
            var r = _startingProperty.Bind(DoNothing);
            Assert.True(r is Ok<bool>);
        }
    }

    public class OkTAction_SadPath_Tests
    {
        private readonly IResult<bool> _startingProperty = new Ok<bool>(false);
        private void ThrowGeneralException(bool _) { throw new Exception(); }
        private void ThrowNotImplementedException(bool _) { throw new NotImplementedException(); }

        [Fact(DisplayName = "Exception doesn't bubble.")]
        public void ExceptionDoesNotBubble()
        {
            _startingProperty.Bind(ThrowGeneralException);
        }
        
        [Fact(DisplayName = "IResult is Error<T>.")]
        public void IResultIsError()
        {
            var r = _startingProperty.Bind(ThrowGeneralException);
            Assert.True(r is Error<bool>);
        }

        [Fact(DisplayName = "Error holds exception.")]
        public void ErrorHoldsException()
        {
            var r = _startingProperty.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }
    }
}
