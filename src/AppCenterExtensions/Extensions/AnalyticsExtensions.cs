using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions
{
    public static class AnalyticsExtensions
    {
        public static string TrimForAnalytics(this string text)
            => text
            .Replace("Async", string.Empty)
            .Replace("ViewModel", string.Empty);

        public static Dictionary<string, string> OptimizeForAnalytics(
            this IDictionary<string, string> properties)
            => properties?.ToDictionary(
                property => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(property.Key),
                property => property.Value);
    }
}
