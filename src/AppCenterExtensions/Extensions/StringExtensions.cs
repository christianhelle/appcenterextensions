using System.Linq;
using System.Text.RegularExpressions;

namespace AppCenterExtensions.Extensions
{
    /// <summary>
    /// Exposes extension methods for the string class
    /// </summary>
    public static class StringExtension
    {
        private static readonly string[] TrimNames =
        {
            "Command", "View", "Model", "Async"
        };

        /// <summary>
        /// Splits the pascal cased text into words separated by whitespaces
        /// then removes instances of Command, View, Model, and Async.
        /// For example:
        ///     UserSettingsPage => User Settings
        ///     AdvancedSettingsViewModel => Advanced Settings
        /// </summary>
        /// <param name="input">Any string but usually taken from the nameof() some type</param>
        /// <returns>Returns a string that represents a event name to be used for AppCenter Analytics</returns>
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

            return str.Replace("  ", " ").Trim();
        }
    }
}