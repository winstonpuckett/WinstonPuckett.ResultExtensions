using System;

namespace WinstonPuckett.ResultExtensions
{
    public interface IResult<T> 
    {
        T Value { get; }
    }
    public class Ok<T> : IResult<T>
    {
        public T Value { get; }

        public Ok(T value)
        {
            Value = value;
        }
    }
    public class Error<T> : IResult<T>
    {
        public T Value { get; }
        public Exception Exception { get; }

        public Error(T value, Exception e)
        {
            Value = value;
            Exception = e;
        }
    }
}
