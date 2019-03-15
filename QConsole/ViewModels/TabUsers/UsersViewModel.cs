using System;
using QConsole.Commands;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.Models;
using QConsole.Ext;
using QConsole.BLL.Services;
using QConsole.BLL.Interfaces;
using QConsole.BLL.DTO;
using AutoMapper;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace QConsole.ViewModels.TabUsers
{
    class UsersViewModel : BaseViewModel
    {
        private readonly string _connectionString = Common.ConnectionStrings.ConnectionString;

        // Refresh button command.
        private RelayCommand refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                return refreshCommand ??
                  (refreshCommand = new RelayCommand(obj =>
                  {
                      RefreshTab();
                  }));
            }
        }

        // Grant role button command.
        private RelayCommand _assignRoleCommand;
        public RelayCommand AssignRoleCommand
        {
            get
            {
                return _assignRoleCommand ??
                  (_assignRoleCommand = new RelayCommand(obj =>
                  {
                      GrantRole(obj);
                  }));
            }
        }

        // New role button command.
        private RelayCommand _createNewRoleCommand;
        public RelayCommand CreateNewRoleCommand
        {
            get
            {
                return _createNewRoleCommand ??
                  (_createNewRoleCommand = new RelayCommand(obj =>
                  {
                      AddNewRoleAsync();
                  }));
            }
        }

        // Edit role button command.
        private RelayCommand _editRoleCommand;
        public RelayCommand EditRoleCommand
        {
            get
            {
                return _editRoleCommand ??
                  (_editRoleCommand = new RelayCommand(obj =>
                  {
                      EditRoleAsync();
                  }));
            }
        }

        // Remove role button command.
        private RelayCommand _removeRoleCommand;
        public RelayCommand RemoveRoleCommand
        {
            get
            {
                return _removeRoleCommand ??
                  (_removeRoleCommand = new RelayCommand(obj =>
                  {
                      RemoveRoleAsync();
                  }));
            }
        }
        // Revoke role button command.
        private RelayCommand _revokeRoleCommand;
        public RelayCommand RevokeRoleCommand
        {
            get
            {
                return _revokeRoleCommand ??
                  (_revokeRoleCommand = new RelayCommand(obj =>
                  {
                      RevokeRole(obj);
                  }));
            }
        }

        // Users list for datagrid.
        private ObservableCollection<User> _usersList;
        public ObservableCollection<User> UsersList
        {
            get => _usersList;
            set
            {
                _usersList = value;
                OnPropertyChanged(("UsersList"));
            }
        }

        //Assigned roles for user.
        private DataTable _assignedRoles;
        public DataTable AssignedRoles
        {
            get => _assignedRoles;
            set
            {
                _assignedRoles = value;
                OnPropertyChanged(("AssignedRoles"));
            }
        }

        //Assigned roles for user.
        private DataTable _availableRoles;
        public DataTable AvailableRoles
        {
            get => _availableRoles;
            set
            {
                _availableRoles = value;
                OnPropertyChanged(("AvailableRoles"));
            }
        }


        //Selected User
        private User _selectedUser;
        public User SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;

                if (value != null)
                {
                    IUserService service = new UserService(_connectionString);
                    AssignedRoles = service.GetAssignedRoles(value.Usesysid);
                    AvailableRoles = service.GetAvailableRoles(value.Usesysid);
                }
                else
                {
                    AssignedRoles = null;
                    AvailableRoles = null;
                }

                OnPropertyChanged("SelectedUser");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UsersViewModel()
        {
            Console.WriteLine("users: " + this.GetHashCode());
            GetUsersAsync();
        }

        // Get sessions async
        private async void GetUsersAsync()
        {
            User cur_row = SelectedUser;
            await Task.Run(() => GetUsers());
            //Select selected session in datagrid
            if (cur_row != null)
            {
                SelectedUser = UsersList.Where(p => p.Usesysid == cur_row.Usesysid).DefaultIfEmpty().First();
            }
        }

        // Get sessions async
        private async void GetUsersAsync(string username)
        {
            await Task.Run(() => GetUsers());
            //Select selected session in datagrid
            if (username != null)
            {
                SelectedUser = UsersList.Where(p => p.Usename == username).DefaultIfEmpty().First();
            }
        }

        private void GetUsers()
        {
            IUserService service = new UserService(_connectionString);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()).CreateMapper();
            var sessions = mapper.Map<IEnumerable<UserDTO>, List<User>>(service.GetUsers());
            UsersList = new ObservableCollection<User>(sessions);
        }

        private void GrantRole(object role)
        {
            string rolename = ((DataRowView)role)["groname"].ToString();
            IUserService service = new UserService(_connectionString);
            string sql = service.GrantRole(SelectedUser.Usename, rolename);
            LogPanel.PrintLog(sql);
            RefreshTab();
        }

        private void RevokeRole(object role)
        {
            string rolename = ((DataRowView)role)["rolname"].ToString();
            IUserService service = new UserService(_connectionString);
            string sql = service.RevokeRole(SelectedUser.Usename, rolename);
            LogPanel.PrintLog(sql);
            RefreshTab();
        }

        private void RefreshTab(string newUsername = null)
        {
            if (newUsername != null)
                GetUsersAsync(newUsername);
            else
                GetUsersAsync();
        }

        /// <summary>
        /// Create new user or role button.
        /// </summary>
        private async void AddNewRoleAsync()
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            UserPropertyWindowViewModel vm = new UserPropertyWindowViewModel(displayRootRegistry);
            await displayRootRegistry.ShowModalPresentation(vm);
            if (vm.DialogResult)
                RefreshTab(vm.Username);
        }

        /// <summary>
        /// Edit user or role button.
        /// </summary>
        private async void EditRoleAsync()
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            UserPropertyWindowViewModel vm = new UserPropertyWindowViewModel(SelectedUser, displayRootRegistry);
            await displayRootRegistry.ShowModalPresentation(vm);
            if (vm.DialogResult)
                RefreshTab();
        }

        private void RemoveRoleAsync()
        {
            string Username = SelectedUser.Usename;
            if (MessageBox.Show("Удалить пользователя/роль '" + Username + "'? ",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                IUserService service = new UserService(_connectionString);

                try
                {
                    var result = service.RemoveRoleOrUser(Username);
                    SelectedUser = null;
                    LogPanel.PrintLog(result);
                    RefreshTab();
                }
                catch (Exception ex)
                {
                    LogPanel.PrintLog(ex.Message.ToString());
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        
    }
}
