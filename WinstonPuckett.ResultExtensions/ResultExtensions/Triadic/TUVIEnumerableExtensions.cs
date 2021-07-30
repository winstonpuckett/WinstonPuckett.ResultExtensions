using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinstonPuckett.ResultExtensions
{
    public static partial class ResultExtensions
    {
        // Action Synchronous

        public static IResult<(T, U, V)> Bind<T, U, V>(this (T, U, V) input, IEnumerable<Action<T, U, V>> functions)
        {
            try
            {
                foreach (var function in functions)
                    function(input.Item1, input.Item2, input.Item3);

                return new Ok<(T, U, V)>(input);
            }
            catch (Exception e)
            {
                return new Error<(T, U, V)>(e);
            }
        }

        public static IResult<(T, U, V)> Bind<T, U, V>(this IResult<(T, U, V)> input, IEnumerable<Action<T, U, V>> functions)
        {
            switch (input)
            {
                case Ok<(T, U, V)> ok:
                    return ok.Value.Bind(functions);
                case Error<(T, U, V)> error:
                    return error;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        // Action Asynchronous

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this Task<(T, U, V)> input, IEnumerable<Action<T, U, V>> functions)
        {
            try
            {
                var i = await input;
                foreach (var function in functions)
                    function(i.Item1, i.Item2, i.Item3);

                return new Ok<(T, U, V)>(i);
            }
            catch (Exception e)
            {
                return new Error<(T, U, V)>(e);
            }
        }

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this (T, U, V) input, IEnumerable<Func<T, U, V, Task>> functions)
        {
            try
            {
                foreach (var function in functions)
                    await function(input.Item1, input.Item2, input.Item3);

                return new Ok<(T, U, V)>(input);
            }
            catch (Exception e)
            {
                return new Error<(T, U, V)>(e);
            }
        }

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this Task<(T, U, V)> input, IEnumerable<Func<T, U, V, Task>> functions)
        {
            try
            {
                var i = await input;
                foreach (var function in functions)
                    await function(i.Item1, i.Item2, i.Item3);

                return new Ok<(T, U, V)>(i);
            }
            catch (Exception e)
            {
                return new Error<(T, U, V)>(e);
            }
        }

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this Task<IResult<(T, U, V)>> input, IEnumerable<Action<T, U, V>> functions)
        {
            try
            {
                var i = await input;
                return i.Bind(functions);
            }
            catch (Exception e)
            {
                return new Error<(T, U, V)>(e);
            }
        }

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this IResult<(T, U, V)> input, IEnumerable<Func<T, U, V, Task>> functions)
        {
            switch (input)
            {
                case Ok<(T, U, V)> ok:
                    return await ok.Value.Bind(functions);
                case Error<(T, U, V)> error:
                    return error;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this Task<IResult<(T, U, V)>> input, IEnumerable<Func<T, U, V, Task>> functions)
        {
            try
            {
                var i = await input;
                return await i.Bind(functions);
            }
            catch (Exception e)
            {
                return new Error<(T, U, V)>(e);
            }
        }

        // Function Synchronous

        public static IResult<IEnumerable<W>> Bind<T, U, V, W>(this (T, U, V) input, IEnumerable<Func<T, U, V, W>> functions)
        {
            try
            {
                return new Ok<IEnumerable<W>>(functions.Select(f => f(input.Item1, input.Item2, input.Item3)).ToList());
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<W>>(e);
            }
        }

        public static IResult<IEnumerable<W>> Bind<T, U, V, W>(this IResult<(T, U, V)> input, IEnumerable<Func<T, U, V, W>> functions)
        {
            switch (input)
            {
                case Ok<(T, U, V)> ok:
                    return ok.Value.Bind(functions);
                case Error<(T, U, V)> error:
                    return new Error<IEnumerable<W>>(error.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        // Function Asynchronous

        public static async Task<IResult<IEnumerable<W>>> Bind<T, U, V, W>(this Task<(T, U, V)> input, IEnumerable<Func<T, U, V, W>> functions)
        {
            try
            {
                var i = await input;
                return new Ok<IEnumerable<W>>(functions.Select(f => f(i.Item1, i.Item2, i.Item3)).ToList());
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<W>>(e);
            }
        }

        public static async Task<IResult<IEnumerable<W>>> Bind<T, U, V, W>(this Task<IResult<(T, U, V)>> input, IEnumerable<Func<T, U, V, W>> functions)
        {
            try
            {
                var i = await input;
                return i.Bind(functions);
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<W>>(e);
            }
        }

        public static async Task<IResult<IEnumerable<W>>> Bind<T, U, V, W>(this (T, U, V) input, IEnumerable<Func<T, U, V, Task<W>>> functions)
        {
            try
            {
                return new Ok<IEnumerable<W>>(await Task.WhenAll(functions.Select(f => f(input.Item1, input.Item2, input.Item3)).ToList()));
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<W>>(e);
            }
        }

        public static async Task<IResult<IEnumerable<W>>> Bind<T, U, V, W>(this Task<(T, U, V)> input, IEnumerable<Func<T, U, V, Task<W>>> functions)
        {
            try
            {
                var i = await input;
                return new Ok<IEnumerable<W>>(await Task.WhenAll(functions.Select(f => f(i.Item1, i.Item2, i.Item3))));
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<W>>(e);
            }
        }

        public static async Task<IResult<IEnumerable<W>>> Bind<T, U, V, W>(this IResult<(T, U)> input, IEnumerable<Func<T, U, V, Task<W>>> functions)
        {
            switch (input)
            {
                case Ok<(T, U, V)> ok:
                    return await ok.Value.Bind(functions);
                case Error<(T, U, V)> error:
                    return new Error<IEnumerable<W>>(error.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        public static async Task<IResult<IEnumerable<W>>> Bind<T, U, V, W>(this Task<IResult<(T, U)>> input, IEnumerable<Func<T, U, V, Task<W>>> functions)
        {
            try
            {
                return await (await input).Bind(functions);
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<W>>(e);
            }
        }
    }
}
