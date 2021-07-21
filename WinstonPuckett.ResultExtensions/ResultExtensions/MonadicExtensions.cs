using System;
using System.Threading.Tasks;

namespace WinstonPuckett.ResultExtensions
{
    public static partial class ResultExtensions
    {
        // Action Synchronous

        public static IResult<T> Bind<T>(this T input, Action<T> function)
        {
            try
            {
                function(input);
                return new Ok<T>(input);
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }
        }

        public static IResult<T> Bind<T>(this IResult<T> input, Action<T> function)
        {
            switch (input)
            {
                case Ok<T> ok:
                    return ok.Value.Bind(function);
                case Error<T> error:
                    return error;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        // Action Asynchronous

        public static async Task<IResult<T>> Bind<T>(this Task<T> input, Action<T> function)
        {
            try
            {
                var i = await input;
                function(i);
                return new Ok<T>(i);
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }
        }

        public static async Task<IResult<T>> Bind<T>(this T input, Func<T, Task> function)
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

        public static async Task<IResult<T>> Bind<T>(this Task<T> input, Func<T, Task> function)
        {
            try
            {
                var i = await input;
                await function(i);
                return new Ok<T>(i);
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }
        }

        public static async Task<IResult<T>> Bind<T>(this Task<IResult<T>> input, Action<T> function)
        {
            var i = await input;
            return i.Bind(function);
        }

        public static async Task<IResult<T>> Bind<T>(this IResult<T> input, Func<T, Task> function)
        {
            switch (input)
            {
                case Ok<T> ok:
                    return await ok.Value.Bind(function);
                case Error<T> error:
                    return error;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }
        public static async Task<IResult<T>> Bind<T>(this Task<IResult<T>> input, Func<T, Task> function)
        {
            var i = await input;
            return await i.Bind(function);
        }

        // Function Synchronous

        public static IResult<U> Bind<T, U>(this T input, Func<T, U> function)
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

        public static IResult<U> Bind<T, U>(this IResult<T> input, Func<T, U> function)
        {
            switch (input)
            {
                case Ok<T> ok:
                    return ok.Value.Bind(function);
                case Error<T> error:
                    return new Error<U>(error.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        // Function Asynchronous

        public static async Task<IResult<U>> Bind<T, U>(this Task<T> input, Func<T, U> function)
        {
            try
            {
                var i = await input;
                return new Ok<U>(function(i));
            }
            catch (Exception e)
            {
                return new Error<U>(e);
            }
        }

        public static async Task<IResult<U>> Bind<T, U>(this Task<IResult<T>> input, Func<T, U> function)
        {
            try
            {
                var i = await input;
                return i.Bind(function);
            }
            catch (Exception e)
            {
                return new Error<U>(e);
            }
        }
    }
}
