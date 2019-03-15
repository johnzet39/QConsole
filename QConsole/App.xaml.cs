using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using QConsole.ViewModels;
using QConsole.ViewModels.TabUsers;
using QConsole.ViewModels.TabLayers;
using QConsole.ViewModels.TabGrants;
using QConsole.Views;
using QConsole.Views.TabUsers;
using QConsole.Views.TabLayers;
using QConsole.Views.TabGrants;

namespace QConsole
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();
        //MainWindowViewModel mainWindowViewModel;

        public App()
        {
            displayRootRegistry.RegisterWindowType<MainWindowViewModel, MainWindow>();
            displayRootRegistry.RegisterWindowType<AboutWindowViewModel, AboutWindowView>();
            displayRootRegistry.RegisterWindowType<SettingsWindowViewModel, SettingsWindowView>();
            displayRootRegistry.RegisterWindowType<UserPropertyWindowViewModel, UserWindowView>();
            displayRootRegistry.RegisterWindowType<LayerPropertyWindowViewModel, LayerPropertyWindowView>();
            displayRootRegistry.RegisterWindowType<GrantPropertyWindowViewModel, GrantPropertyWindowView>();
        }

        /// <summary>
        /// Загрузка окна, если приложение начинается с этого окна.
        /// </summary>
        //protected override async void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);

        //    mainWindowViewModel = new MainWindowViewModel();

        //    await displayRootRegistry.ShowModalPresentation(mainWindowViewModel);

        //    Shutdown();
        //}
    }
}
