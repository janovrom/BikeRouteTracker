using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BikeRouteTracker.ViewModels;
using BikeRouteTracker.Views;
using Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace BikeRouteTracker
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void RegisterServices()
        {
            base.RegisterServices();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            ServiceCollection serviceCollection = new();
            serviceCollection.UseMicrosoftDependencyResolver();
            serviceCollection.AddServices();

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.UseMicrosoftDependencyResolver();

            MainViewModel mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainViewModel
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = mainViewModel
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}