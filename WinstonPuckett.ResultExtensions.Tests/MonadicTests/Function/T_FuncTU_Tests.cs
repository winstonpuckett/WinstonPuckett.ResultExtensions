using Xunit;
using System;

namespace WinstonPuckett.ResultExtensions.Tests
{
    public class TFuncTU_HappyPath_Tests
    {
        private readonly bool _startingProperty = false;
        private bool Flip(bool b) 
            => !b;


        [Fact(DisplayName = "Value contains original value.")]
        public void ReturnsValueWrappedInIResult()
        {
            var r = _startingProperty.Bind(Flip);
            Assert.Equal(!_startingProperty, ((Ok<bool>)r).Value);
        }
     
        [Fact(DisplayName = "IResult is Ok<T>.")]
        public void ReturnsOk()
        {
            var r = _startingProperty.Bind(Flip);
            Assert.True(r is Ok<bool>);
        }
    }

    public class TFuncTU_SadPath_Tests
    {
        private readonly bool _startingProperty = false;
        private bool ThrowGeneralException(bool _) { throw new Exception(); }
        private bool ThrowNotImplementedException(bool _) { throw new NotImplementedException(); }

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
