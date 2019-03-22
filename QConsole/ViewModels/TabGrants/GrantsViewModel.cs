using System;
using QConsole.Commands;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.Models;
using QConsole.ViewModels.TabUsers;
using QConsole.Ext;
using QConsole.BLL.Services;
using QConsole.BLL.Interfaces;
using QConsole.BLL.DTO;
using AutoMapper;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace QConsole.ViewModels.TabGrants
{
    class GrantsViewModel : BaseViewModel
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

        // Show property command.
        private RelayCommand _showPropertyRoleCommand;
        public RelayCommand ShowPropertyRoleCommand
        {
            get
            {
                return _showPropertyRoleCommand ??
                  (_showPropertyRoleCommand = new RelayCommand(obj =>
                  {
                      ShowPropertyRoleAsync(obj);
                  }));
            }
        }

        // Show property command.
        private RelayCommand _showPropertyGrantCommand;
        public RelayCommand ShowPropertyGrantCommand
        {
            get
            {
                return _showPropertyGrantCommand ??
                  (_showPropertyGrantCommand = new RelayCommand(obj =>
                  {
                      ShowPropertyGrantAsync(obj);
                  }));
            }
        }


        // groups list for datagrid.
        private ObservableCollection<User> _groupsList;
        public ObservableCollection<User> GroupsList
        {
            get => _groupsList;
            set
            {
                _groupsList = value;
                OnPropertyChanged(("GroupsList"));
            }
        }

        // users list for datagrid.
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

        // grantlayers list for datagrid.
        private ObservableCollection<Grant> _grantLayersList;
        public ObservableCollection<Grant> GrantLayersList
        {
            get => _grantLayersList;
            set
            {
                _grantLayersList = value;
                OnPropertyChanged(("GrantLayersList"));
            }
        }

        // grantlayers list for datagrid.
        private ObservableCollection<Grant> _grantDictsList;
        public ObservableCollection<Grant> GrantDictsList
        {
            get => _grantDictsList;
            set
            {
                _grantDictsList = value;
                OnPropertyChanged(("GrantDictsList"));
            }
        }

        //Selected group
        private User _selectedGroup;
        public User SelectedGroup
        {
            get
            {
                return _selectedGroup;
            }
            set
            {
                _selectedGroup = value;

                if (value != null)
                {
                    GetUsers(value.Usesysid);
                    GetLayers(value.Usename);
                    GetDicts(value.Usename);
                }
                else
                    UsersList = null;

                OnPropertyChanged("SelectedGroup");
            }
        }

        //Selected group
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
                OnPropertyChanged("SelectedUser");
            }
        }

        //Selected layer
        private Grant _selectedGrantLayer;
        public Grant SelectedGrantLayer
        {
            get
            {
                return _selectedGrantLayer;
            }
            set
            {
                _selectedGrantLayer = value;
                OnPropertyChanged("SelectedGrantLayer");
            }
        }

        //Selected dict
        private Grant _selectedGrantDict;
        public Grant SelectedGrantDict
        {
            get
            {
                return _selectedGrantDict;
            }
            set
            {
                _selectedGrantDict = value;
                OnPropertyChanged("SelectedGrantDict");
            }
        }

        private bool _isFocused;
        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                _isFocused = value;
                OnPropertyChanged(("IsFocused"));
            }
        }



        /// <summary>
        /// Constructor
        /// </summary>
        public GrantsViewModel()
        {
            Console.WriteLine("grants: " + this.GetHashCode());
            RefreshTab();
        }

        private void GetGroups()
        {
            IGrantService service = new GrantService(_connectionString);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()).CreateMapper();
            var list = mapper.Map<IEnumerable<UserDTO>, List<User>>(service.GetGroups());
            GroupsList = new ObservableCollection<User>(list);
        }

        private void GetUsers(string usesysid)
        {
            IGrantService service = new GrantService(_connectionString);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()).CreateMapper();
            var list = mapper.Map<IEnumerable<UserDTO>, List<User>>(service.GetUsers(usesysid));
            UsersList = new ObservableCollection<User>(list);
        }

        private void GetLayers(string usesysid)
        {
            IGrantService service = new GrantService(_connectionString);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<GrantDTO, Grant>()).CreateMapper();
            var list = mapper.Map<IEnumerable<GrantDTO>, List<Grant>>(service.GetLayers(usesysid));
            GrantLayersList = new ObservableCollection<Grant>(list);
        }

        private void GetDicts(string usesysid)
        {
            IGrantService service = new GrantService(_connectionString);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<GrantDTO, Grant>()).CreateMapper();
            var list = mapper.Map<IEnumerable<GrantDTO>, List<Grant>>(service.GetDicts(usesysid));
            GrantDictsList = new ObservableCollection<Grant>(list);
        }

        private void RefreshTab()
        {
            RefreshTabAsync();
        }

        private async void RefreshTabAsync()
        {
            User cur_group = SelectedGroup;
            User cur_user = SelectedUser;
            Grant cur_layer = SelectedGrantLayer;
            Grant cur_dict = SelectedGrantDict;

            await Task.Run(() => GetGroups());

            if (cur_group != null)
            {
                SelectedGroup = GroupsList.Where(p => p.Usesysid == cur_group.Usesysid).DefaultIfEmpty().First();
            }

            if (cur_user != null)
            {
                SelectedUser = UsersList.Where(p => p.Usesysid == cur_user.Usesysid).DefaultIfEmpty().First();
            }

            if (cur_layer != null)
            {
                SelectedGrantLayer = GrantLayersList.Where(p => p.Table_schema == cur_layer.Table_schema)
                                                    .Where(p => p.Table_name == cur_layer.Table_name).DefaultIfEmpty().First();
            }

            if (cur_dict != null)
            {
                SelectedGrantDict = GrantDictsList.Where(p => p.Table_schema == cur_dict.Table_schema)
                                                  .Where(p => p.Table_name == cur_dict.Table_name).DefaultIfEmpty().First();
            }

            IsFocused = true;
        }

        private async void ShowPropertyRoleAsync(object selectedRow)
        {
            var curRow = (User)selectedRow;
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            UserPropertyWindowViewModel vm = new UserPropertyWindowViewModel(curRow, displayRootRegistry);
            await displayRootRegistry.ShowModalPresentation(vm);
            if (vm.DialogResult)
                RefreshTab();
        }


        private async void ShowPropertyGrantAsync(object selectedRow)
        {
            var curRow = (Grant)selectedRow;
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            GrantPropertyWindowViewModel vm = new GrantPropertyWindowViewModel(curRow, displayRootRegistry, SelectedGroup.Usename);
            await displayRootRegistry.ShowModalPresentation(vm);
            if (vm.DialogResult)
                RefreshTab();
        }


    }
}
