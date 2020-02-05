using System;
using System.Diagnostics;

namespace AppCenterExtensions.Extensions
{
    /// <summary>
    /// Exposes extension methods for the Action class
    /// </summary>
    public static class ActionExtensions
    {
        /// <summary>
        /// Safely invoke an Action and swallow the result
        /// or provide a callback method that will be invoked
        /// in the case of an exception being thrown
        /// </summary>
        /// <param name="action">Callback method</param>
        /// <param name="onError">Callback method invoked in the case of an exception being thrown</param>
        public static void SafeInvoke(
            this Action action, 
            Action<Exception> onError = null)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                onError?.Invoke(e);
            }
        }
    }
}
