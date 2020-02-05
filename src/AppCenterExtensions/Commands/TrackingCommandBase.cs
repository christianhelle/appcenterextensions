using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AppCenterExtensions.Extensions;

namespace AppCenterExtensions.Commands
{
    /// <summary>
    /// Base Tracking ICommand implementation that sends analytics data to AppCenter upon Execute()
    /// </summary>
    public abstract class TrackingCommandBase : ITrackingCommand
    {
        private readonly Delegate action;
        private readonly IAnalytics analytics;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="action">The callback to execute upon Execute()</param>
        /// <param name="eventName">Event name that describes what the command is used for (ex: Logout Button Tapped)</param>
        /// <param name="screenName">Screen or page name that hosts the user interaction component (ex: Settings)</param>
        /// <param name="properties">Custom properties to attach to the analytics data (ex: {"Current Item",CurrentItem.Name})</param>
        /// <param name="analytics">Keep this as NULL to use the default implementation. This is exposed for unit testing purposes</param>
        protected TrackingCommandBase(
            Delegate action,
            string eventName,
            string screenName,
            Dictionary<string, string> properties = null,
            IAnalytics analytics = null)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
            ScreenName = screenName ?? GetCallerType().Name.ToTrackingEventName();

            Properties = properties ?? new Dictionary<string, string>();
            this.analytics = analytics ?? AppCenterAnalytics.Instance;
        }

        private Type GetCallerType()
        {
            var stackTrace = new StackTrace();
            var stackFrames = stackTrace.GetFrames();
            var declaringTypes = stackFrames?.Select(frame => frame?.GetMethod()?.DeclaringType)?.ToList();
            var callerType = declaringTypes?.FirstOrDefault(
                t =>
                    t != GetType() &&
                    t.BaseType != typeof(TrackingCommandBase) &&
                    !t.IsInstanceOfType(this) &&
                    !t.IsAssignableFrom(typeof(TrackingCommandBase)));
            return callerType;
        }

        /// <summary>
        /// This method is invoked upon CanExecute()
        /// </summary>
        /// <param name="parameter">Parameter specified by ICommand.CanExecute()</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        protected abstract bool OnCanExecute(object parameter);

        /// <summary>
        /// This method is invoked upon Execute()
        /// </summary>
        /// <param name="parameter"></param>
        protected abstract void OnExecute(object parameter);

        /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter) => OnCanExecute(parameter);

        /// <summary>Defines the method to be called when the command is invoked.</summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;
            OnExecute(parameter);
            Report(parameter);
        }

        /// <summary>
        /// Event name that describes what the command is used for (ex: Logout Button Tapped)
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// Screen or page name that hosts the user interaction component (ex: Settings)
        /// </summary>
        public string ScreenName { get; }

        /// <summary>
        /// Custom properties to attach to the analytics data (ex: {"Current Item",CurrentItem.Name})
        /// </summary>
        public Dictionary<string, string> Properties { get; }

        /// <summary>Occurs when changes occur that affect whether or not the command should execute.</summary>
        /// <returns></returns>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Raise the CanExecuteChanged event
        /// </summary>
        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        private void Report(object parameter)
        {
            Properties[nameof(EventName)] = EventName;
            Properties[nameof(ScreenName)] = ScreenName;

            if (action.Target != null)
                Properties["Target"] = action.Target.GetType().Name;

            if (parameter != null)
            {
                var parameterType = parameter.GetType().Name;
                Properties["Parameter"] = parameterType;
                foreach (var item in parameter.ToDictionary())
                    Properties[$"{parameterType}-{item.Key}"] = item.Value ?? string.Empty;
            }

            analytics.TrackEvent(EventName, Properties);
        }
    }
}