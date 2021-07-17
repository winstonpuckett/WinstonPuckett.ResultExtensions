# Purpose

We need to know the complete list of everything we accept. Libraries like this, which are a collection of functions which do the same thing for different signatures, provide easy opportunities to miss something.

# Signatures

Foreach non-async, there are four signatures to accept

<sub>Action based</sub>
- T, Action<T>
- IResult\<T\>, Action<T>

<sub>Function based</sub>
- T, Func<T, U>
- IResult\<T\> -> Func<T, U>

Foreach async, there are 12 signatures to accept

<sub>Action based</sub>
- Task\<T\>, Action\<T\>
- Task\<IResult\<T\>\>, Action\<T\>
- T, Func\<T, Task\>
- IResult\<T\>, Func\<T, Task\>
- Task\<T\>, Func\<T, Task>
- Task\<IResult\<T\>\>, Func\<T, Task\>

<sub>Function based</sub>
- Task\<T\>, Func\<T, U\>
- Task\<IResult\<T\>\>, Func\<T, U\>
- T, Func\<T, Task\<U\>\>
- IResult\<T\>, Func\<T, Task\<U\>\>
- Task\<T\>, Func\<T, Task\<U\>\>
- Task\<IResult\<T\>\>, Func\<T, Task\<U\>\>

# Bind / Bind Async (144 functions)

## Monad (48)

- Base (16)
- IEnumerable (16)
- Params (16)

## Dyad (48)

- Base (16)
- IEnumerable (16)
- Params (16)

## Triad (48)

- Base (16)
- IEnumerable (16)
- Params (16)

# Unwrap (2 functions)

## Unwrap
IResult\<T\> -> U: throws if error

## Unwrap or
IResult\<T\>, Func\<Exception\> -> U: Uses Func to transform into T if error.