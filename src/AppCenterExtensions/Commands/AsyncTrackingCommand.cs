using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppCenterExtensions.Extensions;

namespace AppCenterExtensions.Commands
{
    /// <summary>
    /// Tracking ICommand implementation that uses an async function as the callback
    /// </summary>
    public class AsyncTrackingCommand : TrackingCommandBase
    {
        private readonly Func<Task> executeFunc;
        private readonly Func<bool> canExecute;

        /// <summary>
        /// Creates an instance of AsyncTrackingCommand
        /// </summary>
        /// <param name="executeFunc">Async callback function</param>
        /// <param name="canExecute">Callback function invoked to check if OnExecute() can be invoked</param>
        /// <param name="eventName">Event name that describes what the command is used for (ex: Logout Button Tapped)</param>
        /// <param name="screenName">Screen or page name that hosts the user interaction component (ex: Settings)</param>
        /// <param name="properties">Custom properties to attach to the analytics data (ex: {"Current Item",CurrentItem.Name})</param>
        /// <param name="analytics">Keep this as NULL to use the default implementation. This is exposed for unit testing purposes</param>
        public AsyncTrackingCommand(
            Func<Task> executeFunc,
            string eventName,
            string screenName = null,
            Func<bool> canExecute = null,
            Dictionary<string, string> properties = null,
            IAnalytics analytics = null)
            : base(executeFunc, eventName, screenName, properties, analytics)
        {
            this.executeFunc = executeFunc ?? throw new ArgumentNullException(nameof(executeFunc));
            this.canExecute = canExecute;
        }

        /// <summary>
        /// An awaitable task that is set to the result of the async callback function invoked by Execute()
        /// </summary>
        public Task CompletionTask { get; private set; }

        /// <summary>
        /// This method is invoked upon CanExecute()
        /// </summary>
        /// <param name="parameter">Parameter specified by ICommand.CanExecute()</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        protected override bool OnCanExecute(object parameter)
            => canExecute?.Invoke() ?? true;

        /// <summary>
        /// This method is invoked upon Execute()
        /// </summary>
        /// <param name="parameter"></param>
        protected override void OnExecute(object parameter) 
        {
            CompletionTask = executeFunc.Invoke();
            CompletionTask.Forget();
        }
    }
}