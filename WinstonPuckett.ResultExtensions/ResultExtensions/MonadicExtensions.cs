using System;
using System.Threading.Tasks;

namespace WinstonPuckett.ResultExtensions
{
    public static partial class ResultExtensions
    {
        // Action Synchronous

        public static IResult<T> Bind<T>(this T input, Action<T> function)
            => input.OkOrError(function);

        public static IResult<T> Bind<T>(this IResult<T> input, Action<T> function)
            => input.UnwrapThenOkOrError(function);

        // Action Asynchronous

        public static async Task<IResult<T>> Bind<T>(this Task<T> input, Action<T> function)
            => (await input).OkOrError(function);

        public static async Task<IResult<T>> Bind<T>(this T input, Func<T, Task> function)
            => await input.OkOrErrorAsync(function);

        public static async Task<IResult<T>> Bind<T>(this Task<T> input, Func<T, Task> function)
            => await (await input).OkOrErrorAsync(function);

        // Function Synchronous

        public static IResult<U> Bind<T, U>(this T input, Func<T, U> function)
            => input.OkOrError(function);

        public static IResult<U> Bind<T, U>(this IResult<T> input, Func<T, U> function)
            => input.UnwrapThenOkOrError(function);
    }
}
