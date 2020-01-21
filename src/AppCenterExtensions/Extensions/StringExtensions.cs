using System.Text.RegularExpressions;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions
{
    public static class StringExtension
    {
        public static string ToTrackingEventName(this string input)
            => Regex
                .Replace(
                    input,
                    "([A-Z])",
                    " $1",
                    RegexOptions.Compiled)
                .Replace("Command", string.Empty)
                .Replace("View", string.Empty)
                .Replace("Model", string.Empty)
                .Replace("Page", string.Empty)
                .Replace("Async", string.Empty)
                .Trim();
    }
}