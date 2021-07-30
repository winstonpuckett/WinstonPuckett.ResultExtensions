using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinstonPuckett.ResultExtensions
{
    public static partial class ResultExtensions
    {
        // Action Synchronous

        public static IResult<T> Bind<T>(this T input, params Action<T>[] functions)
            => input.Bind((IEnumerable<Action<T>>)functions);

        public static IResult<T> Bind<T>(this IResult<T> input, params Action<T>[] functions)
            => input.Bind((IEnumerable<Action<T>>)functions);

        // Action Asynchronous

        public static async Task<IResult<T>> Bind<T>(this Task<T> input, params Action<T>[] functions)
            => await input.Bind((IEnumerable<Action<T>>)functions);

        public static async Task<IResult<T>> Bind<T>(this T input, params Func<T, Task>[] functions)
            => await input.Bind((IEnumerable<Func<T, Task>>)functions);

        public static async Task<IResult<T>> Bind<T>(this Task<T> input, params Func<T, Task>[] functions)
            => await input.Bind((IEnumerable<Func<T, Task>>)functions);

        public static async Task<IResult<T>> Bind<T>(this Task<IResult<T>> input, params Action<T>[] functions)
            => await input.Bind((IEnumerable<Action<T>>)functions);

        public static async Task<IResult<T>> Bind<T>(this IResult<T> input, params Func<T, Task>[] functions)
            => await input.Bind((IEnumerable<Func<T, Task>>)functions);

        public static async Task<IResult<T>> Bind<T>(this Task<IResult<T>> input, params Func<T, Task>[] functions)
            => await input.Bind((IEnumerable<Func<T, Task>>)functions);

        // Function Synchronous

        public static IResult<IEnumerable<U>> Bind<T, U>(this T input, params Func<T, U>[] functions)
            => input.Bind((IEnumerable<Func<T, U>>)functions);

        public static IResult<IEnumerable<U>> Bind<T, U>(this IResult<T> input, params Func<T, U>[] functions)
            => input.Bind((IEnumerable<Func<T, U>>)functions);

        // Function Asynchronous

        public static async Task<IResult<IEnumerable<U>>> Bind<T, U>(this Task<T> input, params Func<T, U>[] functions)
            => await input.Bind((IEnumerable<Func<T, U>>)functions);

        public static async Task<IResult<IEnumerable<U>>> Bind<T, U>(this Task<IResult<T>> input, params Func<T, U>[] functions)
            => await input.Bind((IEnumerable<Func<T, U>>)functions);

        public static async Task<IResult<IEnumerable<U>>> Bind<T, U>(this T input, params Func<T, Task<U>>[] functions)
            => await input.Bind((IEnumerable<Func<T, Task<U>>>)functions);

        public static async Task<IResult<IEnumerable<U>>> Bind<T, U>(this Task<T> input, params Func<T, Task<U>>[] functions)
            => await input.Bind((IEnumerable<Func<T, Task<U>>>)functions);

        public static async Task<IResult<IEnumerable<U>>> Bind<T, U>(this IResult<T> input, params Func<T, Task<U>>[] functions)
            => await input.Bind((IEnumerable<Func<T, Task<U>>>)functions);

        public static async Task<IResult<IEnumerable<U>>> Bind<T, U>(this Task<IResult<T>> input, params Func<T, Task<U>>[] functions)
            => await input.Bind((IEnumerable<Func<T, Task<U>>>)functions);
    }
}
