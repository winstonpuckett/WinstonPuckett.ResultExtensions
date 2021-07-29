﻿using System;
using System.Threading.Tasks;
using WinstonPuckett.ResultExtensions;
using Xunit;

namespace Monads.Functions.Tests
{
    public class TaskOkTFuncTTaskU_HappyPath_Tests
    {
        private Task<IResult<bool>> _startingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false));
        private Task<IResult<bool>> _cancelledStartingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false), new System.Threading.CancellationToken(true));
        private async Task<bool> Flip(bool b) => await Task.Run(() => !b);


        [Fact(DisplayName = "Value returns flip of value.")]
        public async Task ReturnsFlippedValue()
        {
            var r = await _startingProperty.Bind(Flip);
            Assert.Equal(!((Ok<bool>)await _startingProperty).Value, ((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "Cancelled token doesn't throw exception.")]
        public async Task CancelledTokenThrowsNoException()
        {
            await _cancelledStartingProperty.Bind(Flip);
        }
    }

    public class TaskOkTFuncTTaskU_SadPath_Tests
    {
        private Task<IResult<bool>> _startingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false));
        private Task<IResult<bool>> _cancelledStartingProperty => Task.Run(() => (IResult<bool>)new Ok<bool>(false), new System.Threading.CancellationToken(true));
        private async Task<bool> ThrowGeneralException(bool _) { await Task.Run(() => throw new Exception()); return false; }
        private async Task<bool> ThrowNotImplementedException(bool _) { await Task.Run(() => throw new NotImplementedException()); return false; }

        [Fact(DisplayName = "Cancelled token doesn't throw exception.")]
        public async Task CancelledTokenThrowsNoException()
        {
            await _cancelledStartingProperty.Bind(ThrowGeneralException);
        }

        [Fact(DisplayName = "Error holds exception.")]
        public async Task ErrorHoldsException()
        {
            var r = await _startingProperty.Bind(ThrowNotImplementedException);
            Assert.True((r as Error<bool>).Exception is NotImplementedException);
        }
    }
}
