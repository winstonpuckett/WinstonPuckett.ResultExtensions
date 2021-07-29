using System;
using System.Threading.Tasks;
using Xunit;
using WinstonPuckett.ResultExtensions;

namespace Monads.Functions.Tests
{
    public class TTaskFuncTTaskU_HappyPath_Tests
    {
        private readonly Task<bool> _startingProperty = Task.Run(() => false);
        private Task<bool> _cancelledStartingProperty
            => Task.Run(() => false, new System.Threading.CancellationToken(true));
        private async Task<bool> Flip(bool b)
            => await Task.Run(() => !b);


        [Fact(DisplayName = "Value returns flip of value.")]
        public async Task ReturnsFlippedValue()
        {
            var r = await _startingProperty.Bind(Flip);
            Assert.Equal(!await _startingProperty, ((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "Cancelled token doesn't throw exception.")]
        public async Task CancelledTokenThrowsNoException()
        {
            await _cancelledStartingProperty.Bind(Flip);
        }

        [Fact(DisplayName = "IResult is Ok<T>.")]
        public async Task ReturnsOk()
        {
            var r = await _startingProperty.Bind(Flip);
            Assert.True(r is Ok<bool>);
        }
    }

    public class TTaskFuncTTaskU_SadPath_Tests
    {
        private readonly Task<bool> _startingProperty = Task.Run(() => false);
        private Task<bool> _cancelledStartingProperty
            => Task.Run(() => false, new System.Threading.CancellationToken(true));
        private async Task<bool> ThrowGeneralException(bool _) { await Task.Run(() => throw new Exception()); return false; }
        private async Task<bool> ThrowNotImplementedException(bool _) { await Task.Run(() => throw new NotImplementedException()); return false; }

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
