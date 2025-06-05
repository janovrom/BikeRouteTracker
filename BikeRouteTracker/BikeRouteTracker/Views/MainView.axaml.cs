using Avalonia.Controls;
using Avalonia.Interactivity;

namespace BikeRouteTracker.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            Avalonia.Controls.Platform.IInsetsManager? insetsManager = TopLevel.GetTopLevel(this)?.InsetsManager;
            if (insetsManager is not null)
            {
                insetsManager.DisplayEdgeToEdge = true;
                insetsManager.IsSystemBarVisible = false;
            }
        }
    }
}