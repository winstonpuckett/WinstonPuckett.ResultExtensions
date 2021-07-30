using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinstonPuckett.ResultExtensions
{
    public static partial class ResultExtensions
    {
        // Action Synchronous

        public static IResult<(T, U, V)> Bind<T, U, V>(this (T, U, V) input, params Action<T, U, V>[] functions)
            => input.Bind((IEnumerable<Action<T, U, V>>)functions);

        public static IResult<(T, U, V)> Bind<T, U, V>(this IResult<(T, U, V)> input, params Action<T, U, V>[] functions)
            => input.Bind((IEnumerable<Action<T, U, V>>)functions);

        // Action Asynchronous

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this Task<(T, U, V)> input, params Action<T, U, V>[] functions)
            => await input.Bind((IEnumerable<Action<T, U, V>>)functions);

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this (T, U, V) input, params Func<T, U, V, Task>[] functions)
            => await input.Bind((IEnumerable<Func<T, U, V, Task>>)functions);

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this Task<(T, U, V)> input, params Func<T, U, V, Task>[] functions)
            => await input.Bind((IEnumerable<Func<T, U, V, Task>>)functions);

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this Task<IResult<(T, U, V)>> input, params Action<T, U, V>[] functions)
            => await input.Bind((IEnumerable<Action<T, U, V>>)functions);

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this IResult<(T, U, V)> input, params Func<T, U, V, Task>[] functions)
            => await input.Bind((IEnumerable<Func<T, U, V, Task>>)functions);

        public static async Task<IResult<(T, U, V)>> Bind<T, U, V>(this Task<IResult<(T, U, V)>> input, params Func<T, U, V, Task>[] functions)
            => await input.Bind((IEnumerable<Func<T, U, V, Task>>)functions);

        // Function Synchronous

        public static IResult<IEnumerable<W>> Bind<T, U, V, W>(this (T, U, V) input, params Func<T, U, V, W>[] functions)
            => input.Bind((IEnumerable<Func<T, U, V, W>>)functions);

        public static IResult<IEnumerable<W>> Bind<T, U, V, W>(this IResult<(T, U, V)> input, params Func<T, U, V, W>[] functions)
            => input.Bind((IEnumerable<Func<T, U, V, W>>)functions);

        // Function Asynchronous

        public static async Task<IResult<IEnumerable<W>>> Bind<T, U, V, W>(this Task<(T, U, V)> input, params Func<T, U, V, W>[] functions)
            => await input.Bind((IEnumerable<Func<T, U, V, W>>)functions);

        public static async Task<IResult<IEnumerable<W>>> Bind<T, U, V, W>(this Task<IResult<(T, U, V)>> input, params Func<T, U, V, W>[] functions)
            => await input.Bind((IEnumerable<Func<T, U, V, W>>)functions);
        
        public static async Task<IResult<IEnumerable<W>>> Bind<T, U, V, W>(this (T, U, V) input, params Func<T, U, V, Task<W>>[] functions)
            => await input.Bind((IEnumerable<Func<T, U, V, Task<W>>>)functions);

        public static async Task<IResult<IEnumerable<W>>> Bind<T, U, V, W>(this Task<(T, U, V)> input, params Func<T, U, V, Task<W>>[] functions)
            => await input.Bind((IEnumerable<Func<T, U, V, Task<W>>>)functions);

        public static async Task<IResult<IEnumerable<W>>> Bind<T, U, V, W>(this IResult<(T, U)> input, params Func<T, U, V, Task<W>>[] functions)
            => await input.Bind((IEnumerable<Func<T, U, V, Task<W>>>)functions);


        public static async Task<IResult<IEnumerable<W>>> Bind<T, U, V, W>(this Task<IResult<(T, U)>> input, params Func<T, U, V, Task<W>>[] functions)
            => await input.Bind((IEnumerable<Func<T, U, V, Task<W>>>)functions);
    }
}
