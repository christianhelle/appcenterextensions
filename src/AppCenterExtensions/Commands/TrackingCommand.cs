using System;
using System.Collections.Generic;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Commands
{
    public class TrackingCommand : ITrackingCommand
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

            this.Report(action, parameter, analytics);
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
