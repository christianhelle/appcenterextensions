using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions
{
    public static class StringExtension
    {
        public static List<string> TrimNames { get; }
            = new List<string>
            {
                "Command", "View", "Model", "Async"
            };

        public static string ToTrackingEventName(this string input)
        {
            var str = Regex
                .Replace(
                    input,
                    "([A-Z])",
                    " $1",
                    RegexOptions.Compiled);

            str = TrimNames.Aggregate(
                str,
                (current, trimName) => current.Replace(
                    trimName, string.Empty));

            return str.Trim();
        }
    }
}