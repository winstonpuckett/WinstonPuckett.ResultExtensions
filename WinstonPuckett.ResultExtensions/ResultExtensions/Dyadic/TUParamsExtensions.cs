using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinstonPuckett.ResultExtensions
{
    public static partial class ResultExtensions
    {
        // Action Synchronous

        public static IResult<(T, U)> Bind<T, U>(this (T, U) input, params Action<T, U>[] functions)
            => input.Bind(functions);

        public static IResult<(T, U)> Bind<T, U>(this IResult<(T, U)> input, params Action<T, U>[] functions)
            => input.Bind(functions);


        // Action Asynchronous

        public static async Task<IResult<(T, U)>> Bind<T, U>(this Task<(T, U)> input, params Action<T, U>[] functions)
            => await input.Bind(functions);

        public static async Task<IResult<(T, U)>> Bind<T, U>(this (T, U) input, params Func<T, U, Task>[] functions)
            => await input.Bind(functions);

        public static async Task<IResult<(T, U)>> Bind<T, U>(this Task<(T, U)> input, params Func<T, U, Task>[] functions)
            => await input.Bind(functions);

        public static async Task<IResult<(T, U)>> Bind<T, U>(this Task<IResult<(T, U)>> input, params Action<T, U>[] functions)
            => await input.Bind(functions);

        public static async Task<IResult<(T, U)>> Bind<T, U>(this IResult<(T, U)> input, params Func<T, U, Task>[] functions)
            => await input.Bind(functions);

        public static async Task<IResult<(T, U)>> Bind<T, U>(this Task<IResult<(T, U)>> input, params Func<T, U, Task>[] functions)
            => await input.Bind(functions);

        // Function Synchronous

        public static IResult<IEnumerable<V>> Bind<T, U, V>(this (T, U) input, params Func<T, U, V>[] functions)
            => input.Bind(functions);


        public static IResult<IEnumerable<V>> Bind<T, U, V>(this IResult<(T, U)> input, params Func<T, U, V>[] functions)
            => input.Bind(functions);

        // Function Asynchronous

        public static async Task<IResult<IEnumerable<V>>> Bind<T, U, V>(this Task<(T, U)> input, params Func<T, U, V>[] functions)
            => await input.Bind(functions);

        public static async Task<IResult<IEnumerable<V>>> Bind<T, U, V>(this Task<IResult<(T, U)>> input, params Func<T, U, V>[] functions)
            => await input.Bind(functions);

        public static async Task<IResult<IEnumerable<V>>> Bind<T, U, V>(this (T, U) input, params Func<T, U, Task<V>>[] functions)
            => await input.Bind(functions);

        public static async Task<IResult<IEnumerable<V>>> Bind<T, U, V>(this Task<(T, U)> input, params Func<T, U, Task<V>>[] functions)
            => await input.Bind(functions);

        public static async Task<IResult<IEnumerable<V>>> Bind<T, U, V>(this IResult<(T, U)> input, params Func<T, U, Task<V>>[] functions)
            => await input.Bind(functions);

        public static async Task<IResult<IEnumerable<V>>> Bind<T, U, V>(this Task<IResult<(T, U)>> input, params Func<T, U, Task<V>>[] functions)
            => await input.Bind(functions);
    }
}
