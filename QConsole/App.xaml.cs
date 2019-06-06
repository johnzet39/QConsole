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
using QConsole.ViewModels.TabLogger;
using QConsole.Views;
using QConsole.Views.TabUsers;
using QConsole.Views.TabLayers;
using QConsole.Views.TabGrants;
using QConsole.Views.TabLogger;

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
            UpdateSettings();

            displayRootRegistry.RegisterWindowType<MainWindowViewModel, MainWindow>();
            displayRootRegistry.RegisterWindowType<AboutWindowViewModel, AboutWindowView>();
            displayRootRegistry.RegisterWindowType<SettingsWindowViewModel, SettingsWindowView>();
            displayRootRegistry.RegisterWindowType<UserPropertyWindowViewModel, UserWindowView>();
            displayRootRegistry.RegisterWindowType<LayerPropertyWindowViewModel, LayerPropertyWindowView>();
            displayRootRegistry.RegisterWindowType<ListDictionariesViewModel, ListDictionariesView>();
            displayRootRegistry.RegisterWindowType<GrantPropertyWindowViewModel, GrantPropertyWindowView>();
            displayRootRegistry.RegisterWindowType<LoggerPropertyWindowViewModel, LoggerPropertyWindowView>();
        }

        /// <summary>
        /// Copy user settings from previous application version if necessary
        /// </summary>
        private void UpdateSettings()
        {
            if (QConsole.Properties.Settings.Default.UpdateSettings)
            {
                QConsole.Properties.Settings.Default.Upgrade();
                QConsole.Properties.Settings.Default.UpdateSettings = false;
                QConsole.Properties.Settings.Default.Save();
            }
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
