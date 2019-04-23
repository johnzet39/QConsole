using QConsole.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QConsole.ViewModels
{
    class MainMenuItemViewModel
    {
        public MainMenuItemViewModel()
        {
        }

        // OK button command.
        private RelayCommand _itemCommand;
        public RelayCommand ItemCommand
        {
            get
            {
                return _itemCommand ??
                  (_itemCommand = new RelayCommand(obj =>
                  {
                      ExecCommand(obj);
                  }));
            }
        }

        private void ExecCommand(object obj)
        {
            MessageBox.Show(Header);
        }

        public string Header { get; set; }

        public ObservableCollection<MainMenuItemViewModel> ConfigurationMenuItems { get; set; }
    }
}
