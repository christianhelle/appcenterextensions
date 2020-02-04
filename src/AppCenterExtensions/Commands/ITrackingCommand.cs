using System.Collections.Generic;
using System.Windows.Input;

namespace AppCenterExtensions.Commands
{
    public interface ITrackingCommand : ICommand
    {
        string EventName { get; }
        string ScreenName { get; }
        Dictionary<string, string> Properties { get; }
        void RaiseCanExecuteChanged();
    }
}
