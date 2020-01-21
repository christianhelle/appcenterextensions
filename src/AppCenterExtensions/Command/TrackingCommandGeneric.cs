using System;
using System.Collections.Generic;
using System.Windows.Input;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions;
using Microsoft.AppCenter.Analytics;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Command
{

    public class TrackingCommand<T> : ICommand
    {
        private readonly Action<T> action;
        private readonly Func<T, bool> canExecute;

        public TrackingCommand(
            Action<T> action,
            string eventName,
            string screenName,
            Func<T, bool> canExecute = null,
            Dictionary<string, string> properties = null)
        {
            if (string.IsNullOrWhiteSpace(eventName))
                throw new ArgumentNullException(nameof(eventName));

            if (string.IsNullOrWhiteSpace(screenName))
                throw new ArgumentNullException(nameof(screenName));

            this.action = action;
            this.canExecute = canExecute;

            EventName = eventName;
            ScreenName = screenName;
            Properties = properties ?? new Dictionary<string, string>();
        }

        public string EventName { get; }
        public string ScreenName { get; }
        public Dictionary<string, string> Properties { get; }

        public bool CanExecute(object parameter) 
            => canExecute?.Invoke((T) parameter) ?? true;

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            action((T) parameter);

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

            Analytics.TrackEvent(
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