using Xunit;
using System;

namespace WinstonPuckett.ResultExtensions.Tests
{
    public class TAction_HappyPath_Tests
    {
        private readonly bool _startingProperty = false;
        private void DoNothing(bool _) { }


        [Fact(DisplayName = "Value contains original value.")]
        public void ReturnsValueWrappedInIResult()
        {
            var r = _startingProperty.Bind(DoNothing);
            Assert.False(r.Value);
        }
     
        [Fact(DisplayName = "IResult is Ok<T>.")]
        public void ReturnsOk()
        {
            var r = _startingProperty.Bind(DoNothing);
            Assert.True(r is Ok<bool>);
        }
    }

    public class TAction_SadPath_Tests
    {
        private readonly bool _startingProperty = false;
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
        
        [Fact(DisplayName = "Error holds starting value.")]
        public void ErrorHoldsStartingValue()
        {
            var r = _startingProperty.Bind(ThrowGeneralException);
            Assert.False(r.Value);
        }

        [Fact(DisplayName = "Error holds exception.")]
        public void ErrorHoldsException()
        {
            var r = _startingProperty.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }
    }

}
