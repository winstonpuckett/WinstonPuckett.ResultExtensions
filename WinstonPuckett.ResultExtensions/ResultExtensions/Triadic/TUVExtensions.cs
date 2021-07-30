using System;
using System.Threading.Tasks;

namespace WinstonPuckett.ResultExtensions
{
    public static partial class ResultExtensions
    {
        // Action Synchronous

        public static IResult<(T, U, V)> Bind<T, U, V>(this (T, U, V) input, Action<T, U, V> function)
        {
            try
            {
                function(input.Item1, input.Item2, input.Item3);
                return new Ok<(T, U, V)>(input);
            }
            catch (Exception e)
            {
                return new Error<(T, U, V)>(e);
            }
        }

        public static IResult<(T, U, V)> Bind<T, U, V>(this IResult<(T, U, V)> input, Action<T, U, V> function)
        {
            switch (input)
            {
                case Ok<(T, U, V)> ok:
                    return ok.Value.Bind(function);
                case Error<(T, U, V)> error:
                    return error;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        // Action Asynchronous

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this Task<(T, U, V)> input, Action<T, U, V> function)
        {
            try
            {
                var i = await input;
                function(i.Item1, i.Item2, i.Item3);
                return new Ok<(T, U, V)>(i);
            }
            catch (Exception e)
            {
                return new Error<(T, U, V)>(e);
            }
        }

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this (T, U, V) input, Func<T, U, V, Task> function)
        {
            try
            {
                await function(input.Item1, input.Item2, input.Item3);
                return new Ok<(T, U, V)>(input);
            }
            catch (Exception e)
            {
                return new Error<(T, U, V)>(e);
            }
        }

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this Task<(T, U, V)> input, Func<T, U, V, Task> function)
        {
            try
            {
                var i = await input;
                await function(i.Item1, i.Item2, i.Item3);
                return new Ok<(T, U, V)>(i);
            }
            catch (Exception e)
            {
                return new Error<(T, U, V)>(e);
            }
        }

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this Task<IResult<(T, U, V)>> input, Action<T, U, V> function)
        {
            try
            {
                var i = await input;
                return i.Bind(function);
            }
            catch (Exception e)
            {
                return new Error<(T, U, V)>(e);
            }
        }

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this IResult<(T, U, V)> input, Func<T, U, V, Task> function)
        {
            switch (input)
            {
                case Ok<(T, U, V)> ok:
                    return await ok.Value.Bind(function);
                case Error<(T, U, V)> error:
                    return error;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this Task<IResult<(T, U, V)>> input, Func<T, U, V, Task> function)
        {
            try
            {
                var i = await input;
                return await i.Bind(function);
            }
            catch (Exception e)
            {
                return new Error<(T, U, V)>(e);
            }
        }

        // Function Synchronous

        public static IResult<W> Bind<T, U, V, W>(this (T, U, V) input, Func<T, U, V, W> function)
        {
            try
            {
                return new Ok<W>(function(input.Item1, input.Item2, input.Item3));
            }
            catch (Exception e)
            {
                return new Error<W>(e);
            }
        }

        public static IResult<W> Bind<T, U, V, W>(this IResult<(T, U, V)> input, Func<T, U, V, W> function)
        {
            switch (input)
            {
                case Ok<(T, U, V)> ok:
                    return ok.Value.Bind(function);
                case Error<(T, U, V)> error:
                    return new Error<W>(error.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        // Function Asynchronous

        public static async Task<IResult<W>> Bind<T, U, V, W>(this Task<(T, U, V)> input, Func<T, U, V, W> function)
        {
            try
            {
                var i = await input;
                return new Ok<W>(function(i.Item1, i.Item2, i.Item3));
            }
            catch (Exception e)
            {
                return new Error<W>(e);
            }
        }

        public static async Task<IResult<W>> Bind<T, U, V, W>(this Task<IResult<(T, U, V)>> input, Func<T, U, V, W> function)
        {
            try
            {
                var i = await input;
                return i.Bind(function);
            }
            catch (Exception e)
            {
                return new Error<W>(e);
            }
        }

        public static async Task<IResult<W>> Bind<T, U, V, W>(this (T, U, V) input, Func<T, U, V, Task<W>> function)
        {
            try
            {
                return new Ok<W>(await function(input.Item1, input.Item2, input.Item3));
            }
            catch (Exception e)
            {
                return new Error<W>(e);
            }
        }

        public static async Task<IResult<W>> Bind<T, U, V, W>(this Task<(T, U, V)> input, Func<T, U, V, Task<W>> function)
        {
            try
            {
                var i = await input;
                return new Ok<W>(await function(i.Item1, i.Item2, i.Item3));
            }
            catch (Exception e)
            {
                return new Error<W>(e);
            }
        }

        public static async Task<IResult<W>> Bind<T, U, V, W>(this IResult<(T, U, V)> input, Func<T, U, V, Task<W>> function)
        {
            switch (input)
            {
                case Ok<(T, U, V)> ok:
                    return await ok.Value.Bind(function);
                case Error<(T, U, V)> error:
                    return new Error<W>(error.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        public static async Task<IResult<W>> Bind<T, U, V, W>(this Task<IResult<(T, U, V)>> input, Func<T, U, V, Task<W>> function)
        {
            try
            {
                return await (await input).Bind(function);
            }
            catch (Exception e)
            {
                return new Error<W>(e);
            }
        }
    }
}
