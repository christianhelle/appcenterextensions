﻿using System;
using System.Collections.Generic;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Commands
{
    public class TrackingCommand : TrackingCommandBase
    {
        private readonly Action action;
        private readonly Func<bool> canExecute;

        public TrackingCommand(
            Action action,
            string eventName,
            string screenName,
            Func<bool> canExecute = null,
            Dictionary<string, string> properties = null,
            IAnalytics analytics = null)
            : base(action, eventName, screenName, properties, analytics)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.canExecute = canExecute;
        }

        protected override bool OnCanExecute(object parameter)
            => canExecute?.Invoke() ?? true;

        protected override void OnExecute(object parameter) 
            => action.Invoke();
    }
}
