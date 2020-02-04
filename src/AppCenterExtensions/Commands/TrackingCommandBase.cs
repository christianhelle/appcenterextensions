using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AppCenterExtensions.Extensions;

namespace AppCenterExtensions.Commands
{
    public abstract class TrackingCommandBase : ITrackingCommand
    {
        private readonly Delegate action;
        private readonly IAnalytics analytics;

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
                    !t.GetTypeInfo().IsAssignableFrom(GetType()));
            return callerType;
        }

        protected abstract bool OnCanExecute(object parameter);
        protected abstract void OnExecute(object parameter);

        public bool CanExecute(object parameter) => OnCanExecute(parameter);

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;
            OnExecute(parameter);
            Report(parameter);
        }

        public string EventName { get; }
        public string ScreenName { get; }
        public Dictionary<string, string> Properties { get; }

        public event EventHandler CanExecuteChanged;

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