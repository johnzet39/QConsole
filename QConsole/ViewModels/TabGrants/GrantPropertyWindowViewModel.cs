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

namespace QConsole.ViewModels.TabGrants
{
    class GrantPropertyWindowViewModel : BaseViewModel
    {
        private IGrantService grantService;
        private readonly string _connectionString = Common.ConnectionStrings.ConnectionString;
        DisplayRootRegistry DisplayRootRegistry;

        private string Tableschema;
        private string Tablename;
        private string RoleName;

        private bool? _oldIsSelect;
        private bool? _oldIsUpdate;
        private bool? _oldIsInsert;
        private bool? _oldIsDelete;

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
                      OkButton();
                  }));
            }
        }



        private bool? _isSelect;
        public bool? IsSelect
        {
            get => _isSelect;
            set
            {
                _isSelect = value;
                OnPropertyChanged(("IsSelect"));
            }
        }

        private bool? _isUpdate;
        public bool? IsUpdate
        {
            get => _isUpdate;
            set
            {
                _isUpdate = value;
                OnPropertyChanged(("IsUpdate"));
            }
        }

        private bool? _isInsert;
        public bool? IsInsert
        {
            get => _isInsert;
            set
            {
                _isInsert = value;
                OnPropertyChanged(("IsInsert"));
            }
        }

        private bool? _isDelete;
        public bool? IsDelete
        {
            get => _isDelete;
            set
            {
                _isDelete = value;
                OnPropertyChanged(("IsDelete"));
            }
        }





        /// <summary>
        /// Costructor for Edit mode.
        /// </summary>
        public GrantPropertyWindowViewModel(Grant currentRow, DisplayRootRegistry displayRootRegistry, string roleName)
        {
            DisplayRootRegistry = displayRootRegistry;

            Tableschema = currentRow.Table_schema;
            Tablename = currentRow.Table_name;
            RoleName = roleName;

            IsSelect = currentRow.IsSelect;
            _oldIsSelect = currentRow.IsSelect;
            IsUpdate = currentRow.IsUpdate;
            _oldIsUpdate = currentRow.IsUpdate;
            IsInsert = currentRow.IsInsert;
            _oldIsInsert = currentRow.IsInsert;
            IsDelete = currentRow.IsDelete;
            _oldIsDelete = currentRow.IsDelete;
        }

        private void OkButton()
        {
            if (IsSelect != _oldIsSelect || IsUpdate != _oldIsUpdate || IsInsert != _oldIsInsert || IsDelete != _oldIsDelete)
            {
                List<String> grants_list = new List<string>();
                if ((bool)IsSelect) grants_list.Add("SELECT");
                if ((bool)IsUpdate) grants_list.Add("UPDATE");
                if ((bool)IsInsert) grants_list.Add("INSERT");
                if ((bool)IsDelete) grants_list.Add("DELETE");

                try
                {
                    grantService = new GrantService(_connectionString);
                    var result = grantService.GrantTableToRole(Tableschema, Tablename, RoleName,  grants_list);
                    Ext.LogPanel.PrintLog((List<string>)result);
                }
                catch (Exception ex)
                {
                    Ext.LogPanel.PrintLog(ex.Message.ToString());
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            DialogResult = true;
            this.CloseWindow();
        }

        private void CloseWindow()
        {
            DisplayRootRegistry.HidePresentation(this);
        }


    }
}
