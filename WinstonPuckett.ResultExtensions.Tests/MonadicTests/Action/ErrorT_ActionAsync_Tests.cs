using Xunit;
using System;
using WinstonPuckett.ResultExtensions;
using System.Threading.Tasks;

namespace Monads.Actions.Tests
{
    public class ErrorTActionAsync_HappyPath_Tests
    {
        private readonly static string _initialErrorMessage = "I am the initial error message.";
        private readonly IResult<bool> _startingProperty = new Error<bool>(new Exception(_initialErrorMessage));
        private async Task ThrowNotImplementedException(bool _) { await Task.Run(() => throw new NotImplementedException()); }

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
