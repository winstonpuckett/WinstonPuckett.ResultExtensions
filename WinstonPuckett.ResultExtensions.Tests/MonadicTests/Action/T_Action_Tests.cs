using Xunit;
using System;
using WinstonPuckett.ResultExtensions;

namespace Monads.Actions.Tests
{
    public class TAction_HappyPath_Tests
    {
        private readonly bool _startingProperty = false;
        private void DoNothing(bool _) { }


        [Fact(DisplayName = "Value contains original value.")]
        public void ReturnsValueWrappedInIResult()
        {
            var r = _startingProperty.Bind(DoNothing);
            Assert.False(((Ok<bool>)r).Value);
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

        [Fact(DisplayName = "Error holds exception.")]
        public void ErrorHoldsException()
        {
            var r = _startingProperty.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }
    }
}
