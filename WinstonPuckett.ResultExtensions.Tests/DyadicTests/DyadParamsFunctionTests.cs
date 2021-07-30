using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.ResultExtensions.Tests.DyadicTests
{
    public class DyadParamsFunctionTests
    {
        // General data.
        private readonly static string _anyUniqueErrorMessage = "I am the initial error message.";

        // Input to bind.
        private readonly (bool, bool) _false = (false, false);
        private readonly Task<(bool, bool)> _taskFalse = Task.Run(() => (false, false));
        private readonly Task<(bool, bool)> _taskCancelledFalse = Task.Run(() => (false, false), new System.Threading.CancellationToken(true));
        private readonly IResult<(bool, bool)> _okFalse = new Ok<(bool, bool)>((false, false));
        private readonly Task<IResult<(bool, bool)>> _taskOkFalse = Task.Run(() => (IResult<(bool, bool)>)new Ok<(bool, bool)>((false, false)));
        private readonly Task<IResult<(bool, bool)>> _taskCancelledOkFalse = Task.Run(() => (IResult<(bool, bool)>)new Ok<(bool, bool)>((false, false)), new System.Threading.CancellationToken(true));
        private readonly IResult<(bool, bool)> _errorFalse = new Error<(bool, bool)>(new Exception(_anyUniqueErrorMessage));
        private readonly Task<IResult<(bool, bool)>> _taskErrorFalse = Task.Run(() => (IResult<(bool, bool)>)new Error<(bool, bool)>(new Exception(_anyUniqueErrorMessage)));
        private readonly Task<IResult<(bool, bool)>> _taskCancelledErrorFalse = Task.Run(() => (IResult<(bool, bool)>)new Error<bool>(new Exception(_anyUniqueErrorMessage)), new System.Threading.CancellationToken(true));

        // Functions to bind to
        private bool Flip(bool b, bool _) => !b;
        private async Task<bool> FlipAsync(bool b, bool _) => await Task.Run(() => !b);
        private bool ThrowNotImplementedException(bool _, bool _2) { throw new NotImplementedException(); }
        private async Task<bool> ThrowNotImplementedExceptionAsync(bool _, bool _2) { await Task.Run(() => throw new NotImplementedException()); return false; }


        // T -> U

        [Fact(DisplayName = "T transforms to U.")]
        public void TTransformsToU()
        {
            var r = _false.Bind(Flip, Flip);
            Assert.True(((Ok<IEnumerable<bool>>)r).Value.All(b => b == !_false.Item1));
        }

        [Fact(DisplayName = "T transforms to U async.")]
        public async Task TTransformsToUAsync()
        {
            var r = await _false.Bind(FlipAsync, FlipAsync);
            Assert.True(((Ok<IEnumerable<bool>>)r).Value.All(b => b == !_false.Item1));
        }

        [Fact(DisplayName = "T to Exception carries what is thrown.")]
        public void TToExceptionCarriesWhatIsThrown()
        {
            var r = _false.Bind(Flip, ThrowNotImplementedException);
            Assert.True((r as Error<IEnumerable<bool>>).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "T to Exception carries what is thrown async.")]
        public async Task TToExceptionCarriesWhatIsThrownAsync()
        {
            var r = await _false.Bind(FlipAsync, ThrowNotImplementedExceptionAsync);
            Assert.True((r as Error<IEnumerable<bool>>).Exception is NotImplementedException);
        }

        // T Task -> U

        [Fact(DisplayName = "T Task transforms to U.")]
        public async Task TTaskTransformsToU()
        {
            var r = await _taskFalse.Bind(Flip, Flip);
            Assert.True(((Ok<IEnumerable<bool>>)r).Value.All(b => b == !_false.Item1));
        }

        [Fact(DisplayName = "T Task transforms to U async.")]
        public async Task TTaskTransformsToUAsync()
        {
            var r = await _taskFalse.Bind(FlipAsync, FlipAsync);
            Assert.True(((Ok<IEnumerable<bool>>)r).Value.All(b => b == !_false.Item1));
        }

        [Fact(DisplayName = "T Task honors cancellation token.")]
        public async Task TTaskHonorsCancellationToken()
        {
            var r = await _taskCancelledFalse.Bind(Flip, Flip);
            Assert.True(((Error<IEnumerable<bool>>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "T Task honors cancellation token async.")]
        public async Task TTaskHonorsCancellationTokenAsync()
        {
            var r = await _taskCancelledFalse.Bind(FlipAsync, FlipAsync);
            Assert.True(((Error<IEnumerable<bool>>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "T Task to Exception carries what is thrown.")]
        public async Task TTaskToExceptionCarriesWhatIsThrown()
        {
            var r = await _taskFalse.Bind(Flip, ThrowNotImplementedException);
            Assert.True((r as Error<IEnumerable<bool>>).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "T Task to Exception carries what is thrown async.")]
        public async Task TTaskToExceptionCarriesWhatIsThrownAsync()
        {
            var r = await _taskFalse.Bind(FlipAsync, ThrowNotImplementedExceptionAsync);
            Assert.True((r as Error<IEnumerable<bool>>).Exception is NotImplementedException);
        }

        // Ok -> U

        [Fact(DisplayName = "Ok transforms to U.")]
        public void OkTransformsToU()
        {
            var r = _okFalse.Bind(Flip, Flip);
            Assert.True(((Ok<IEnumerable<bool>>)r).Value.All(b => b == !_false.Item1));
        }

        [Fact(DisplayName = "Ok transforms to U async.")]
        public async Task OkTransformsToUAsync()
        {
            var r = await _okFalse.Bind(FlipAsync, FlipAsync);
            Assert.True(((Ok<IEnumerable<bool>>)r).Value.All(b => b == !_false.Item1));
        }

        [Fact(DisplayName = "Ok to Exception carries what is thrown.")]
        public void OkToExceptionCarriesWhatIsThrown()
        {
            var r = _okFalse.Bind(Flip, ThrowNotImplementedException);
            Assert.True((r as Error<IEnumerable<bool>>).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Ok to Exception carries what is thrown async.")]
        public async Task OkToExceptionCarriesWhatIsThrownAsync()
        {
            var r = await _okFalse.Bind(FlipAsync, ThrowNotImplementedExceptionAsync);
            Assert.True((r as Error<IEnumerable<bool>>).Exception is NotImplementedException);
        }

        // Ok Task -> U

        [Fact(DisplayName = "Ok Task transforms to U.")]
        public async Task OkTaskTransformsToU()
        {
            var r = await _taskOkFalse.Bind(Flip, Flip);
            Assert.True(((Ok<IEnumerable<bool>>)r).Value.All(b => b == !_false.Item1));
        }

        [Fact(DisplayName = "Ok Task transforms to U async.")]
        public async Task OkTaskTransformsToUAsync()
        {
            var r = await _taskOkFalse.Bind(FlipAsync, FlipAsync);
            Assert.True(((Ok<IEnumerable<bool>>)r).Value.All(b => b == !_false.Item1));
        }

        [Fact(DisplayName = "Ok Task honors cancellation token.")]
        public async Task OkTaskHonorsCancellationToken()
        {
            var r = await _taskCancelledOkFalse.Bind(Flip, Flip);
            Assert.True(((Error<IEnumerable<bool>>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "Ok Task honors cancellation token async.")]
        public async Task OkTaskHonorsCancellationTokenAsync()
        {
            var r = await _taskCancelledOkFalse.Bind(FlipAsync, FlipAsync);
            Assert.True(((Error<IEnumerable<bool>>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "Ok Task to Exception carries what is thrown.")]
        public async Task OkTaskToExceptionCarriesWhatIsThrown()
        {
            var r = await _taskOkFalse.Bind(Flip, ThrowNotImplementedException);
            Assert.True((r as Error<IEnumerable<bool>>).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Ok Task to Exception carries what is thrown async.")]
        public async Task OkTaskToExceptionCarriesWhatIsThrownAsync()
        {
            var r = await _taskOkFalse.Bind(FlipAsync, ThrowNotImplementedExceptionAsync);
            Assert.True((r as Error<IEnumerable<bool>>).Exception is NotImplementedException);
        }

        // Error -> U

        [Fact(DisplayName = "Error does not bind.")]
        public void ErrorDoesNotBind()
        {
            var r = _errorFalse.Bind(Flip, ThrowNotImplementedException);
            Assert.False(((Error<IEnumerable<bool>>)r).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Error does not bind async.")]
        public async Task ErrorDoesNotBindAsync()
        {
            var r = await _errorFalse.Bind(FlipAsync, ThrowNotImplementedExceptionAsync);
            Assert.False(((Error<IEnumerable<bool>>)r).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Error retains original message.")]
        public void ErrorContainsOriginalError()
        {
            var r = _errorFalse.Bind(Flip, ThrowNotImplementedException);
            Assert.Equal(_anyUniqueErrorMessage, ((Error<IEnumerable<bool>>)r).Exception.Message);
        }

        [Fact(DisplayName = "Error retains original message async.")]
        public async Task ErrorContainsOriginalErrorAsync()
        {
            var r = await _errorFalse.Bind(FlipAsync, ThrowNotImplementedExceptionAsync);
            Assert.Equal(_anyUniqueErrorMessage, ((Error<IEnumerable<bool>>)r).Exception.Message);
        }

        // Error Task -> U

        [Fact(DisplayName = "Error Task honors cancellation token.")]
        public async Task ErrorTaskHonorsCancellationToken()
        {
            var r = await _taskCancelledErrorFalse.Bind(Flip, Flip);
            Assert.True(((Error<IEnumerable<bool>>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "Error Task honors cancellation token async.")]
        public async Task ErrorTaskHonorsCancellationTokenAsync()
        {
            var r = await _taskCancelledErrorFalse.Bind(FlipAsync, FlipAsync);
            Assert.True(((Error<IEnumerable<bool>>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "Error Task does not bind.")]
        public async Task ErrorTaskDoesNotBind()
        {
            var r = await _taskErrorFalse.Bind(Flip, ThrowNotImplementedException);
            Assert.False(((Error<IEnumerable<bool>>)r).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Error Task does not bind async.")]
        public async Task ErrorTaskDoesNotBindAsync()
        {
            var r = await _taskErrorFalse.Bind(FlipAsync, ThrowNotImplementedExceptionAsync);
            Assert.False(((Error<IEnumerable<bool>>)r).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Error Task retains original message.")]
        public async Task ErrorTaskRetainsOriginalMessage()
        {
            var r = await _taskErrorFalse.Bind(Flip, ThrowNotImplementedException);
            Assert.Equal(_anyUniqueErrorMessage, ((Error<IEnumerable<bool>>)r).Exception.Message);
        }

        [Fact(DisplayName = "Error Task retains original message async.")]
        public async Task ErrorTaskRetainsOriginalMessageAsync()
        {
            var r = await _taskErrorFalse.Bind(FlipAsync, ThrowNotImplementedExceptionAsync);
            Assert.Equal(_anyUniqueErrorMessage, ((Error<IEnumerable<bool>>)r).Exception.Message);
        }
    }
}
