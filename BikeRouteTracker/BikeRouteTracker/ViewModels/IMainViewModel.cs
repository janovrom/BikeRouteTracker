using System.Windows.Input;

namespace BikeRouteTracker.ViewModels
{
    public enum MainViewModelState
    {
        Stopped,
        Starting,
        Running,
        Paused,
    }

    internal interface IMainViewModel
    {
        int SpeedKph { get; }
        double Progress { get; }
        MainViewModelState State { get; }
        ICommand StopCommand { get; }
        ICommand StartCommand { get; }
        ICommand PauseCommand { get; }
        ICommand ResumeCommand { get; }
        string CountdownText { get; set; }
    }
}
