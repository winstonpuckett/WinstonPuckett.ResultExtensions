using Xunit;
using System;
using WinstonPuckett.ResultExtensions;

namespace Monads.Functions.Tests
{
    public class OkTFuncTU_HappyPath_Tests
    {
        private readonly IResult<bool> _startingProperty = new Ok<bool>(false);
        private bool Flip(bool b) => !b;


        [Fact(DisplayName = "Value returns flip of value.")]
        public void ReturnsFlippedValue()
        {
            var r = _startingProperty.Bind(Flip);
            Assert.Equal(!((Ok<bool>)_startingProperty).Value, ((Ok<bool>)r).Value);
        }
    }

    public class OkTFuncTU_SadPath_Tests
    {
        private readonly IResult<bool> _startingProperty = new Ok<bool>(false);
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
