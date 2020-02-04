using System;
using System.Collections.Generic;
using System.Globalization;
using AppCenterExtensions.Extensions;
using Page = Xamarin.Forms.Page;

namespace AppCenterExtensions.XamarinForms
{
    public static class PageExtensions
    {
        public static void TrackPage(this Page page, TimeSpan duration, IAnalytics analytics = null)
        {
            var properties = new Dictionary<string, string>
            {
                [nameof(Page.Title)] = page.Title,
                ["Duration"] = duration.TotalSeconds.ToString(CultureInfo.InvariantCulture)
            };

            (analytics ?? AppCenterAnalytics.Instance)
                .TrackEvent(
                    page.GetType().Name.ToTrackingEventName(),
                    properties);
        }
    }
}