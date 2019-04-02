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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Windows.Media;

namespace QConsole.ViewModels.TabStats
{
    class StatsViewModel : BaseViewModel
    {
        private readonly string _connectionString = Common.ConnectionStrings.ConnectionString;
        private int periodDays;
        private ILayerService layerService;
        private ILoggerService loggerService;
        List<Layer> layerList;

        /// <summary>
        /// Constructor
        /// </summary>
        public StatsViewModel()
        {
            PeriodDays = 30;
            Console.WriteLine("stats: " + this.GetHashCode());
            CreatePlotCountOperations();
            CreatePlotCountOfPeriod();
            RefreshTab();
        }

        public int PeriodDays
        {
            get => periodDays;
            set
            {
                periodDays = value;
                OnPropertyChanged(("PeriodDays"));
            }
        }

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

        private void RefreshTab()
        {
            RefreshTabAsync();
        }

        private async void RefreshTabAsync()
        {
            await Task.Run(() => GetLayers());

            //App.Current.Dispatcher.BeginInvoke(new Action(this.RefreshPlotCountOfPeriod));
            RefreshPlotCountInserts();
            RefreshPlotCountOperations();
        }

        private void GetLayers()
        {
            layerService = new LayerService(_connectionString);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<LayerDTO, Layer>()).CreateMapper();
            layerList = mapper.Map<IEnumerable<LayerDTO>, List<Layer>>(layerService.GetLayers());
        }

        // #####################
        // ####### PLOTS operations #######
        // #####################
        #region Plot operations

        private void CreatePlotCountOperations()
        {
            SeriesCountOperationsCollection = new SeriesCollection();
        }

        private void RefreshPlotCountOperations()
        {
            if (SeriesCountOperationsCollection != null)
            {
                foreach (var series in SeriesCountOperationsCollection)
                {
                    SeriesCountOperationsCollection.Remove(series);
                }

                string[] operations = { "INSERT", "UPDATE", "DELETE" };

                int index = 0;
                loggerService = new LoggerService(_connectionString);
                foreach (string operation in operations)
                {
                    int count = loggerService.GetCountByOperation(operation, PeriodDays);
                    if (count > 0)
                    {

                        PieSeries pieSeries = new PieSeries
                        {
                            Title = operation,
                            Values = new ChartValues<ObservableValue> { new ObservableValue(count) },
                            DataLabels = true
                        };
                        SeriesCountOperationsCollection.Add(pieSeries);
                        index++;
                    }
                }
            }
        }
        public SeriesCollection SeriesCountOperationsCollection { get; set; }

        #endregion


        // #####################
        // ####### PLOTS inserts #######
        // #####################
        #region Plot inserts

        //Func<ChartPoint, string> labelPoint;

        private void CreatePlotCountOfPeriod()
        {
            SeriesCountInsertsCollection = new SeriesCollection();
            //labelPoint = chartPoint =>
            //    string.Format("{0} \n ({1:P})", chartPoint.Y, chartPoint.Participation);
        }


        private void RefreshPlotCountInserts()
        {
            if (SeriesCountInsertsCollection != null)
            {

                foreach (var series in SeriesCountInsertsCollection)
                {
                    SeriesCountInsertsCollection.Remove(series);
                }

                loggerService = new LoggerService(_connectionString);

                foreach (Layer layer in layerList)
                {
                    int count = loggerService.GetCountInserts(layer.Table_schema, layer.Table_name, PeriodDays);

                    if (count > 0)
                    {
                        PieSeries pieSeries = new PieSeries
                        {
                            Title = layer.Table_name +
                                        (layer.Descript != String.Empty && layer.Descript != null ? (string.Format("\n({0})", layer.Descript)) : ""),
                            Values = new ChartValues<ObservableValue> { new ObservableValue(count) },
                            DataLabels = true
                            //LabelPoint = labelPoint,
                        };
                        SeriesCountInsertsCollection.Add(pieSeries);
                    }
                }
            }
        }

        public SeriesCollection SeriesCountInsertsCollection { get; set; }

        #endregion
    }
}
