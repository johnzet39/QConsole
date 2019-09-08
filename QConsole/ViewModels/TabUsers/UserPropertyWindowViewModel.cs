using QConsole.Commands;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using QConsole.Models;
using QConsole.BLL.Services;
using QConsole.BLL.Interfaces;
using System.Windows;
using System.Text.RegularExpressions;

namespace QConsole.ViewModels.TabUsers
{
    class UserPropertyWindowViewModel : BaseViewModel, IDataErrorInfo
    {
        IUserService userService;
        private readonly string _connectionString = Common.ConnectionStrings.ConnectionString;
        DisplayRootRegistry DisplayRootRegistry;
        private string _oldDescription;
        public bool IsNew { get; set; }

        public bool DialogResult = false;

        // Grant role button command.
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

        //Username
        private string  _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(("Username"));
            }
        }

        //Description
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(("Description"));
            }
        }

        //IsRole
        private bool _isRole;
        public bool IsRole
        {
            get => _isRole;
            set
            {
                _isRole = value;
                OnPropertyChanged(("IsRole"));
            }
        }

        /// <summary>
        /// Costructor for New role mode.
        /// </summary>
        public UserPropertyWindowViewModel(DisplayRootRegistry displayRootRegistry)
        {
            DisplayRootRegistry = displayRootRegistry;

            IsNew = true;
        }

        /// <summary>
        /// Costructor for Edit mode.
        /// </summary>
        public UserPropertyWindowViewModel(User currentUser, DisplayRootRegistry displayRootRegistry)
        {
            DisplayRootRegistry = displayRootRegistry;

            Username = currentUser.Usename;
            Description = currentUser.Descript;
            _oldDescription = Description;
            IsRole = currentUser.Isrole;

            IsNew = false;
        }

        private void OkButton(object parameter)
        {
            #region Check passwords

            string Password = null;
            var parameters = (object[])parameter;
            var PasswordBox = parameters[0] as PasswordBox;
            var PasswordBoxRepeat = parameters[1] as PasswordBox;
            if (PasswordBox.Password != PasswordBoxRepeat.Password)
            {
                Ext.UIHelper.ShowToolTip("Пароли не совпадают", PasswordBoxRepeat, 5);
                return;
            }
            if (IsNew)
                if (!IsRole)
                {
                    if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                    {
                        Ext.UIHelper.ShowToolTip("Введите пароль", PasswordBox, 5);
                        return;
                    }
                }
            Password = PasswordBox.Password;

            #endregion

            if (IsNew)
            {
                CreateNewRole(Password);
            }
            else
                EditCurrentRole(Password);
        }

        private void CloseWindow()
        {
            DisplayRootRegistry.HidePresentation(this);
        }

        private void CreateNewRole(string password)
        {
            userService = new UserService(_connectionString);
            try
            {
                if (IsRole)
                    userService.CreateUserOrRole(Username, null, Description ?? String.Empty);
                else
                    userService.CreateUserOrRole(Username, password, Description ?? String.Empty);
            }
            catch (Exception ex)
            {
                Ext.LogPanel.PrintLog(ex.Message.ToString());
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
            this.CloseWindow();
        }

        private void EditCurrentRole(string password)
        {
            userService = new UserService(_connectionString);

            string DescriptionResult = null;
            if (_oldDescription != Description)
            {
                DescriptionResult = Description;
            }
            try
            {
                userService.EditUserOrRole(Username, password, DescriptionResult);
            }
            catch (Exception ex)
            {
                Ext.LogPanel.PrintLog(ex.Message.ToString());
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
            this.CloseWindow();
        }


        #region IDataErrorInfo User

        string IDataErrorInfo.Error
        {
            get
            {
                return null;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                IsValid = GetValidStatus();
                return GetValidationError(propertyName);
            }
        }

        #endregion

        #region Validation

        static readonly string[] ValidatedProperties =
        {
            "Username"
        };

        private bool _isvalid;
        public bool IsValid
        {
            get => _isvalid;
            set
            {
                _isvalid = value;
                OnPropertyChanged(("IsValid"));
            }
        }

        private bool GetValidStatus()
        {
            foreach (string property in ValidatedProperties)
                if (GetValidationError(property) != null)
                    return false;

            return true;
        }

        string GetValidationError(string propertyName)
        {
            string error = null;

            switch (propertyName)
            {
                case "Username":
                    error = ValidateUsername();
                    break;
            }

            return error;
        }

        private string ValidateUsername()
        {
            if (Username == String.Empty)
            {
                return "Имя роли или пользователя не может быть пустым.";
            }
            if (Username == null)
            {
                return "";
            }
            return null;
        }

        #endregion

    }
}
