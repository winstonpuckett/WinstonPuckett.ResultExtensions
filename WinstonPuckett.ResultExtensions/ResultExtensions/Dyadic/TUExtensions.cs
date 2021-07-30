using System;
using System.Threading.Tasks;

namespace WinstonPuckett.ResultExtensions
{
    public static partial class ResultExtensions
    {
        // Action Synchronous

        public static IResult<(T, U)> Bind<T, U>(this (T, U) input, Action<T, U> function)
        {
            try
            {
                function(input.Item1, input.Item2);
                return new Ok<(T, U)>(input);
            }
            catch (Exception e)
            {
                return new Error<(T, U)>(e);
            }
        }

        public static IResult<(T, U)> Bind<T, U>(this IResult<(T, U)> input, Action<T, U> function)
        {
            switch (input)
            {
                case Ok<(T, U)> ok:
                    return ok.Value.Bind(function);
                case Error<(T, U)> error:
                    return error;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        // Action Asynchronous

        public static async Task<IResult<(T, U)>> Bind<T, U>(this Task<(T, U)> input, Action<T, U> function)
        {
            try
            {
                var i = await input;
                function(i.Item1, i.Item2);
                return new Ok<(T, U)>(i);
            }
            catch (Exception e)
            {
                return new Error<(T, U)>(e);
            }
        }

        public static async Task<IResult<(T, U)>> Bind<T, U>(this (T, U) input, Func<T, U, Task> function)
        {
            try
            {
                await function(input.Item1, input.Item2);
                return new Ok<(T, U)>(input);
            }
            catch (Exception e)
            {
                return new Error<(T, U)>(e);
            }
        }

        public static async Task<IResult<(T, U)>> Bind<T, U>(this Task<(T, U)> input, Func<T, U, Task> function)
        {
            try
            {
                var i = await input;
                await function(i.Item1, i.Item2);
                return new Ok<(T, U)>(i);
            }
            catch (Exception e)
            {
                return new Error<(T, U)>(e);
            }
        }

        public static async Task<IResult<(T, U)>> Bind<T, U>(this Task<IResult<(T, U)>> input, Action<T, U> function)
        {
            try
            {
                var i = await input;
                return i.Bind(function);
            }
            catch (Exception e)
            {
                return new Error<(T, U)>(e);
            }
        }

        public static async Task<IResult<(T, U)>> Bind<T, U>(this IResult<(T, U)> input, Func<T, U, Task> function)
        {
            switch (input)
            {
                case Ok<(T, U)> ok:
                    return await ok.Value.Bind(function);
                case Error<(T, U)> error:
                    return error;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        public static async Task<IResult<(T, U)>> Bind<T, U>(this Task<IResult<(T, U)>> input, Func<T, U, Task> function)
        {
            try
            {
                var i = await input;
                return await i.Bind(function);
            }
            catch (Exception e)
            {
                return new Error<(T, U)>(e);
            }
        }

        // Function Synchronous

        public static IResult<V> Bind<T, U, V>(this (T, U) input, Func<T, U, V> function)
        {
            try
            {
                return new Ok<V>(function(input.Item1, input.Item2));
            }
            catch (Exception e)
            {
                return new Error<V>(e);
            }
        }

        public static IResult<V> Bind<T, U, V>(this IResult<(T, U)> input, Func<T, U, V> function)
        {
            switch (input)
            {
                case Ok<(T, U)> ok:
                    return ok.Value.Bind(function);
                case Error<(T, U)> error:
                    return new Error<V>(error.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        // Function Asynchronous

        public static async Task<IResult<V>> Bind<T, U, V>(this Task<(T, U)> input, Func<T, U, V> function)
        {
            try
            {
                var i = await input;
                return new Ok<V>(function(i.Item1, i.Item2));
            }
            catch (Exception e)
            {
                return new Error<V>(e);
            }
        }

        public static async Task<IResult<V>> Bind<T, U, V>(this Task<IResult<(T, U)>> input, Func<T, U, V> function)
        {
            try
            {
                var i = await input;
                return i.Bind(function);
            }
            catch (Exception e)
            {
                return new Error<V>(e);
            }
        }

        public static async Task<IResult<V>> Bind<T, U, V>(this (T, U) input, Func<T, U, Task<V>> function)
        {
            try
            {
                return new Ok<V>(await function(input.Item1, input.Item2));
            }
            catch (Exception e)
            {
                return new Error<V>(e);
            }
        }

        public static async Task<IResult<V>> Bind<T, U, V>(this Task<(T, U)> input, Func<T, U, Task<V>> function)
        {
            try
            {
                var i = await input;
                return new Ok<V>(await function(i.Item1, i.Item2));
            }
            catch (Exception e)
            {
                return new Error<V>(e);
            }
        }

        public static async Task<IResult<V>> Bind<T, U, V>(this IResult<(T, U)> input, Func<T, U, Task<V>> function)
        {
            switch (input)
            {
                case Ok<(T, U)> ok:
                    return await ok.Value.Bind(function);
                case Error<(T, U)> error:
                    return new Error<V>(error.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        public static async Task<IResult<V>> Bind<T, U, V>(this Task<IResult<(T, U)>> input, Func<T, U, Task<V>> function)
        {
            try
            {
                return await (await input).Bind(function);
            }
            catch (Exception e)
            {
                return new Error<V>(e);
            }
        }
    }
}
