using System;
using System.Collections.Generic;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Commands
{
    public class TrackingCommand<T> : TrackingCommandBase
    {
        private readonly Action<T> action;
        private readonly Func<T, bool> canExecute;

        public TrackingCommand(
            Action<T> action,
            string eventName,
            string screenName = null,
            Func<T, bool> canExecute = null,
            Dictionary<string, string> properties = null,
            IAnalytics analytics = null)
            : base(action, eventName, screenName, properties, analytics)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.canExecute = canExecute;
        }

        protected override bool OnCanExecute(object parameter)
            => canExecute?.Invoke((T)parameter) ?? true;

        protected override void OnExecute(object parameter) 
            => action.Invoke((T)parameter);
    }
}