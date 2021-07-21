using Xunit;
using System;
using WinstonPuckett.ResultExtensions;
using System.Threading.Tasks;

namespace Monads.Actions.Tests
{
    public class TaskOkTActionAsync_HappyPath_Tests
    {
        private Task<IResult<bool>> _startingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false));
        private async Task DoNothing(bool _) { await Task.Run(() => { }); }



        [Fact(DisplayName = "Value contains original value.")]
        public async Task ReturnsValueWrappedInIResult()
        {
            var r = await _startingProperty.Bind(DoNothing);
            Assert.False(((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "IResult is Ok<T>.")]
        public async Task ReturnsOk()
        {
            var r = await _startingProperty.Bind(DoNothing);
            Assert.True(r is Ok<bool>);
        }
    }

    public class TaskOkTActionAsync_SadPath_Tests
    {
        private Task<IResult<bool>> _startingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false));
        private async Task ThrowGeneralException(bool _) { await Task.Run(() => throw new Exception()); }
        private async Task ThrowNotImplementedException(bool _) { await Task.Run(() => throw new NotImplementedException()); }


        [Fact(DisplayName = "Exception doesn't bubble.")]
        public async Task ExceptionDoesNotBubble()
        {
            await _startingProperty.Bind(ThrowGeneralException);
        }

        [Fact(DisplayName = "IResult is Error<T>.")]
        public async Task IResultIsError()
        {
            var r = await _startingProperty.Bind(ThrowGeneralException);
            Assert.True(r is Error<bool>);
        }

        [Fact(DisplayName = "Error holds exception.")]
        public async Task ErrorHoldsException()
        {
            var r = await _startingProperty.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }
    }
}
