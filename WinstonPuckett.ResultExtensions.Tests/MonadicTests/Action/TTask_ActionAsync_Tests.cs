using Xunit;
using System;
using System.Threading.Tasks;
using WinstonPuckett.ResultExtensions;

namespace Monads.Actions.Tests
{
    public class TTaskActionAsync_HappyPath_Tests
    {
        private Task<bool> _startingProperty => Task.Run(() => false);
        private Task<bool> _cancelledStartingProperty => Task.Run(() => false, new System.Threading.CancellationToken(true));
        private async Task DoNothingAsync(bool _) { await Task.Run(()=>{}); }


        [Fact(DisplayName = "Value contains original value.")]
        public async Task ReturnsValueWrappedInIResult()
        {
            var r = await _startingProperty.Bind(DoNothingAsync);
            Assert.False(((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "Cancelled token doesn't throw exception.")]
        public async Task CancelledTokenThrowsNoException()
        {
            await _cancelledStartingProperty.Bind(DoNothingAsync);
        }
    }

    public class TTaskActionAsync_SadPath_Tests
    {
        private Task<bool> _startingProperty => Task.Run(() => false);
        private async Task ThrowNotImplementedException(bool _) { await Task.Run(() => throw new NotImplementedException()); }

        [Fact(DisplayName = "Error holds exception.")]
        public async Task ErrorHoldsException()
        {
            var r = await _startingProperty.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }
    }
}
