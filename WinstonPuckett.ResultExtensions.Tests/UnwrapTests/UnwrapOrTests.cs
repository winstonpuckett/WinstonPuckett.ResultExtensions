using System;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.ResultExtensions.Tests.UnwrapTests
{
    public class UnwrapOrTests
    {
        private readonly IResult<bool> _error = new Error<bool>(new IndexOutOfRangeException());
        private readonly Task<IResult<bool>> _errorTask = Task.Run(() => (IResult<bool>)new Error<bool>(new IndexOutOfRangeException()));
        private readonly IResult<bool> _ok = new Ok<bool>(true);
        private readonly Task<IResult<bool>> _okTask = Task.Run(() => (IResult<bool>)new Ok<bool>(true));
        private readonly Task<IResult<bool>> _okTaskCancelled = Task.Run(() => (IResult<bool>)new Ok<bool>(true), new System.Threading.CancellationToken(true));

        private bool ToTrue(Exception e) => true;
        private async Task<bool> ToTrueAsync(Exception e) => await Task.Run(() => true);

        [Fact(DisplayName = "UnwrapOr throws exception on error.")]
        public void UnwrapThrowsExceptionOnError()
        {
            Assert.True(_error.UnwrapOr(ToTrue));
        }
        [Fact(DisplayName = "UnwrapOr throws exception on error async.")]
        public async Task UnwrapThrowsExceptionOnErrorAsync()
        {
            Assert.True(await _error.UnwrapOr(ToTrueAsync));
        }

        [Fact(DisplayName = "UnwrapOr throws exception on error task.")]
        public async Task UnwrapThrowsExceptionOnErrorTask()
        {
            Assert.True(await _errorTask.UnwrapOr(ToTrue));
        }
        [Fact(DisplayName = "UnwrapOr throws exception on error task async.")]
        public async Task UnwrapThrowsExceptionOnErrorTaskAsync()
        {
            Assert.True(await _errorTask.UnwrapOr(ToTrueAsync));
        }

        [Fact(DisplayName = "UnwrapOr unwraps for cancellation token.")]
        public async Task UnwrapOrUnwrapsForCancellationToken()
        {
            Assert.True(await _okTaskCancelled.UnwrapOr(ToTrue));
        }
        [Fact(DisplayName = "UnwrapOr unwraps for cancellation token async.")]
        public async Task UnwrapOrUnwrapsForCancellationTokenAsync()
        {
            Assert.True(await _okTaskCancelled.UnwrapOr(ToTrueAsync));
        }

        [Fact(DisplayName = "UnwrapOr unwraps on ok.")]
        public void UnwrapUnwrapsOnOk()
        {
            Assert.True(_ok.UnwrapOr(ToTrue));
        }
        [Fact(DisplayName = "UnwrapOr unwraps on ok async.")]
        public async Task UnwrapUnwrapsOnOkAsync()
        {
            Assert.True(await _ok.UnwrapOr(ToTrueAsync));
        }

        [Fact(DisplayName = "UnwrapOr unwraps on ok task.")]
        public async Task UnwrapUnwrapsOnOkTask()
        {
            Assert.True(await _okTask.UnwrapOr(ToTrue));
        }
        [Fact(DisplayName = "UnwrapOr unwraps on ok task.")]
        public async Task UnwrapUnwrapsOnOkTaskAsync()
        {
            Assert.True(await _okTask.UnwrapOr(ToTrueAsync));
        }
    }
}
