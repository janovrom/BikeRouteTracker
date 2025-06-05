using ReactiveUI;
using System.Windows.Input;

namespace BikeRouteTracker.ViewModels
{
    public class DesignMainViewModel : ViewModelBase, IMainViewModel
    {
        public int SpeedKph { get; set; } = 25;
        public double Progress { get; set; } = 0.5;

        public ICommand StopCommand { get; init; }
        public ICommand StartCommand { get; init; }
        public ICommand PauseCommand { get; init; } 
        public ICommand ResumeCommand { get; init; }

        public MainViewModelState State { get; set; } = MainViewModelState.Stopped;
        public string CountdownText { get; set; } = "3";

        public DesignMainViewModel()
        {
            StopCommand = ReactiveCommand.Create(() => { });
            StartCommand = ReactiveCommand.Create(() => { });
            PauseCommand = ReactiveCommand.Create(() => { });
            ResumeCommand = ReactiveCommand.Create(() => { });
        }
    }
}
