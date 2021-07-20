using System;

namespace WinstonPuckett.ResultExtensions
{
    public interface IResult<T> { }

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
        public Exception Exception { get; }

        public Error(Exception e)
        {
            Exception = e;
        }
    }
}
