using System;

namespace WinstonPuckett.ResultExtensions
{
    public static partial class ResultExtensions
    {
        public static IResult<T> Bind<T>(this T input, Action<T> function)
            => input.OkOrError(function);

        public static IResult<T> Bind<T>(this IResult<T> input, Action<T> function)
            => input.UnwrapThenOkOrError(function);
    }
}
