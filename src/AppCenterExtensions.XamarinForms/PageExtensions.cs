using System;
using System.Collections.Generic;
using System.Globalization;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions;
using Page = Xamarin.Forms.Page;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.XamarinForms
{
    public static class PageExtensions
    {
        public static void TrackPage(this Page page, TimeSpan duration, IAnalytics analytics = null)
        {
            var properties = page.BindingContext?.ToDictionary() ?? new Dictionary<string, string>();            
            properties["Duration"] = duration.TotalSeconds.ToString(CultureInfo.InvariantCulture);

            (analytics ?? AppCenterAnalytics.Instance)
                .TrackEvent(
                    page.GetType().Name.ToTrackingEventName(),
                    properties);
        }
    }
}