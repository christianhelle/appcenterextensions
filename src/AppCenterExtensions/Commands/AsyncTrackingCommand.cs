using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Commands
{
    public class AsyncTrackingCommand : ITrackingCommand
    {
        private readonly Func<Task> executeFunc;
        private readonly Func<bool> canExecute;
        private readonly IAnalytics analytics;

        public AsyncTrackingCommand(
            Func<Task> executeFunc,
            string eventName,
            string screenName,
            Func<bool> canExecute = null,
            Dictionary<string, string> properties = null,
            IAnalytics analytics = null)
        {
            this.executeFunc = executeFunc ?? throw new ArgumentNullException(nameof(executeFunc));

            if (string.IsNullOrWhiteSpace(eventName))
                throw new ArgumentNullException(nameof(eventName));

            if (string.IsNullOrWhiteSpace(screenName))
                throw new ArgumentNullException(nameof(screenName));
                        
            if (canExecute != null) 
                this.canExecute = new Func<bool>(canExecute);

            EventName = eventName;
            ScreenName = screenName;
            Properties = properties ?? new Dictionary<string, string>();

            this.analytics = analytics ?? AppCenterAnalytics.Instance;
        }

        public string EventName { get; }
        public string ScreenName { get; }
        public Dictionary<string, string> Properties { get; }

        public Task CompletionTask { get; private set; } = Task.CompletedTask;
        
        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) 
            => canExecute?.Invoke() ?? true;

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            (CompletionTask = executeFunc.Invoke()).Forget();
            
            Properties[nameof(EventName)] = EventName;
            Properties[nameof(ScreenName)] = ScreenName;

            if (executeFunc.Target != null)
                Properties["Target"] = executeFunc.Target.GetType().Name;

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
    }
}
