using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions;
using Microsoft.AppCenter.Analytics;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Command
{
    public class AsyncTrackingCommand : ICommand
    {
        private readonly WeakFunc<Task> executeFunc;
        private readonly WeakFunc<bool> canExecute;

        public AsyncTrackingCommand(
            Func<Task> func,
            string eventName,
            string screenName,
            Func<bool> canExecute = null,
            Dictionary<string, string> properties = null,
            bool keepTargetAlive = false)
        {
            if (func == null) 
                throw new ArgumentNullException(nameof(func));

            if (string.IsNullOrWhiteSpace(eventName))
                throw new ArgumentNullException(nameof(eventName));

            if (string.IsNullOrWhiteSpace(screenName))
                throw new ArgumentNullException(nameof(screenName));
            
            executeFunc = new WeakFunc<Task>(func, keepTargetAlive);
            
            if (canExecute != null) 
                this.canExecute = new WeakFunc<bool>(canExecute, keepTargetAlive);

            EventName = eventName;
            ScreenName = screenName;
            Properties = properties ?? new Dictionary<string, string>();
        }

        public string EventName { get; }
        public string ScreenName { get; }
        public Dictionary<string, string> Properties { get; }

        public Task CompletionTask { get; private set; } = Task.CompletedTask;
        
        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) 
            => canExecute == null || 
               (canExecute.IsStatic || canExecute.IsAlive) && canExecute.Execute();

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            if (executeFunc == null || !executeFunc.IsStatic && !executeFunc.IsAlive)
                return;

            CompletionTask = executeFunc.Execute();
            
            Properties[nameof(EventName)] = EventName;
            Properties[nameof(ScreenName)] = ScreenName;

            if (executeFunc.Target != null)
                Properties["Target"] = executeFunc.Target.GetType().Name;

            if (parameter != null)
            {
                string parameterType = parameter.GetType().Name;
                Properties["Parameter"] = parameterType;
                foreach (var item in parameter.ToDictionary())
                    Properties[$"{parameterType}-{item.Key}"] = item.Value ?? string.Empty;
            }

            Analytics.TrackEvent(
                EventName,
                Properties);
        }
    }
}
