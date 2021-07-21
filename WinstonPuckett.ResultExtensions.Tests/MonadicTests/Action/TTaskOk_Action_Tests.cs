using Xunit;
using System;
using WinstonPuckett.ResultExtensions;
using System.Threading.Tasks;

namespace Monads.Actions.Tests
{
    public class TaskOkTAction_HappyPath_Tests
    {
        private Task<IResult<bool>> _startingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false));
        private void DoNothing(bool _) { }


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

    public class TaskOkTAction_SadPath_Tests
    {
        private Task<IResult<bool>> _startingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false));
        private void ThrowGeneralException(bool _) { throw new Exception(); }
        private void ThrowNotImplementedException(bool _) { throw new NotImplementedException(); }

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
