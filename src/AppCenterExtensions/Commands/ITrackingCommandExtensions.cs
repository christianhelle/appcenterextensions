using System;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Commands
{
    public static class ITrackingCommandExtensions
    {
        public static void Report(
            this ITrackingCommand command,
            Delegate executeCallback,
            object parameter,
            IAnalytics analytics)
        {
            command.Properties[nameof(ITrackingCommand.EventName)] = command.EventName;
            command.Properties[nameof(ITrackingCommand.ScreenName)] = command.ScreenName;

            if (executeCallback.Target != null)
                command.Properties["Target"] = executeCallback.Target.GetType().Name;

            if (parameter != null)
            {
                var parameterType = parameter.GetType().Name;
                command.Properties["Parameter"] = parameterType;
                foreach (var item in parameter.ToDictionary())
                    command.Properties[$"{parameterType}-{item.Key}"] = item.Value ?? string.Empty;
            }

            analytics.TrackEvent(
                command.EventName,
                command.Properties);
        }
    }
}