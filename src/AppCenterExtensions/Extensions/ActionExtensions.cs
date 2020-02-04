using System;
using System.Diagnostics;

namespace AppCenterExtensions.Extensions
{
    public static class ActionExtensions
    {
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
