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
    }

    public class OkTAction_SadPath_Tests
    {
        private readonly IResult<bool> _startingProperty = new Ok<bool>(false);
        private void ThrowNotImplementedException(bool _) { throw new NotImplementedException(); }

        [Fact(DisplayName = "Error holds exception.")]
        public void ErrorHoldsException()
        {
            var r = _startingProperty.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }
    }
}
