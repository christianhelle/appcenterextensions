using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

#pragma warning disable AvoidAsyncVoid // Avoid async void

namespace AppCenterExtensions.Extensions
{
    /// <summary>
    /// Exposes extension methods for the Task class
    /// </summary>
    public static class TaskExtensions
    {
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
                if (task != null)
                    await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (report)
                    ex.Report(crashes: crashes);
            }
        }
    }
}

#pragma warning restore AvoidAsyncVoid // Avoid async void