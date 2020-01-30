using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Commands
{
    public class AsyncTrackingCommand : TrackingCommandBase
    {
        private readonly Func<Task> executeFunc;
        private readonly Func<bool> canExecute;

        public AsyncTrackingCommand(
            Func<Task> executeFunc,
            string eventName,
            string screenName,
            Func<bool> canExecute = null,
            Dictionary<string, string> properties = null,
            IAnalytics analytics = null)
            : base(executeFunc, eventName, screenName, properties, analytics)
        {
            this.executeFunc = executeFunc ?? throw new ArgumentNullException(nameof(executeFunc));
            this.canExecute = canExecute;
        }

        public Task CompletionTask { get; private set; }

        protected override bool OnCanExecute(object parameter)
            => canExecute?.Invoke() ?? true;

        protected override void OnExecute(object parameter) 
        {
            CompletionTask = executeFunc.Invoke();
            CompletionTask.Forget();
        }
    }
}