using System.Collections.Generic;
using System.Windows.Input;

namespace AppCenterExtensions.Commands
{
    /// <summary>
    /// An ICommand interface designed for Analytics
    /// </summary>
    public interface ITrackingCommand : ICommand
    {
        /// <summary>
        /// Event name that describes what the command is used for (ex: Logout Button Tapped)
        /// </summary>
        string EventName { get; }
        /// <summary>
        /// Screen or page name that hosts the user interaction component (ex: Settings)
        /// </summary>
        string ScreenName { get; }
        /// <summary>
        /// Custom properties to attach to the analytics data (ex: {"Current Item",CurrentItem.Name})
        /// </summary>
        Dictionary<string, string> Properties { get; }
        /// <summary>
        /// Raise the CanExecuteChanged event
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}
