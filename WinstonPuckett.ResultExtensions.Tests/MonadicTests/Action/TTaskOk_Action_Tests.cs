using Xunit;
using System;
using WinstonPuckett.ResultExtensions;
using System.Threading.Tasks;

namespace Monads.Actions.Tests
{
    public class TaskOkTAction_HappyPath_Tests
    {
        private Task<IResult<bool>> _startingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false));
        private Task<IResult<bool>> _cancelledStartingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false), new System.Threading.CancellationToken(true));
        private void DoNothing(bool _) { }


        [Fact(DisplayName = "Value contains original value.")]
        public async Task ReturnsValueWrappedInIResult()
        {
            var r = await _startingProperty.Bind(DoNothing);
            Assert.False(((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "Cancelled token doesn't throw exception.")]
        public async Task CancelledTokenThrowsNoException()
        {
            await _cancelledStartingProperty.Bind(DoNothing);
        }
    }

    public class TaskOkTAction_SadPath_Tests
    {
        private Task<IResult<bool>> _startingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false));
        private void ThrowNotImplementedException(bool _) { throw new NotImplementedException(); }

        [Fact(DisplayName = "Error holds exception.")]
        public async Task ErrorHoldsException()
        {
            var r = await _startingProperty.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }
    }
}
