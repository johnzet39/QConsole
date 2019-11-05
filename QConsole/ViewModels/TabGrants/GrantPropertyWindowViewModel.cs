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
using AutoMapper;
using QConsole.BLL.DTO;

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
        private List<GrantColumn> _oldColumnsList;

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

        private bool _isSelect;
        public bool IsSelect
        {
            get => _isSelect;
            set
            {
                _isSelect = value;
                OnPropertyChanged(("IsSelect"));
            }
        }

        private bool _isUpdate;
        public bool IsUpdate
        {
            get => _isUpdate;
            set
            {
                _isUpdate = value;
                OnPropertyChanged(("IsUpdate"));
            }
        }

        private bool _isInsert;
        public bool IsInsert
        {
            get => _isInsert;
            set
            {
                _isInsert = value;
                OnPropertyChanged(("IsInsert"));
            }
        }

        private bool _isDelete;
        public bool IsDelete
        {
            get => _isDelete;
            set
            {
                _isDelete = value;
                OnPropertyChanged(("IsDelete"));
            }
        }

        private List<GrantColumn> _columnsList;
        public List<GrantColumn> ColumnsList
        {
            get => _columnsList;
            set
            {
                _columnsList = value;
                OnPropertyChanged("ColumnsList");
            }
        }


        /// <summary>
        /// Costructor for Edit mode.
        /// </summary>
        public GrantPropertyWindowViewModel(Grant currentRow, DisplayRootRegistry displayRootRegistry, string roleName)
        {
            grantService = new GrantService(_connectionString);
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

            ColumnsList = GetColumns(currentRow.Table_schema, currentRow.Table_name, roleName);
            _oldColumnsList = new List<GrantColumn>(ColumnsList.Select(x => (GrantColumn)x.Clone()));
            Console.WriteLine(ColumnsList.GetHashCode());
            Console.WriteLine(_oldColumnsList.GetHashCode());
        }

        private void OkButton()
        {
            GrantActions();
            GrantColumns();
            
            DialogResult = true;
            this.CloseWindow();
        }

        private void GrantActions()
        {
            if (IsSelect != _oldIsSelect || IsUpdate != _oldIsUpdate || IsInsert != _oldIsInsert || IsDelete != _oldIsDelete)
            {
                bool selChanged = false;
                bool updChanged = false;
                bool insChanged = false;
                bool delChanged = false;
                if (IsSelect != _oldIsSelect)
                    selChanged = true;
                if (IsUpdate != _oldIsUpdate)
                    updChanged = true;
                if (IsInsert != _oldIsInsert)
                    insChanged = true;
                if (IsDelete != _oldIsDelete)
                    delChanged = true;

                try
                {
                    grantService.GrantTableToRole(Tableschema, Tablename, RoleName,
                                                  IsSelect, IsUpdate, IsInsert, IsDelete,
                                                  selChanged, updChanged, insChanged, delChanged);
                }
                catch (Exception ex)
                {
                    Ext.LogPanel.PrintLog(ex.Message.ToString());
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void GrantColumns()
        {
            var columnGranters = CompareColumnsGrants(_oldColumnsList, ColumnsList,
                                                      out bool selChanged, out bool updChanged,
                                                      out bool insChanged);

            if (columnGranters?.Count() > 0)
            {
                try
                {
                    List<string> selectList = new List<string>();
                    List<string> updateList = new List<string>();
                    List<string> insertList = new List<string>();

                    foreach (var gran in columnGranters)
                    {
                        if (gran.IsSelect)
                            selectList.Add(gran.ColumnName);
                        if (gran.IsUpdate)
                            updateList.Add(gran.ColumnName);
                        if (gran.IsInsert)
                            insertList.Add(gran.ColumnName);
                    }
                    grantService.GrantColumnsToRole(Tableschema,Tablename,RoleName,
                                                selectList, updateList, insertList,
                                                selChanged, updChanged, insChanged);
                }
                catch (Exception ex)
                {
                    Ext.LogPanel.PrintLog(ex.Message.ToString());
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

        }

        private List<ColumnGranter> CompareColumnsGrants(List<GrantColumn> old_columns, List<GrantColumn> columns,
                                                        out bool selChanged, out bool updChanged, out bool insChanged)
        {
            selChanged = false;
            updChanged = false;
            insChanged = false;

            List<ColumnGranter> granters = new List<ColumnGranter>();
            for (int i = 0; i < columns.Count(); i++)
            {
                bool hasChanges = false;

                if (columns[i].IsSelect != old_columns[i].IsSelect)
                    selChanged = true;
                if (columns[i].IsUpdate != old_columns[i].IsUpdate)
                    updChanged = true;
                if (columns[i].IsInsert != old_columns[i].IsInsert)
                    insChanged = true;

                if (selChanged || updChanged || insChanged)
                    hasChanges = true;

                if (hasChanges)
                {
                    ColumnGranter columnGranter = new ColumnGranter
                    {
                        ColumnName = columns[i].Column_name
                    };

                    if (columns[i].IsSelect)
                        columnGranter.IsSelect = true;
                    if (columns[i].IsUpdate)
                        columnGranter.IsUpdate = true;
                    if (columns[i].IsInsert)
                        columnGranter.IsInsert = true;

                    granters.Add(columnGranter);
                }
            }
            return granters;
        }

        private void CloseWindow()
        {
            DisplayRootRegistry.HidePresentation(this);
        }

        private List<GrantColumn> GetColumns(string table_schema, string table_name, string role_name)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<GrantColumnDTO, GrantColumn>()).CreateMapper();
            return mapper.Map<IEnumerable<GrantColumnDTO>, List<GrantColumn>>(grantService.GetColumns(table_schema, table_name, role_name));
        }

        public class ColumnGranter
        {
            public string ColumnName { get; set; }
            public bool IsSelect { get; set; }
            public bool IsUpdate { get; set; }
            public bool IsInsert { get; set; }
        }

    }
}
