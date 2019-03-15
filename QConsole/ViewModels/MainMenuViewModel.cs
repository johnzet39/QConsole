using QConsole.Commands;
using QConsole.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QConsole.ViewModels
{
    class MainMenuViewModel : BaseViewModel
    {

        // About button command.
        private RelayCommand showAboutWindowCommand;
        public RelayCommand ShowAboutWindowCommand
        {
            get
            {
                return showAboutWindowCommand ??
                  (showAboutWindowCommand = new RelayCommand(obj =>
                  {
                      ShowAboutWindowAsync();
                  }));
            }
        }

        // Settings button command.
        private RelayCommand _showSettingsWindowCommand;
        public RelayCommand ShowSettingsWindowCommand
        {
            get
            {
                return _showSettingsWindowCommand ??
                  (_showSettingsWindowCommand = new RelayCommand(obj =>
                  {
                      ShowSettingsWindowAsync();
                  }));
            }
        }

        

        private async void ShowAboutWindowAsync()
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            AboutWindowViewModel vm = new AboutWindowViewModel();
            await displayRootRegistry.ShowModalPresentation(vm);
        }

        private async void ShowSettingsWindowAsync()
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            SettingsWindowViewModel vm = new SettingsWindowViewModel(displayRootRegistry);
            await displayRootRegistry.ShowModalPresentation(vm);
        }

    }
}
