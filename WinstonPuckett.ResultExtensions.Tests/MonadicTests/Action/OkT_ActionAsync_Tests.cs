using Xunit;
using System;
using WinstonPuckett.ResultExtensions;
using System.Threading.Tasks;

namespace Monads.Actions.Tests
{
    public class OkTActionAsync_HappyPath_Tests
    {
        private readonly IResult<bool> _startingProperty = new Ok<bool>(false);
        private async Task DoNothing(bool _) { await Task.Run(()=>{}); }


        [Fact(DisplayName = "Value contains original value.")]
        public async Task ReturnsValueWrappedInIResult()
        {
            var r = await _startingProperty.Bind(DoNothing);
            Assert.False(((Ok<bool>)r).Value);
        }
    }

    public class OkTActionAsync_SadPath_Tests
    {
        private readonly IResult<bool> _startingProperty = new Ok<bool>(false);
        private async Task ThrowGeneralException(bool _) { await Task.Run(() => throw new Exception()); }
        private async Task ThrowNotImplementedException(bool _) { await Task.Run(() => throw new NotImplementedException()); }


        [Fact(DisplayName = "Exception doesn't bubble.")]
        public async Task ExceptionDoesNotBubble()
        {
            await _startingProperty.Bind(ThrowGeneralException);
        }

        [Fact(DisplayName = "Error holds exception.")]
        public async Task ErrorHoldsException()
        {
            var r = await _startingProperty.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }
    }
}
