using Xunit;
using System;
using WinstonPuckett.ResultExtensions;

namespace Monads.Functions.Tests
{
    public class TFuncTU_HappyPath_Tests
    {
        private readonly bool _startingProperty = false;
        private bool Flip(bool b) => !b;

        [Fact(DisplayName = "Value returns flip of value.")]
        public void ReturnsFlippedValue()
        {
            var r = _startingProperty.Bind(Flip);
            Assert.Equal(!_startingProperty, ((Ok<bool>)r).Value);
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

        [Fact(DisplayName = "Error holds exception.")]
        public void ErrorHoldsException()
        {
            var r = _startingProperty.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }
    }
}
