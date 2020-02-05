using System;
using System.Collections.Generic;

namespace AppCenterExtensions.Commands
{
    /// <summary>
    /// Tracking ICommand implementation that uses an Action with a parameter as the callback
    /// </summary>
    public class TrackingCommand<T> : TrackingCommandBase
    {
        private readonly Action<T> action;
        private readonly Func<T, bool> canExecute;

        /// <summary>
        /// Creates an instance of TrackingCommand
        /// </summary>
        /// <param name="action">Callback function</param>
        /// <param name="canExecute">Callback function invoked to check if OnExecute() can be invoked</param>
        /// <param name="eventName">Event name that describes what the command is used for (ex: Logout Button Tapped)</param>
        /// <param name="screenName">Screen or page name that hosts the user interaction component (ex: Settings)</param>
        /// <param name="properties">Custom properties to attach to the analytics data (ex: {"Current Item",CurrentItem.Name})</param>
        /// <param name="analytics">Keep this as NULL to use the default implementation. This is exposed for unit testing purposes</param>
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

        /// <summary>
        /// This method is invoked upon CanExecute()
        /// </summary>
        /// <param name="parameter">Parameter specified by ICommand.CanExecute()</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        protected override bool OnCanExecute(object parameter)
            => canExecute?.Invoke((T)parameter) ?? true;

        /// <summary>
        /// This method is invoked upon Execute()
        /// </summary>
        /// <param name="parameter"></param>
        protected override void OnExecute(object parameter) 
            => action.Invoke((T)parameter);
    }
}