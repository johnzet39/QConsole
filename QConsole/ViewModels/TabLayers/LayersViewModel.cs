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

namespace QConsole.ViewModels.TabLayers
{
    class LayersViewModel : BaseViewModel
    {
        private readonly string _connectionString = Common.ConnectionStrings.ConnectionString;
        private ILayerService service;

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
        private RelayCommand _showPropertyCommand;
        public RelayCommand ShowPropertyCommand
        {
            get
            {
                return _showPropertyCommand ??
                  (_showPropertyCommand = new RelayCommand(obj =>
                  {
                      ShowPropertyAsync(obj);
                  }));
            }
        }

        

        // Layers list for datagrid.
        private ObservableCollection<Layer> _layersList;
        public ObservableCollection<Layer> LayersList
        {
            get => _layersList;
            set
            {
                _layersList = value;
                OnPropertyChanged(("LayersList"));
            }
        }

        // Dicts list for datagrid.
        private ObservableCollection<Layer> _dictsList;
        public ObservableCollection<Layer> DictsList
        {
            get => _dictsList;
            set
            {
                _dictsList = value;
                OnPropertyChanged(("DictsList"));
            }
        }

        //Selected Layer
        private Layer _selectedLayer;
        public Layer SelectedLayer
        {
            get
            {
                return _selectedLayer;
            }
            set
            {
                _selectedLayer = value;
                OnPropertyChanged("SelectedLayer");
            }
        }

        //Selected Layer
        private Layer _selectedDicts;
        public Layer SelectedDict
        {
            get
            {
                return _selectedDicts;
            }
            set
            {
                _selectedDicts = value;
                OnPropertyChanged("SelectedDict");
            }
        }

        //
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
        public LayersViewModel()
        {
            Console.WriteLine("layers: " + this.GetHashCode());
            RefreshTab();
        }

        private void GetLayers()
        {
            service = new LayerService(_connectionString);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<LayerDTO, Layer>()).CreateMapper();
            var layers = mapper.Map<IEnumerable<LayerDTO>, List<Layer>>(service.GetLayers());
            LayersList = new ObservableCollection<Layer>(layers);
        }

        private void GetDicts()
        {
            service = new LayerService(_connectionString);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<LayerDTO, Layer>()).CreateMapper();
            var layers = mapper.Map<IEnumerable<LayerDTO>, List<Layer>>(service.GetDicts());
            DictsList = new ObservableCollection<Layer>(layers);
        }

        private void RefreshTab()
        {
            RefreshTabAsync();
        }

        private async void RefreshTabAsync()
        {
            Layer cur_layer = SelectedLayer;
            Layer cur_dict = SelectedDict;

            await Task.Run(() => GetLayers());
            await Task.Run(() => GetDicts());

            if (cur_layer != null)
            {
                SelectedLayer = LayersList.Where(p => p.Table_schema == cur_layer.Table_schema)
                                          .Where(p => p.Table_name == cur_layer.Table_name).DefaultIfEmpty().First();
            }
            if (cur_dict != null)
            {
                SelectedDict = DictsList.Where(p => p.Table_schema == cur_dict.Table_schema)
                                          .Where(p => p.Table_name == cur_dict.Table_name).DefaultIfEmpty().First();
            }

            IsFocused = true;
        }

        private async void ShowPropertyAsync(object selectedRow)
        {
            var curRow = (Layer)selectedRow;
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            LayerPropertyWindowViewModel vm = new LayerPropertyWindowViewModel(curRow, displayRootRegistry);
            await displayRootRegistry.ShowModalPresentation(vm);
            if (vm.DialogResult)
                RefreshTab();
        }

    }
}
