using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WinstonPuckett.ResultExtensions
{
    public static class UnwrapExtensions
    {
        // Unwrap

        public static T Unwrap<T>(this IResult<T> result)
        {
            switch (result)
            {
                case Ok<T> ok:
                    return ok.Value;
                case Error<T> err:
                    throw err.Exception;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(result));
            }
        }

        public static async Task<T> Unwrap<T>(this Task<IResult<T>> result)
        {
            switch (await result)
            {
                case Ok<T> ok:
                    return ok.Value;
                case Error<T> err:
                    throw err.Exception;
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(result));
            }
        }

        // Unwrap or...

        public static T UnwrapOr<T>(this IResult<T> result, Func<Exception, T> transform)
        {
            switch (result)
            {
                case Ok<T> ok:
                    return ok.Value;
                case Error<T> err:
                    return transform(err.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(result));
            }
        }
        public static async Task<T> UnwrapOr<T>(this IResult<T> result, Func<Exception, Task<T>> transform)
        {
            switch (result)
            {
                case Ok<T> ok:
                    return ok.Value;
                case Error<T> err:
                    return await transform(err.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(result));
            }
        }

        public static async Task<T> UnwrapOr<T>(this Task<IResult<T>> result, Func<Exception, T> transform)
        {
            try
            {
                switch (await result)
                {
                    case Ok<T> ok:
                        return ok.Value;
                    case Error<T> err:
                        return transform(err.Exception);
                    default:
                        throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(result));
                }
            }
            catch (TaskCanceledException e)
            {
                return transform(e);
            }
        }

        public static async Task<T> UnwrapOr<T>(this Task<IResult<T>> result, Func<Exception, Task<T>> transform)
        {
            IResult<T> r;
            
            try
            {
                r = await result;
            }
            catch (TaskCanceledException e)
            {
                return await transform(e);
            }

            switch (r)
            {
                case Ok<T> ok:
                    return ok.Value;
                case Error<T> err:
                    return await transform(err.Exception);
                default:
                    throw new ArgumentException("Cannot determine whether input is Error or Ok. This might happen if you implement IResult. Try setting a breakpoint on the method before this error and see if it sends back an unexpected IResult type.", nameof(result));
            }
        }
    }
}
