using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinstonPuckett.ResultExtensions
{
    public static partial class ResultExtensions
    {
        // Action Synchronous

        public static IResult<T> Bind<T>(this T input, IEnumerable<Action<T>> functions)
        {
            try
            {
                foreach (var function in functions)
                    function(input);

                return new Ok<T>(input);
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }
        }

        public static IResult<T> Bind<T>(this IResult<T> input, IEnumerable<Action<T>> functions)
        {
            switch (input)
            {
                case Ok<T> ok:
                    return ok.Value.Bind(functions);
                case Error<T> error:
                    return error;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        // Action Asynchronous

        public static async Task<IResult<T>> Bind<T>(this Task<T> input, IEnumerable<Action<T>> functions)
        {
            try
            {
                var i = await input;
                foreach (var function in functions)
                    function(i);
                return new Ok<T>(i);
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }
        }

        public static async Task<IResult<T>> Bind<T>(this T input, IEnumerable<Func<T, Task>> functions)
        {
            try
            {
                foreach (var function in functions)
                    await function(input);
                return new Ok<T>(input);
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }
        }

        public static async Task<IResult<T>> Bind<T>(this Task<T> input, IEnumerable<Func<T, Task>> functions)
        {
            try
            {
                var i = await input;
                foreach (var function in functions)
                    await function(i);

                return new Ok<T>(i);
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }
        }

        public static async Task<IResult<T>> Bind<T>(this Task<IResult<T>> input, IEnumerable<Action<T>> functions)
        {
            try
            {
                var i = await input;
                return i.Bind(functions);
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }
        }

        public static async Task<IResult<T>> Bind<T>(this IResult<T> input, IEnumerable<Func<T, Task>> functions)
        {
            switch (input)
            {
                case Ok<T> ok:
                    return await ok.Value.Bind(functions);
                case Error<T> error:
                    return error;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        public static async Task<IResult<T>> Bind<T>(this Task<IResult<T>> input, IEnumerable<Func<T, Task>> functions)
        {
            try
            {
                var i = await input;
                return await i.Bind(functions);
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }
        }

        // Function Synchronous

        public static IResult<IEnumerable<U>> Bind<T, U>(this T input, IEnumerable<Func<T, U>> functions)
        {
            try
            {
                return new Ok<IEnumerable<U>>(functions.Select(f => f(input)));
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<U>>(e);
            }
        }

        public static IResult<IEnumerable<U>> Bind<T, U>(this IResult<T> input, IEnumerable<Func<T, U>> functions)
        {
            switch (input)
            {
                case Ok<T> ok:
                    return ok.Value.Bind(functions);
                case Error<T> error:
                    return new Error<IEnumerable<U>>(error.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        // Function Asynchronous

        public static async Task<IResult<IEnumerable<U>>> Bind<T, U>(this Task<T> input, IEnumerable<Func<T, U>> functions)
        {
            try
            {
                var i = await input;
                return new Ok<IEnumerable<U>>(functions.Select(f => f(i)));
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<U>>(e);
            }
        }

        public static async Task<IResult<IEnumerable<U>>> Bind<T, U>(this Task<IResult<T>> input, IEnumerable<Func<T, U>> functions)
        {
            try
            {
                var i = await input;
                return i.Bind(functions);
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<U>>(e);
            }
        }

        public static async Task<IResult<IEnumerable<U>>> Bind<T, U>(this T input, IEnumerable<Func<T, Task<U>>> functions)
        {
            try
            {
                return new Ok<IEnumerable<U>>(await Task.WhenAll(functions.Select(f => f(input))));
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<U>>(e);
            }
        }

        public static async Task<IResult<IEnumerable<U>>> Bind<T, U>(this Task<T> input, IEnumerable<Func<T, Task<U>>> functions)
        {
            try
            {
                var i = await input;
                return new Ok<IEnumerable<U>>(await Task.WhenAll(functions.Select(f => f(i))));
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<U>>(e);
            }
        }

        public static async Task<IResult<IEnumerable<U>>> Bind<T, U>(this IResult<T> input, IEnumerable<Func<T, Task<U>>> functions)
        {
            switch (input)
            {
                case Ok<T> ok:
                    return await ok.Value.Bind(functions);
                case Error<T> error:
                    return new Error<IEnumerable<U>>(error.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(input));
            }
        }

        public static async Task<IResult<IEnumerable<U>>> Bind<T, U>(this Task<IResult<T>> input, IEnumerable<Func<T, Task<U>>> functions)
        {
            try
            {
                return await (await input).Bind(functions);
            }
            catch (Exception e)
            {
                return new Error<IEnumerable<U>>(e);
            }
        }
    }
}
