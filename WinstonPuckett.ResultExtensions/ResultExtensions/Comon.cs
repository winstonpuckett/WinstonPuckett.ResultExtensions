using System;

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
                return new Error<T>(input, e);
            }
        }
    }
}
