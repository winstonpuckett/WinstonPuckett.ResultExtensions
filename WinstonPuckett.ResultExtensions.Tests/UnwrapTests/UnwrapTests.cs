using System;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.ResultExtensions.Tests.UnwrapTests
{
    public class UnwrapTests
    {
        private readonly IResult<bool> _error = new Error<bool>(new IndexOutOfRangeException());
        private readonly Task<IResult<bool>> _errorTask = Task.Run(() => (IResult<bool>)new Error<bool>(new IndexOutOfRangeException()));
        private readonly IResult<bool> _ok = new Ok<bool>(true);
        private readonly Task<IResult<bool>> _okTask = Task.Run(() => (IResult<bool>)new Ok<bool>(true));
        private readonly Task<IResult<bool>> _okTaskCancelled = Task.Run(() => (IResult<bool>)new Ok<bool>(true), new System.Threading.CancellationToken(true));

        [Fact(DisplayName = "Unwrap throws exception on error.")]
        public void UnwrapThrowsExceptionOnError()
        {
            Assert.Throws<IndexOutOfRangeException>(() => _error.Unwrap());
        }

        [Fact(DisplayName = "Unwrap throws exception on error task.")]
        public async Task UnwrapThrowsExceptionOnErrorAsync()
        {
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _errorTask.Unwrap());
        }

        [Fact(DisplayName = "Unwrap honors cancellation token.")]
        public async Task UnwrapHonorsCancellationToken()
        {
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await _okTaskCancelled.Unwrap());
        }

        [Fact(DisplayName = "Unwrap unwraps on ok.")]
        public void UnwrapUnwrapsOnOk()
        {
            Assert.True(_ok.Unwrap());
        }

        [Fact(DisplayName = "Unwrap unwraps on ok async.")]
        public async Task UnwrapUnwrapsOnOkAsync()
        {
            Assert.True(await _okTask.Unwrap());
        }
    }
}
