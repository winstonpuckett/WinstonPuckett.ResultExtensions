using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.ResultExtensions.Tests.TriadicTests
{
    public class TriadFunctionTests
    {
        // General data.
        private readonly static string _anyUniqueErrorMessage = "I am the initial error message.";

        // Input to bind.
        private readonly (bool, bool, bool) _false = (false, false, false);
        private readonly Task<(bool, bool, bool)> _taskFalse = Task.Run(() => (false, false, false));
        private readonly Task<(bool, bool, bool)> _taskCancelledFalse = Task.Run(() => (false, false, false), new System.Threading.CancellationToken(true));
        private readonly IResult<(bool, bool, bool)> _okFalse = new Ok<(bool, bool, bool)>((false, false, false));
        private readonly Task<IResult<(bool, bool, bool)>> _taskOkFalse = Task.Run(() => (IResult<(bool, bool, bool)>)new Ok<(bool, bool, bool)>((false, false, false)));
        private readonly Task<IResult<(bool, bool, bool)>> _taskCancelledOkFalse = Task.Run(() => (IResult<(bool, bool, bool)>)new Ok<(bool, bool, bool)>((false, false, false)), new System.Threading.CancellationToken(true));
        private readonly IResult<(bool, bool, bool)> _errorFalse = new Error<(bool, bool, bool)>(new Exception(_anyUniqueErrorMessage));
        private readonly Task<IResult<(bool, bool, bool)>> _taskErrorFalse = Task.Run(() => (IResult<(bool, bool, bool)>)new Error<(bool, bool, bool)>(new Exception(_anyUniqueErrorMessage)));
        private readonly Task<IResult<(bool, bool, bool)>> _taskCancelledErrorFalse = Task.Run(() => (IResult<(bool, bool, bool)>)new Error<bool>(new Exception(_anyUniqueErrorMessage)), new System.Threading.CancellationToken(true));

        // Functions to bind to
        private bool Flip(bool b, bool _, bool _2) => !b;
        private async Task<bool> FlipAsync(bool b, bool _, bool _2) => await Task.Run(() => !b);
        private bool ThrowNotImplementedException(bool _, bool _2, bool _3) { throw new NotImplementedException(); }
        private async Task<bool> ThrowNotImplementedExceptionAsync(bool _, bool _2, bool _3) { await Task.Run(() => throw new NotImplementedException()); return false; }


        // T -> U

        [Fact(DisplayName = "T transforms to U.")]
        public void TTransformsToU()
        {
            var r = _false.Bind(Flip);
            Assert.Equal(!_false.Item1, ((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "T transforms to U async.")]
        public async Task TTransformsToUAsync()
        {
            var r = await _false.Bind(FlipAsync);
            Assert.Equal(!_false.Item1, ((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "T to Exception carries what is thrown.")]
        public void TToExceptionCarriesWhatIsThrown()
        {
            var r = _false.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "T to Exception carries what is thrown async.")]
        public async Task TToExceptionCarriesWhatIsThrownAsync()
        {
            var r = await _false.Bind(ThrowNotImplementedExceptionAsync);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        // T Task -> U

        [Fact(DisplayName = "T Task transforms to U.")]
        public async Task TTaskTransformsToU()
        {
            var r = await _taskFalse.Bind(Flip);
            Assert.Equal(!(await _taskFalse).Item1, ((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "T Task transforms to U async.")]
        public async Task TTaskTransformsToUAsync()
        {
            var r = await _taskFalse.Bind(FlipAsync);
            Assert.Equal(!(await _taskFalse).Item1, ((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "T Task honors cancellation token.")]
        public async Task TTaskHonorsCancellationToken()
        {
            var r = await _taskCancelledFalse.Bind(Flip);
            Assert.True(((Error<bool>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "T Task honors cancellation token async.")]
        public async Task TTaskHonorsCancellationTokenAsync()
        {
            var r = await _taskCancelledFalse.Bind(FlipAsync);
            Assert.True(((Error<bool>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "T Task to Exception carries what is thrown.")]
        public async Task TTaskToExceptionCarriesWhatIsThrown()
        {
            var r = await _taskFalse.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "T Task to Exception carries what is thrown async.")]
        public async Task TTaskToExceptionCarriesWhatIsThrownAsync()
        {
            var r = await _taskFalse.Bind(ThrowNotImplementedExceptionAsync);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        // Ok -> U

        [Fact(DisplayName = "Ok transforms to U.")]
        public void OkTransformsToU()
        {
            var r = _okFalse.Bind(Flip);
            Assert.Equal(!((Ok<(bool, bool, bool)>)_okFalse).Value.Item1, ((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "Ok transforms to U async.")]
        public async Task OkTransformsToUAsync()
        {
            var r = await _okFalse.Bind(FlipAsync);
            Assert.Equal(!((Ok<(bool, bool, bool)>)_okFalse).Value.Item1, ((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "Ok to Exception carries what is thrown.")]
        public void OkToExceptionCarriesWhatIsThrown()
        {
            var r = _okFalse.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Ok to Exception carries what is thrown async.")]
        public async Task OkToExceptionCarriesWhatIsThrownAsync()
        {
            var r = await _okFalse.Bind(ThrowNotImplementedExceptionAsync);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        // Ok Task -> U

        [Fact(DisplayName = "Ok Task transforms to U.")]
        public async Task OkTaskTransformsToU()
        {
            var r = await _taskOkFalse.Bind(Flip);
            Assert.Equal(!((Ok<(bool, bool, bool)>)await _taskOkFalse).Value.Item1, ((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "Ok Task transforms to U async.")]
        public async Task OkTaskTransformsToUAsync()
        {
            var r = await _taskOkFalse.Bind(FlipAsync);
            Assert.Equal(!((Ok<(bool, bool, bool)>)await _taskOkFalse).Value.Item1, ((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "Ok Task honors cancellation token.")]
        public async Task OkTaskHonorsCancellationToken()
        {
            var r = await _taskCancelledOkFalse.Bind(Flip);
            Assert.True(((Error<bool>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "Ok Task honors cancellation token async.")]
        public async Task OkTaskHonorsCancellationTokenAsync()
        {
            var r = await _taskCancelledOkFalse.Bind(FlipAsync);
            Assert.True(((Error<bool>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "Ok Task to Exception carries what is thrown.")]
        public async Task OkTaskToExceptionCarriesWhatIsThrown()
        {
            var r = await _taskOkFalse.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Ok Task to Exception carries what is thrown async.")]
        public async Task OkTaskToExceptionCarriesWhatIsThrownAsync()
        {
            var r = await _taskOkFalse.Bind(ThrowNotImplementedExceptionAsync);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        // Error -> U

        [Fact(DisplayName = "Error does not bind.")]
        public void ErrorDoesNotBind()
        {
            var r = _errorFalse.Bind(ThrowNotImplementedException);
            Assert.False(((Error<bool>)r).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Error does not bind async.")]
        public async Task ErrorDoesNotBindAsync()
        {
            var r = await _errorFalse.Bind(ThrowNotImplementedExceptionAsync);
            Assert.False(((Error<bool>)r).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Error retains original message.")]
        public void ErrorContainsOriginalError()
        {
            var r = _errorFalse.Bind(ThrowNotImplementedException);
            Assert.Equal(_anyUniqueErrorMessage, ((Error<bool>)r).Exception.Message);
        }

        [Fact(DisplayName = "Error retains original message async.")]
        public async Task ErrorContainsOriginalErrorAsync()
        {
            var r = await _errorFalse.Bind(ThrowNotImplementedExceptionAsync);
            Assert.Equal(_anyUniqueErrorMessage, ((Error<bool>)r).Exception.Message);
        }

        // Error Task -> U

        [Fact(DisplayName = "Error Task honors cancellation token.")]
        public async Task ErrorTaskHonorsCancellationToken()
        {
            var r = await _taskCancelledErrorFalse.Bind(Flip);
            Assert.True(((Error<bool>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "Error Task honors cancellation token async.")]
        public async Task ErrorTaskHonorsCancellationTokenAsync()
        {
            var r = await _taskCancelledErrorFalse.Bind(FlipAsync);
            Assert.True(((Error<bool>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "Error Task does not bind.")]
        public async Task ErrorTaskDoesNotBind()
        {
            var r = await _taskErrorFalse.Bind(ThrowNotImplementedException);
            Assert.False(((Error<bool>)r).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Error Task does not bind async.")]
        public async Task ErrorTaskDoesNotBindAsync()
        {
            var r = await _taskErrorFalse.Bind(ThrowNotImplementedExceptionAsync);
            Assert.False(((Error<bool>)r).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Error Task retains original message.")]
        public async Task ErrorTaskRetainsOriginalMessage()
        {
            var r = await _taskErrorFalse.Bind(ThrowNotImplementedException);
            Assert.Equal(_anyUniqueErrorMessage, ((Error<bool>)r).Exception.Message);
        }

        [Fact(DisplayName = "Error Task retains original message async.")]
        public async Task ErrorTaskRetainsOriginalMessageAsync()
        {
            var r = await _taskErrorFalse.Bind(ThrowNotImplementedExceptionAsync);
            Assert.Equal(_anyUniqueErrorMessage, ((Error<bool>)r).Exception.Message);
        }
    }
}
