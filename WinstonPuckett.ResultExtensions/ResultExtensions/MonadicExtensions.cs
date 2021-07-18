using System;

namespace WinstonPuckett.ResultExtensions
{
    public static partial class ResultExtensions
    {
        public static IResult<T> Bind<T>(this T input, Action<T> function)
        {
            try
            {
                function(input);
                return new Ok<T>(input);
            }
            catch (Exception e)
            {
                return new Error<T>(input, e);
            }
        }
    }
}
