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

        // Refresh button command.
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

        private async void ShowAboutWindowAsync()
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            AboutWindowViewModel vm = new AboutWindowViewModel();
            await displayRootRegistry.ShowModalPresentation(vm);
        }

    }
}
