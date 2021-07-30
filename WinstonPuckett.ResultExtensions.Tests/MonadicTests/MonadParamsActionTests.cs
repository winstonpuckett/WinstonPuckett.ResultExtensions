﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WinstonPuckett.ResultExtensions.Tests.MonadicTests
{
    public class MonadParamsActionTests
    {
        private readonly static string _anyUniqueErrorMessage = "I am the initial error message.";

        // Input to bind.
        private readonly bool _false = false;
        private readonly Task<bool> _taskFalse = Task.Run(() => false);
        private readonly Task<bool> _taskCancelledFalse = Task.Run(() => false, new System.Threading.CancellationToken(true));
        private readonly IResult<bool> _okFalse = new Ok<bool>(false);
        private readonly Task<IResult<bool>> _taskOkFalse = Task.Run(() => (IResult<bool>)new Ok<bool>(false));
        private readonly Task<IResult<bool>> _taskCancelledOkFalse = Task.Run(() => (IResult<bool>)new Ok<bool>(false), new System.Threading.CancellationToken(true));
        private readonly IResult<bool> _errorFalse = new Error<bool>(new Exception(_anyUniqueErrorMessage));
        private readonly Task<IResult<bool>> _taskErrorFalse = Task.Run(() => (IResult<bool>)new Error<bool>(new Exception(_anyUniqueErrorMessage)));
        private readonly Task<IResult<bool>> _taskCancelledErrorFalse = Task.Run(() => (IResult<bool>)new Error<bool>(new Exception(_anyUniqueErrorMessage)), new System.Threading.CancellationToken(true));

        // Functions to bind on.
        private void EmitNothing(bool _) { }
        private async Task EmitNothingAsync(bool _) { await Task.Run(() => { }); }
        private void EmitNotImplementedException(bool _) { throw new NotImplementedException(); }
        private async Task EmitNotImplementedExceptionAsync(bool _) { await Task.Run(() => throw new NotImplementedException()); }

        // T -> Emit

        [Fact(DisplayName = "T holds original value.")]
        public void THoldsOriginalValue()
        {
            var r = _false.Bind(EmitNothing, EmitNothing);
            Assert.False(((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "T holds original value async.")]
        public async Task THoldsOriginalValueAsync()
        {
            var r = await _false.Bind(EmitNothingAsync, EmitNothingAsync);
            Assert.False(((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "T to Exception carries what is thrown.")]
        public void TToExceptionCarriesWhatIsThrown()
        {
            var r = _false.Bind(EmitNothing, EmitNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "T to Exception carries what is thrown async.")]
        public async Task TToExceptionCarriesWhatIsThrownAsync()
        {
            var r = await _false.Bind(EmitNotImplementedExceptionAsync);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        // T Task -> Emit

        [Fact(DisplayName = "T Task holds original value.")]
        public async Task TTaskHoldsOriginalValue()
        {
            var r = await _taskFalse.Bind(EmitNothing, EmitNothing);
            Assert.False(((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "T Task holds original value async.")]
        public async Task TTaskHoldsOriginalValueAsync()
        {
            var r = await _taskFalse.Bind(EmitNothingAsync, EmitNothingAsync);
            Assert.False(((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "T Task to Exception carries what is thrown.")]
        public async Task TTaskToExceptionCarriesWhatIsThrown()
        {
            var r = await _taskFalse.Bind(EmitNothing, EmitNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "T Task to Exception carries what is thrown async.")]
        public async Task TTaskToExceptionCarriesWhatIsThrownAsync()
        {
            var r = await _taskFalse.Bind(EmitNothingAsync, EmitNotImplementedExceptionAsync);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "T Task honors cancellation token.")]
        public async Task TTaskHonorsCancellationToken()
        {
            var r = await _taskCancelledFalse.Bind(EmitNothing, EmitNothing);
            Assert.True(((Error<bool>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "T Task honors cancellation token async.")]
        public async Task TTaskHonorsCancellationTokenAsync()
        {
            var r = await _taskCancelledFalse.Bind(EmitNothingAsync, EmitNothingAsync);
            Assert.True(((Error<bool>)r).Exception is TaskCanceledException);
        }

        // Ok -> Emit

        [Fact(DisplayName = "Ok holds original value.")]
        public void OkHoldsOriginalValue()
        {
            var r = _okFalse.Bind(EmitNothing, EmitNothing);
            Assert.False(((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "Ok holds original value async.")]
        public async Task OkHoldsOriginalValueAsync()
        {
            var r = await _okFalse.Bind(EmitNothingAsync, EmitNothingAsync);
            Assert.False(((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "Ok to Exception carries what is thrown.")]
        public void OkToExceptionCarriesWhatIsThrown()
        {
            var r = _okFalse.Bind(EmitNothing, EmitNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Ok to Exception carries what is thrown async.")]
        public async Task OkToExceptionCarriesWhatIsThrownAsync()
        {
            var r = await _okFalse.Bind(EmitNothingAsync, EmitNotImplementedExceptionAsync);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        // Ok Task -> Emit

        [Fact(DisplayName = "Ok Task holds original value.")]
        public async Task OkTaskHoldsOriginalValue()
        {
            var r = await _taskOkFalse.Bind(EmitNothing, EmitNothing);
            Assert.False(((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "Ok Task holds original value async.")]
        public async Task OkTaskHoldsOriginalValueAsync()
        {
            var r = await _taskOkFalse.Bind(EmitNothingAsync, EmitNothingAsync);
            Assert.False(((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "Ok Task honors cancellation token.")]
        public async Task OkTaskHonorsCancellationToken()
        {
            var r = await _taskCancelledOkFalse.Bind(EmitNothing, EmitNothing);
            Assert.True(((Error<bool>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "Ok Task honors cancellation token async.")]
        public async Task OkTaskHonorsCancellationTokenAsync()
        {
            var r = await _taskCancelledOkFalse.Bind(EmitNothingAsync, EmitNothingAsync);
            Assert.True(((Error<bool>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "Ok Task to Exception carries what is thrown.")]
        public async Task OkTaskToExceptionCarriesWhatIsThrown()
        {
            var r = await _taskOkFalse.Bind(EmitNothing, EmitNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Ok Task to Exception carries what is thrown async.")]
        public async Task OkTaskToExceptionCarriesWhatIsThrownAsync()
        {
            var r = await _taskOkFalse.Bind(EmitNothingAsync, EmitNotImplementedExceptionAsync);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }

        // Error -> Emit

        [Fact(DisplayName = "Error does not bind.")]
        public void ErrorDoesNotCallSubsequentMethod()
        {
            var r = _errorFalse.Bind(EmitNothing, EmitNotImplementedException);
            Assert.False(((Error<bool>)r).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Error retains original message.")]
        public void ErrorContainsOriginalError()
        {
            var r = _errorFalse.Bind(EmitNothing, EmitNotImplementedException);
            Assert.Equal(_anyUniqueErrorMessage, ((Error<bool>)r).Exception.Message);
        }

        [Fact(DisplayName = "Error does not bind async.")]
        public async Task DoesNotContainNewErrorAsync()
        {
            var r = await _errorFalse.Bind(EmitNothingAsync, EmitNotImplementedExceptionAsync);
            Assert.False(((Error<bool>)r).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Error retains original message async.")]
        public async Task ErrorContainsOriginalErrorAsync()
        {
            var r = await _errorFalse.Bind(EmitNothingAsync, EmitNotImplementedExceptionAsync);
            Assert.Equal(_anyUniqueErrorMessage, ((Error<bool>)r).Exception.Message);
        }

        // Error Task -> Emit

        [Fact(DisplayName = "Error Task honors cancellation token.")]
        public async Task ErrorTaskHonorsCancellationToken()
        {
            var r = await _taskCancelledErrorFalse.Bind(EmitNothing, EmitNothing);
            Assert.True(((Error<bool>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "Error Task honors cancellation token async.")]
        public async Task ErrorTaskHonorsCancellationTokenAsync()
        {
            var r = await _taskCancelledErrorFalse.Bind(EmitNothingAsync, EmitNothingAsync);
            Assert.True(((Error<bool>)r).Exception is TaskCanceledException);
        }

        [Fact(DisplayName = "Error Task does not bind.")]
        public async Task ErrorTaskDoesNotBind()
        {
            var r = await _taskErrorFalse.Bind(EmitNothing, EmitNotImplementedException);
            Assert.False(((Error<bool>)r).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Error Task does not bind async.")]
        public async Task ErrorTaskDoesNotBindAsync()
        {
            var r = await _taskErrorFalse.Bind(EmitNothingAsync, EmitNotImplementedExceptionAsync);
            Assert.False(((Error<bool>)r).Exception is NotImplementedException);
        }

        [Fact(DisplayName = "Error Task retains original message.")]
        public async Task ErrorTaskRetainsOriginalMessage()
        {
            var r = await _taskErrorFalse.Bind(EmitNothing, EmitNotImplementedException);
            Assert.Equal(_anyUniqueErrorMessage, ((Error<bool>)r).Exception.Message);
        }

        [Fact(DisplayName = "Error Task retains original message async.")]
        public async Task ErrorTaskRetainsOriginalMessageAsync()
        {
            var r = await _taskErrorFalse.Bind(EmitNothingAsync, EmitNotImplementedExceptionAsync);
            Assert.Equal(_anyUniqueErrorMessage, ((Error<bool>)r).Exception.Message);
        }
    }
}
