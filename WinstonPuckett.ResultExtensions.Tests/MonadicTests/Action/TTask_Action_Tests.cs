using Xunit;
using System;
using System.Threading.Tasks;

namespace WinstonPuckett.ResultExtensions.Tests
{
    public class TTaskAction_HappyPath_Tests
    {
        private Task<bool> _startingProperty => Task.Run(() => false);
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

    public class TTaskAction_SadPath_Tests
    {
        private Task<bool> _startingProperty => Task.Run(() => false);
        private Task<bool> _cancelledStartingProperty 
            => Task.Run(()=>false, new System.Threading.CancellationToken(true));
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
