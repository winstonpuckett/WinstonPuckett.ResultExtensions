using System;
using System.Threading.Tasks;

namespace WinstonPuckett.ResultExtensions
{
    internal static class Common
    {
        internal static IResult<T> Emit<T>(this T input, Action<T> function)
        {
            function(input);
            return new Ok<T>(input);
        }

        internal static IResult<T> OkOrError<T>(this T input, Action<T> function)
        {
            try
            {
                return input.Emit(function);
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }
        }
        internal static IResult<U> OkOrError<T, U>(this T input, Func<T, U> function)
        {
            try
            {
                return new Ok<U>(function(input));
            }
            catch (Exception e)
            {
                return new Error<U>(e);
            }
        }
        internal static async Task<IResult<T>> OkOrErrorAsync<T>(this T input, Func<T, Task> function)
        {
            try
            {
                await function(input);
                return new Ok<T>(input);
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }
        }

        internal static IResult<T> UnwrapThenOkOrError<T>(this IResult<T> input, Action<T> function)
        {
            switch (input)
            {
                case Ok<T> ok:
                    return ok.Value.OkOrError(function);
                case Error<T> error:
                    return error;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }
        internal static IResult<U> UnwrapThenOkOrError<T, U>(this IResult<T> input, Func<T, U> function)
        {
            switch (input)
            {
                case Ok<T> ok:
                    return ok.Value.OkOrError(function);
                case Error<T> error:
                    return new Error<U>(error.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }
    }
}
