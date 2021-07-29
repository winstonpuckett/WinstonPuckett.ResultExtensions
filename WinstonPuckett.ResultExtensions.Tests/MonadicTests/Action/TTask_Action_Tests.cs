using Xunit;
using System;
using System.Threading.Tasks;
using WinstonPuckett.ResultExtensions;

namespace Monads.Actions.Tests
{
    public class TTaskAction_HappyPath_Tests
    {
        private Task<bool> _startingProperty => Task.Run(() => false);
        private Task<bool> _cancelledStartingProperty => Task.Run(() => false, new System.Threading.CancellationToken(true));
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

    public class TTaskAction_SadPath_Tests
    {
        private Task<bool> _startingProperty => Task.Run(() => false);
        private Task<bool> _cancelledStartingProperty => Task.Run(() => false, new System.Threading.CancellationToken(true));
        private void ThrowGeneralException(bool _) => throw new Exception();
        private void ThrowNotImplementedException(bool _) => throw new NotImplementedException();

        [Fact(DisplayName = "Exception doesn't bubble.")]
        public async Task ExceptionDoesNotBubble()
        {
            await _startingProperty.Bind(ThrowGeneralException);
        }

        [Fact(DisplayName = "Cancelled token doesn't throw exception.")]
        public async Task CancelledTokenThrowsNoException()
        {
            await _cancelledStartingProperty.Bind(ThrowGeneralException);
        }

        [Fact(DisplayName = "Error holds exception.")]
        public async Task ErrorHoldsException()
        {
            var r = await _startingProperty.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }
    }
}
