using Xunit;
using System;
using WinstonPuckett.ResultExtensions;
using System.Threading.Tasks;

namespace Monads.Functions.Tests
{
    public class TaskErrorTFuncTTaskU_HappyPath_Tests
    {
        private readonly static string _initialErrorMessage = "I am the initial error message.";
        private Task<IResult<bool>> _startingProperty => Task.Run(() => (IResult<bool>)new Error<bool>(new Exception(_initialErrorMessage)));
        private Task<IResult<bool>> _cancelledStartingProperty => Task.Run(() => (IResult<bool>)new Error<bool>(new Exception(_initialErrorMessage)), new System.Threading.CancellationToken(true));
        private async Task<bool> Flip(bool b) => await Task.Run(() => !b);
        private async Task<bool> ThrowNotImplementedException(bool _) { await Task.Run(() => throw new NotImplementedException()); return false; }

        [Fact(DisplayName = "Cancelled token doesn't throw exception.")]
        public async Task CancelledTokenThrowsNoException()
        {
            await _cancelledStartingProperty.Bind(Flip);
        }

        [Fact(DisplayName = "IResult does not call after Error")]
        public async Task DoesNotContainNewError()
        {
            var r = await _startingProperty.Bind(ThrowNotImplementedException);
            Assert.False(r is NotImplementedException);
        }

        [Fact(DisplayName = "IResult contains original Error")]
        public async Task ContainsOriginalError()
        {
            var r = await _startingProperty.Bind(ThrowNotImplementedException);
            Assert.Equal(_initialErrorMessage, ((Error<bool>)r).Exception.Message);
        }
    }
}
