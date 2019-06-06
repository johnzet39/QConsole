using QConsole.Models;
using QConsole.BLL.DTO;
using QConsole.BLL.Interfaces;
using QConsole.BLL.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QConsole.Commands;
using System.Windows;

namespace QConsole.ViewModels.TabLayers
{
    class ListDictionariesViewModel : BaseViewModel
    {
        private readonly string _connectionString = Common.ConnectionStrings.ConnectionString;
        DisplayRootRegistry DisplayRootRegistry;
        private ILayerService layerService;

        public bool DialogResult = false;


        /// <summary>
        /// Constructor
        /// </summary>
        public ListDictionariesViewModel(DisplayRootRegistry displayRootRegistry)
        {
            DisplayRootRegistry = displayRootRegistry;
            GetAllTablesList();
            GetDictionariesList();

        }

        private RelayCommand _okCommand;
        public RelayCommand OkCommand
        {
            get
            {
                return _okCommand ??
                  (_okCommand = new RelayCommand(obj =>
                  {
                      ClickOkButton(obj);
                  }));
            }
        }

        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??
                  (_addCommand = new RelayCommand(obj =>
                  {
                      ClickAddButton(obj);
                  }));
            }
        }

        private RelayCommand _removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return _removeCommand ??
                  (_removeCommand = new RelayCommand(obj =>
                  {
                      ClickRemoveButton(obj);
                  }));
            }
        }

        

        private ObservableCollection<Dict> _dictionariesList;
        public ObservableCollection<Dict> DictionariesList
        {
            get => _dictionariesList;
            set
            {
                _dictionariesList = value;
                OnPropertyChanged(("DictionariesList"));
            }
        }

        private ObservableCollection<InformationSchemaTable> _allTablesList;
        public ObservableCollection<InformationSchemaTable> AllTablesList
        {
            get => _allTablesList;
            set
            {
                _allTablesList = value;
                OnPropertyChanged(("AllTablesList"));
            }
        }

        private string _schemaName;
        public string SchemaName
        {
            get => _schemaName;
            set
            {
                _schemaName = value;
                OnPropertyChanged(("SchemaName"));
            }
        }

        private string _tableName;
        public string TableName
        {
            get => _tableName;
            set
            {
                _tableName = value;
                OnPropertyChanged(("TableName"));
            }
        }

        private Dict _selectedDictionary;
        public Dict SelectedDictionary
        {
            get => _selectedDictionary;
            set
            {
                _selectedDictionary = value;
                OnPropertyChanged(("SelectedDictionary"));
            }
        }


        private void GetAllTablesList()
        {
            layerService = new LayerService(_connectionString);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<InformationSchemaTableDTO, InformationSchemaTable>()).CreateMapper();
            var mappedOrderedList = mapper.Map<IEnumerable<InformationSchemaTableDTO>, List<InformationSchemaTable>>(layerService.GetListOfAllTables())
                        .OrderBy(x=>x.Table_schema)
                        .ThenBy(x => x.Table_name);
            AllTablesList = new ObservableCollection<InformationSchemaTable>(mappedOrderedList);
        }


        private void GetDictionariesList()
        {
            layerService = new LayerService(_connectionString);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<DictionaryDTO, Dict>()).CreateMapper();
            var mappedOrderedList = mapper.Map<IEnumerable<DictionaryDTO>, List<Dict>>(layerService.GetListOfDictionaries())
                    .OrderBy(x => x.Schema_name)
                    .ThenBy(x => x.Table_name);
            DictionariesList = new ObservableCollection<Dict>(mappedOrderedList);
        }


        private void ClickAddButton(object obj)
        {
            layerService = new LayerService(_connectionString);
            layerService.AddTableToDictionaries(SchemaName, TableName);
            GetDictionariesList();
        }

        private void ClickRemoveButton(object obj)
        {
            if (MessageBox.Show("Удалить строку?",
                                "Подтверждение",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                layerService = new LayerService(_connectionString);
                layerService.RemoveTableFromDictionaries(SelectedDictionary.Id);
                GetDictionariesList();
            }
        }


        private void ClickOkButton(object obj)
        {
            DialogResult = true;
            CloseWindow();
        }


        private void CloseWindow()
        {
            DisplayRootRegistry.HidePresentation(this);
        }
    }
}
