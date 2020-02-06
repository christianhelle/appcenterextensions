using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AppCenterExtensions.Extensions
{
    /// <summary>
    /// Exposes extension methods for the Task class
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class TaskExtensions
    {
#pragma warning disable AvoidAsyncVoid // Avoid async void
        /// <summary>
        /// Performs a fire-and-forget operation on a given Task
        /// </summary>
        /// <param name="task">Task to await and wrap with a try/catch</param>
        /// <param name="report">
        /// Keep this to TRUE to report any possibly thrown Exceptions to AppCenter
        /// </param>
        /// <param name="crashes">
        /// Keep this as NULL to use the default implementation.
        /// This is only exposed for unit testing purposes
        /// </param>
        public static async void Forget(
            this Task task,
            bool report = true,
            ICrashes crashes = null)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (report)
                    ex.Report(crashes: crashes);
            }
        }
#pragma warning restore AvoidAsyncVoid // Avoid async void

        /// <summary>
        /// Wraps the async operation in a try/catch and reports possible exceptions to AppCenter
        /// </summary>
        /// <param name="inputTask">Task to await and wrap with a try/catch</param>
        /// <param name="crashes">
        /// Keep this as NULL to use the default implementation.
        /// This is only exposed for unit testing purposes
        /// </param>
        /// <returns>Returns a Task as the result of ConfigureAwait(false)</returns>
        public static async Task WhenErrorReportAsync(
            this Task inputTask,
            ICrashes crashes = null)
        {
            try
            {
                await inputTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Report(crashes: crashes);
            }
        }

        /// <summary>
        /// Wraps the async operation in a try/catch and reports possible exceptions to AppCenter
        /// </summary>
        /// <typeparam name="T">Type of the result</typeparam>
        /// <param name="inputTask">Task to await and wrap with a try/catch</param>
        /// <param name="crashes">
        /// Keep this as NULL to use the default implementation.
        /// This is only exposed for unit testing purposes
        /// </param>
        /// <returns>Returns a Task as the result of ConfigureAwait(false)</returns>
        public static async Task<T> WhenErrorReportAsync<T>(
            this Task<T> inputTask,
            ICrashes crashes = null)
            where T : class
        {
            try
            {
                return await inputTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Report(crashes: crashes);
                return null;
            }
        }

        internal static async Task WhenNotNullAsync<TInput>(
            this Task<TInput> inputTask,
            Action<TInput> evaluator)
        {
            if (inputTask == null)
                return;

            var result = await inputTask;
            if (result == null)
                return;

            evaluator(result);
        }
    }
}