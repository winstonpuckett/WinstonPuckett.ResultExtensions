using System;
using System.Threading.Tasks;
using Xunit;
using WinstonPuckett.ResultExtensions;

namespace Monads.Functions.Tests
{
    public class TTaskFuncTU_HappyPath_Tests
    {
        private readonly Task<bool> _startingProperty = Task.Run(() => false);
        private Task<bool> _cancelledStartingProperty => Task.Run(() => false, new System.Threading.CancellationToken(true));
        private bool Flip(bool b) => !b;


        [Fact(DisplayName = "Value returns flip of value.")]
        public async Task ReturnsFlippedValue()
        {
            var r = await _startingProperty.Bind(Flip);
            Assert.Equal(!await _startingProperty, ((Ok<bool>)r).Value);
        }

        [Fact(DisplayName = "Cancelled token doesn't throw exception.")]
        public async Task CancelledTokenThrowsNoException()
        {
            await _cancelledStartingProperty.Bind(Flip);
        }
    }

    public class TTaskFuncTU_SadPath_Tests
    {
        private readonly Task<bool> _startingProperty = Task.Run(() => false);
        private Task<bool> _cancelledStartingProperty => Task.Run(() => false, new System.Threading.CancellationToken(true));
        private bool ThrowGeneralException(bool _) { throw new Exception(); }
        private bool ThrowNotImplementedException(bool _) { throw new NotImplementedException(); }

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
