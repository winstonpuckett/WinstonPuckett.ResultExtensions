using Xunit;
using System;
using WinstonPuckett.ResultExtensions;
using System.Threading.Tasks;

namespace Monads.Actions.Tests
{
    public class TaskErrorTAction_HappyPath_Tests
    {
        private readonly static string _initialErrorMessage = "I am the initial error message.";
        private Task<IResult<bool>> _startingProperty = Task.Run(() => (IResult<bool>)new Error<bool>(new Exception(_initialErrorMessage)));
        private void DoNothing(bool _) { }
        private void ThrowNotImplementedException(bool _) { throw new NotImplementedException(); }

        [Fact(DisplayName = "IResult is Error<T>.")]
        public async Task ReturnsError()
        {
            var r = await _startingProperty.Bind(DoNothing);
            Assert.True(r is Error<bool>);
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
