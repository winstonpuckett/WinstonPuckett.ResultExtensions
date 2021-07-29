using Xunit;
using System;
using System.Threading.Tasks;
using WinstonPuckett.ResultExtensions;

namespace Monads.Actions.Tests
{
    public class TActionAsync_HappyPath_Tests
    {
        private readonly bool _startingProperty = false;
        private async Task DoNothingAsync(bool _) { await Task.Run(()=>{}); }


        [Fact(DisplayName = "Value contains original value.")]
        public async Task ReturnsValueWrappedInIResult()
        {
            var r = await _startingProperty.Bind(DoNothingAsync);
            Assert.False(((Ok<bool>)r).Value);
        }
    }

    public class TActionAsync_SadPath_Tests
    {
        private readonly bool _startingProperty = false;
        private async Task ThrowNotImplementedException(bool _) { await Task.Run(() => throw new NotImplementedException()); }

        [Fact(DisplayName = "Error holds exception.")]
        public async Task ErrorHoldsException()
        {
            var r = await _startingProperty.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }
    }
}
