using Xunit;
using System;
using WinstonPuckett.ResultExtensions;
using System.Threading.Tasks;

namespace Monads.Functions.Tests
{
    public class TFuncTTaskU_HappyPath_Tests
    {
        private readonly bool _startingProperty = false;
        private async Task<bool> Flip(bool b) => await Task.Run(() => !b);


        [Fact(DisplayName = "Value returns flip of value.")]
        public async Task ReturnsFlippedValue()
        {
            var r = await _startingProperty.Bind(Flip);
            Assert.Equal(!_startingProperty, ((Ok<bool>)r).Value);
        }
    }

    public class TFuncTTaskU_SadPath_Tests
    {
        private readonly bool _startingProperty = false;
        private async Task<bool> ThrowGeneralException(bool _) { await Task.Run(() => throw new Exception()); return false; }
        private async Task<bool> ThrowNotImplementedException(bool _) { await Task.Run(() => throw new NotImplementedException()); return false; }

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
