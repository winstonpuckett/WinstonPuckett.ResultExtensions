using System;
using System.Threading.Tasks;
using WinstonPuckett.ResultExtensions;
using Xunit;

namespace Monads.Functions.Tests
{
    public class TaskOkTFuncTU_HappyPath_Tests
    {
        private Task<IResult<bool>> _startingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false));
        private Task<IResult<bool>> _cancelledStartingProperty
            => Task.Run(() => (IResult<bool>)new Ok<bool>(false), new System.Threading.CancellationToken(true));
        private bool Flip(bool b)
            => !b;


        [Fact(DisplayName = "Value returns flip of value.")]
        public async Task ReturnsFlippedValue()
        {
            var r = await _startingProperty.Bind(Flip);
            Assert.Equal(!((Ok<bool>)await _startingProperty).Value, ((Ok<bool>)r).Value);
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

    public class TaskOkTFuncTU_SadPath_Tests
    {
        private Task<IResult<bool>> _startingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false));
        private Task<IResult<bool>> _cancelledStartingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false), new System.Threading.CancellationToken(true));
        private bool ThrowGeneralException(bool _) { throw new Exception(); }
        private bool ThrowNotImplementedException(bool _) { throw new NotImplementedException(); }

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
