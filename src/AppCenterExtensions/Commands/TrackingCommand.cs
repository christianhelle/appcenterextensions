using System;
using System.Collections.Generic;
using System.Windows.Input;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions;
using Microsoft.AppCenter.Analytics;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Command
{
    public class TrackingCommand
        : ICommand
    {
        private readonly Action action;
        private readonly Func<bool> canExecute;
        private readonly IAnalytics analytics;

        public TrackingCommand(
            Action action,
            string eventName,
            string screenName,
            Func<bool> canExecute = null,
            Dictionary<string, string> properties = null,
            IAnalytics analytics = null)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));

            if (string.IsNullOrWhiteSpace(eventName))
                throw new ArgumentNullException(nameof(eventName));

            if (string.IsNullOrWhiteSpace(screenName))
                throw new ArgumentNullException(nameof(screenName));

            this.canExecute = canExecute;
            this.analytics = analytics ?? AppCenterAnalytics.Instance;

            EventName = eventName;
            ScreenName = screenName;
            Properties = properties ?? new Dictionary<string, string>();
        }

        public string EventName { get; }
        public string ScreenName { get; }
        public Dictionary<string, string> Properties { get; }

        public bool CanExecute(object parameter) 
            => canExecute?.Invoke() ?? true;

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;
            
            action();
            
            Properties[nameof(EventName)] = EventName;
            Properties[nameof(ScreenName)] = ScreenName;

            if (action.Target != null)
                Properties["Target"] = action.Target.GetType().Name;

            if (parameter != null)
            {
                var parameterType = parameter.GetType().Name;
                Properties["Parameter"] = parameterType;
                foreach (var (key, value) in parameter.ToDictionary())
                    Properties[$"{parameterType}-{key}"] = value ?? string.Empty;
            }

            analytics.TrackEvent(
                EventName,
                Properties);
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
