using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

#pragma warning disable AvoidAsyncVoid // Avoid async void

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class TaskExtensions
    {
        public static Task Empty { get; } = Task.Delay(0);

        public static async void Forget(this Task task, bool report = true)
        {
            try
            {
                if (task != null)
                    await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Report();
            }
        }

        public static async Task WhenErrorReportAsync(this Task inputTask)
        {
            try
            {
                await inputTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Report();
            }
        }

        public static async Task<T> WhenErrorReportAsync<T>(this Task<T> inputTask) where T : class
        {
            try
            {
                return await inputTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Report();
                return null;
            }
        }

        public static async Task<TResult> WhenNotNullAsync<TInput, TResult>(
            this Task<TInput> inputTask,
            Func<TInput, Task<TResult>> evaluator)
            where TResult : class
        {
            if (inputTask == null)
                return null;

            var result = await inputTask;
            if (result == null)
                return null;

            return await evaluator(result).ConfigureAwait(false);
        }

        public static async Task<bool?> WhenNotNullAsync<TInput>(
            this Task<TInput> inputTask,
            Func<TInput, Task<bool>> evaluator)
        {
            if (inputTask == null)
                return null;

            var result = await inputTask;

            return await evaluator(result).ConfigureAwait(false);
        }

        public static async Task<TResult> WhenNotNullAsync<TInput, TResult>(
            this Task<TInput> inputTask,
            Func<TInput, Task<TResult>> evaluator,
            Action progressUpdate)
            where TResult : class
        {
            if (inputTask == null)
                return null;

            var result = await inputTask;
            if (result == null)
                return null;

            progressUpdate();
            return await evaluator(result).ConfigureAwait(false);
        }

        public static async Task<TResult> WhenNotNullAsync<TInput, TResult>(
            this Task<TInput> inputTask,
            Func<TInput, TResult> evaluator)
            where TResult : class
        {
            if (inputTask == null)
                return null;

            var result = await inputTask;
            if (result == null)
                return null;

            return evaluator(result);
        }

        public static async Task WhenNotNullAsync<TInput>(this Task<TInput> inputTask,
                                                     Action<TInput> evaluator)
        {
            if (inputTask == null)
                return;

            var result = await inputTask;
            if (result == null)
                return;

            evaluator(result);
        }

        public static async Task<T> WhenErrorAsync<T>(this Task<T> inputTask,
                                                 Func<Exception, Task<T>> evaluator)
        {
            Exception error;
            try
            {
                return await inputTask;
            }
            catch (Exception e)
            {
                error = e;
            }
            return await evaluator(error);
        }

        public static async Task<T> WhenErrorAsync<T>(this Task<T> inputTask, Func<Exception, T> evaluator)
        {
            Exception error;
            try
            {
                return await inputTask;
            }
            catch (Exception e)
            {
                error = e;
            }
            return evaluator(error.Unwrap());
        }

        public static async Task<T> WhenErrorIgnoreAsync<T>(this Task<T> inputTask) where T : class
        {
            try
            {
                return await inputTask.ConfigureAwait(false);
            }
            catch
            {
                //Ignore
                return null;
            }
        }

        public static async Task<T> WhenErrorIgnoreAsync<T>(this Task<T> inputTask,
                                                       Predicate<Exception> shouldIgnore) where T : class
        {
            try
            {
                return await inputTask.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                if (shouldIgnore(e))
                    return null;

                throw;
            }
        }

        public static async Task WhenErrorIgnoreAsync(this Task inputTask)
        {
            try
            {
                await inputTask.ConfigureAwait(false);
            }
            catch
            {
                //Ignore
            }
        }

        public static async Task<T> AsNotNullAsync<T>(this Task<T> inputTask) where T : new()
        {
            var input = await inputTask.ConfigureAwait(false);
            if (input == null) return new T();
            return input;
        }

        public static async Task<TResult> CastAsync<TResult>(this Task<object> inputTask)
            where TResult : class
        {
            return await inputTask.ConfigureAwait(false) as TResult;
        }

        public static async Task<TResult> CastAsync<TResult, TInput>(this Task<TInput> inputTask)
            where TInput : TResult where TResult : class
        {
            return await inputTask.ConfigureAwait(false) as TResult;
        }

        public static bool IsValid<T>(this Task<T> task)
        {
            if (task == null || task.IsFaulted || task.IsCanceled)
                return false;

            return true;
        }

        public static Func<T, Task> ToAsync<T>(this Action<T> action) => o =>
        {
            action(o);
            return Task.CompletedTask;
        };

        public static Func<Task> ToAsync(this Action action) => () =>
        {
            action();
            return Task.CompletedTask;
        };

        private static Exception Unwrap(this Exception exception)
        {
            var aggregate = exception as AggregateException;
            if (aggregate == null)
                return exception;

            return aggregate.InnerException.Unwrap();
        }
    }
}

#pragma warning restore AvoidAsyncVoid // Avoid async void