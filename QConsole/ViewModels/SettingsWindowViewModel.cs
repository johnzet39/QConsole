using QConsole.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.ViewModels
{
    class SettingsWindowViewModel : BaseViewModel
    {
        public bool DialogResult = false;
        DisplayRootRegistry DisplayRootRegistry;


        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsWindowViewModel(DisplayRootRegistry displayRootRegistry)
        {
            DisplayRootRegistry = displayRootRegistry;
            IsCheckUpdates = Properties.Settings.Default.IsCheckUpdates;
            ConfigurationsPostgreSQL = Properties.Settings.Default.ConfigurationsPostgreSQL;
        }



        // OK button command.
        private RelayCommand _okCommand;
        public RelayCommand OkCommand
        {
            get
            {
                return _okCommand ??
                  (_okCommand = new RelayCommand(obj =>
                  {
                      OkButton(obj);
                  }));
            }
        }

        private bool _isCheckUpdates;
        public bool IsCheckUpdates
        {
            get => _isCheckUpdates;
            set
            {
                _isCheckUpdates = value;
                OnPropertyChanged(("IsCheckUpdates"));
            }
        }

        private string _configurationsPostgreSQL;
        public string ConfigurationsPostgreSQL
        {
            get => _configurationsPostgreSQL;
            set
            {
                _configurationsPostgreSQL = value.Replace("\"", "").Replace(";", "").Replace(",", "");
                OnPropertyChanged(("ConfigurationsPostgreSQL"));
            }
        }

        

        private void OkButton(object parameter)
        {
            Properties.Settings.Default.IsCheckUpdates = IsCheckUpdates;
            Properties.Settings.Default.ConfigurationsPostgreSQL = ConfigurationsPostgreSQL;
            Properties.Settings.Default.Save();

            DialogResult = true;
            this.CloseWindow();
        }

        private void CloseWindow()
        {
            DisplayRootRegistry.HidePresentation(this);
        }
    }
}
