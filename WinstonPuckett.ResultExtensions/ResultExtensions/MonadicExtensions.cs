using System;

namespace WinstonPuckett.ResultExtensions
{
    public static partial class ResultExtensions
    {
        // Action Synchronous

        public static IResult<T> Bind<T>(this T input, Action<T> function)
            => input.OkOrError(function);

        public static IResult<T> Bind<T>(this IResult<T> input, Action<T> function)
            => input.UnwrapThenOkOrError(function);

        // Function Synchronous

        public static IResult<U> Bind<T, U>(this T input, Func<T, U> function)
            => input.OkOrError(function);

        public static IResult<U> Bind<T, U>(this IResult<T> input, Func<T, U> function)
            => input.UnwrapThenOkOrError(function);
    }
}
