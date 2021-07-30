using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinstonPuckett.ResultExtensions
{
    public static partial class ResultExtensions
    {
        // Action Synchronous

        public static IResult<(T, U)> Bind<T, U>(this (T, U) input, IEnumerable<Action<T, U>> functions)
        {
            try
            {
                foreach (var function in functions)
                    function(input.Item1, input.Item2);

                return new Ok<(T, U)>(input);
            }
            catch (Exception e)
            {
                return new Error<(T, U)>(e);
            }
        }

        public static IResult<(T, U)> Bind<T, U>(this IResult<(T, U)> input, IEnumerable<Action<T, U>> functions)
        {
            switch (input)
            {
                case Ok<(T, U)> ok:
                    return ok.Value.Bind(functions);
                case Error<(T, U)> error:
                    return error;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        // Action Asynchronous

        public static async Task<IResult<(T, U)>> Bind<T, U>(this Task<(T, U)> input, IEnumerable<Action<T, U>> functions)
        {
            try
            {
                var i = await input;
                foreach (var function in functions)
                    function(i.Item1, i.Item2);

                return new Ok<(T, U)>(i);
            }
            catch (Exception e)
            {
                return new Error<(T, U)>(e);
            }
        }

        public static async Task<IResult<(T, U)>> Bind<T, U>(this (T, U) input, IEnumerable<Func<T, U, Task>> functions)
        {
            try
            {
                foreach (var function in functions)
                    await function(input.Item1, input.Item2);

                return new Ok<(T, U)>(input);
            }
            catch (Exception e)
            {
                return new Error<(T, U)>(e);
            }
        }

        public static async Task<IResult<(T, U)>> Bind<T, U>(this Task<(T, U)> input, IEnumerable<Func<T, U, Task>> functions)
        {
            try
            {
                var i = await input;
                foreach (var function in functions)
                    await function(i.Item1, i.Item2);

                return new Ok<(T, U)>(i);
            }
            catch (Exception e)
            {
                return new Error<(T, U)>(e);
            }
        }

        public static async Task<IResult<(T, U)>> Bind<T, U>(this Task<IResult<(T, U)>> input, IEnumerable<Action<T, U>> functions)
        {
            try
            {
                var i = await input;
                return i.Bind(functions);
            }
            catch (Exception e)
            {
                return new Error<(T, U)>(e);
            }
        }

        public static async Task<IResult<(T, U)>> Bind<T, U>(this IResult<(T, U)> input, IEnumerable<Func<T, U, Task>> functions)
        {
            switch (input)
            {
                case Ok<(T, U)> ok:
                    return await ok.Value.Bind(functions);
                case Error<(T, U)> error:
                    return error;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        public static async Task<IResult<(T, U)>> Bind<T, U>(this Task<IResult<(T, U)>> input, IEnumerable<Func<T, U, Task>> functions)
        {
            try
            {
                var i = await input;
                return await i.Bind(functions);
            }
            catch (Exception e)
            {
                return new Error<(T, U)>(e);
            }
        }

        // Function Synchronous

        public static IResult<IEnumerable<V>> Bind<T, U, V>(this (T, U) input, IEnumerable<Func<T, U, V>> functions)
        {
            try
            {
                return new Ok<IEnumerable<V>>(functions.Select(f => f(input.Item1, input.Item2)).ToList());
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<V>>(e);
            }
        }

        public static IResult<IEnumerable<V>> Bind<T, U, V>(this IResult<(T, U)> input, IEnumerable<Func<T, U, V>> functions)
        {
            switch (input)
            {
                case Ok<(T, U)> ok:
                    return ok.Value.Bind(functions);
                case Error<(T, U)> error:
                    return new Error<IEnumerable<V>>(error.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        // Function Asynchronous

        public static async Task<IResult<IEnumerable<V>>> Bind<T, U, V>(this Task<(T, U)> input, IEnumerable<Func<T, U, V>> functions)
        {
            try
            {
                var i = await input;
                return new Ok<IEnumerable<V>>(functions.Select(f => f(i.Item1, i.Item2)).ToList());
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<V>>(e);
            }
        }

        public static async Task<IResult<IEnumerable<V>>> Bind<T, U, V>(this Task<IResult<(T, U)>> input, IEnumerable<Func<T, U, V>> functions)
        {
            try
            {
                var i = await input;
                return i.Bind(functions);
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<V>>(e);
            }
        }

        public static async Task<IResult<IEnumerable<V>>> Bind<T, U, V>(this (T, U) input, IEnumerable<Func<T, U, Task<V>>> functions)
        {
            try
            {
                return new Ok<IEnumerable<V>>(await Task.WhenAll(functions.Select(f => f(input.Item1, input.Item2)).ToList()));
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<V>>(e);
            }
        }

        public static async Task<IResult<IEnumerable<V>>> Bind<T, U, V>(this Task<(T, U)> input, IEnumerable<Func<T, U, Task<V>>> functions)
        {
            try
            {
                var i = await input;
                return new Ok<IEnumerable<V>>(await Task.WhenAll(functions.Select(f => f(i.Item1, i.Item2))));
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<V>>(e);
            }
        }

        public static async Task<IResult<IEnumerable<V>>> Bind<T, U, V>(this IResult<(T, U)> input, IEnumerable<Func<T, U, Task<V>>> functions)
        {
            switch (input)
            {
                case Ok<(T, U)> ok:
                    return await ok.Value.Bind(functions);
                case Error<(T, U)> error:
                    return new Error<IEnumerable<V>>(error.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        public static async Task<IResult<IEnumerable<V>>> Bind<T, U, V>(this Task<IResult<(T, U)>> input, IEnumerable<Func<T, U, Task<V>>> functions)
        {
            try
            {
                return await (await input).Bind(functions);
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<V>>(e);
            }
        }
    }
}
