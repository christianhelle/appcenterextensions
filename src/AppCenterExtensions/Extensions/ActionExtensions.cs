using System;
using System.Diagnostics;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions
{
    public static class ActionExtensions
    {
        public static void SafeInvoke(this Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
