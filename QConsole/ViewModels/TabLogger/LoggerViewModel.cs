using QConsole.Commands;
using QConsole.Models;
using QConsole.BLL.Services;
using QConsole.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QConsole.BLL.DTO;
using System.Windows;

namespace QConsole.ViewModels.TabLogger
{
    class LoggerViewModel : BaseViewModel
    {
        private readonly string _connectionString = Common.ConnectionStrings.ConnectionString;
        private ILoggerService _loggerService;
        static Ext.Paging<LogRow> PagedTable;
        private List<LogRow> logList;

        public int[] NumbersOfRecords { get; set; } //Array of groups pages rows count in Combobox 
        public IList<string> ColumnsList { get; set; } //List of attributes in combobox


        /// <summary>
        /// Constrictor
        /// </summary>
        public LoggerViewModel()
        {
            Console.WriteLine("logger: " + this.GetHashCode());

            _loggerService = new LoggerService(_connectionString);
            InitService();
            OnlyLastRows = true;
            PopulateComboBoxNumberGroups();
            PopulateComboBoxAttributes();
            NumberOfRecPerPage = 250;//Current number of rows in pages

            //RefreshTab(); //This is in NumberOfRecPerPage property.
        }

        private void InitService()
        {
            _loggerService.LastRowsCount = Properties.Settings.Default.LoggerDefaultRowCount;
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

        // Show property command.
        private RelayCommand _showPropertyCommand;
        public RelayCommand ShowPropertyCommand
        {
            get
            {
                return _showPropertyCommand ??
                  (_showPropertyCommand = new RelayCommand(obj =>
                  {
                      ShowPropertyAsync();
                  }));
            }
        }

        // Layers list for datagrid.
        private ObservableCollection<LogRow> _logrowsList;
        public ObservableCollection<LogRow> LogRowsList
        {
            get => _logrowsList;
            set
            {
                _logrowsList = value;
                OnPropertyChanged(("LogRowsList"));
            }
        }

        //Selected LogRow
        private LogRow _selectedLogRow;
        public LogRow SelectedLogRow
        {
            get
            {
                return _selectedLogRow;
            }
            set
            {
                _selectedLogRow = value;
                OnPropertyChanged("SelectedLogRow");
            }
        }

        //Sub query from textbox
        private string _extraQuery;
        public string ExtraQuery
        {
            get
            {
                return _extraQuery;
            }
            set
            {
                HasError = false;
                _extraQuery = value;
                OnPropertyChanged("ExtraQuery");
            }
        }

        //Date from
        private DateTime? _dateFrom;
        public DateTime? DateFrom
        {
            get
            {
                return _dateFrom;
            }
            set
            {
                _dateFrom = value;
                OnPropertyChanged("DateFrom");
            }
        }

        //Date to
        private DateTime? _dateTo;
        public DateTime? DateTo
        {
            get
            {
                return _dateTo;
            }
            set
            {
                _dateTo = value;
                OnPropertyChanged("DateTo");
            }
        }

        //Only Last Rows CheckBox
        private bool _onlyLastRows;
        public bool OnlyLastRows
        {
            get
            {
                return _onlyLastRows;
            }
            set
            {
                _onlyLastRows = value;
                OnPropertyChanged("OnlyLastRows");
            }
        }

        // Current number of rows in pages
        private int _numberOfRecPerPage;
        public int NumberOfRecPerPage
        {
            get
            {
                return _numberOfRecPerPage;
            }
            set
            {
                _numberOfRecPerPage = value;
                OnPropertyChanged("NumberOfRecPerPage");
                RefreshTab();
            }
        }

        // Info about current page
        private string _pageInfo;
        public string PageInfo
        {
            get
            {
                return _pageInfo;
            }
            set
            {
                _pageInfo = value;
                OnPropertyChanged("PageInfo");
            }
        }

        //HasError for style ExtraQuery TextBox
        private bool _hasError;
        public bool HasError
        {
            get
            {
                return _hasError;
            }
            set
            {
                _hasError = value;
                OnPropertyChanged("HasError");
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


        private void RefreshTab()
        {
            InitService();
            RefreshTabAsync();
        }

        private async void RefreshTabAsync()
        {
            LogRow CurLogRow = SelectedLogRow;

            await Task.Run(() => GetLogRowsList());

            if (CurLogRow != null)
            {
                SelectedLogRow = LogRowsList.Where(p => p.Gid == CurLogRow.Gid).DefaultIfEmpty().First();
            }

            IsFocused = true;
        }

        private void GetLogRowsList()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<LogRowDTO, LogRow>()).CreateMapper();

            try
            {
                HasError = false;
                logList = mapper.Map<IEnumerable<LogRowDTO>, List<LogRow>>(_loggerService.GetLogList(DateFrom, DateTo, ExtraQuery, OnlyLastRows));
            }
            catch (Exception e)
            {
                HasError = true;
                Ext.LogPanel.PrintLog(e.Message);
            }

            if (logList != null)
            {
                if (OnlyLastRows == true)//Show only last 1000 rows
                {
                    LogRowsList = new ObservableCollection<LogRow>(logList);
                }
                else//Paging
                {
                    PagedTable = new Ext.Paging<LogRow>();
                    PagedTable.PageIndex = 1;
                    IList<LogRow> firstTable = PagedTable.First(logList, NumberOfRecPerPage);
                    LogRowsList = new ObservableCollection<LogRow>(firstTable);
                    PageInfo = PageNumberDisplay();
                }
            }

        }

        private async void ShowPropertyAsync()
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            IList<LogRow> listHist = LogRowsList.Where(x => x.Tableschema == SelectedLogRow.Tableschema 
                                                       && x.Tablename == SelectedLogRow.Tablename
                                                       && x.Gidnum == SelectedLogRow.Gidnum
                                                       && x.Gid != SelectedLogRow.Gid).ToList();
            LoggerPropertyWindowViewModel vm = new LoggerPropertyWindowViewModel(SelectedLogRow, listHist, displayRootRegistry);
            await displayRootRegistry.ShowModalPresentation(vm);
        }

        #region Paging

        private void PopulateComboBoxNumberGroups()
        {
            NumbersOfRecords = new int[] { 10, 50, 100, 250, 500, 1000, 2000 };
        }

        private void PopulateComboBoxAttributes()
        {
            //_loggerService = new LoggerService(_connectionString);
            IList<string> list = _loggerService.GetColumnsList();
            list.Insert(0, "Атрибуты");
            ColumnsList = list;
        }

        /// <summary>
        /// If Page Mode.
        /// </summary>
        private string PageNumberDisplay()
        {
            int PagedNumber = NumberOfRecPerPage * (PagedTable.PageIndex + 1);
            if (PagedNumber > logList.Count)
            {
                PagedNumber = logList.Count;
            }
            int StartDisplayedNumber = (PagedTable.PageIndex) * NumberOfRecPerPage + 1;
            return StartDisplayedNumber + "-" + PagedNumber + "/" + logList.Count; //This dramatically reduced the number of times I had to write this string statement
        }

        // Next page button command.
        private RelayCommand _nextPageCommand;
        public RelayCommand NextPageCommand
        {
            get
            {
                return _nextPageCommand ??
                  (_nextPageCommand = new RelayCommand(obj =>
                  {
                      LogRowsList = new ObservableCollection<LogRow>(PagedTable.Next(logList, NumberOfRecPerPage) );
                      PageInfo = PageNumberDisplay();
                  }));
            }
        }

        // Previous page button command.
        private RelayCommand _previousPageCommand;
        public RelayCommand PreviousPageCommand
        {
            get
            {
                return _previousPageCommand ??
                  (_previousPageCommand = new RelayCommand(obj =>
                  {
                      LogRowsList = new ObservableCollection<LogRow>(PagedTable.Previous(logList, NumberOfRecPerPage));
                      PageInfo = PageNumberDisplay();
                  }));
            }
        }

        // First page button command.
        private RelayCommand _firstPageCommand;
        public RelayCommand FirstPageCommand
        {
            get
            {
                return _firstPageCommand ??
                  (_firstPageCommand = new RelayCommand(obj =>
                  {
                      LogRowsList = new ObservableCollection<LogRow>(PagedTable.First(logList, NumberOfRecPerPage));
                      PageInfo = PageNumberDisplay();
                  }));
            }
        }

        // Last page button command.
        private RelayCommand _lastPageCommand;
        public RelayCommand LastPageCommand
        {
            get
            {
                return _lastPageCommand ??
                  (_lastPageCommand = new RelayCommand(obj =>
                  {
                      LogRowsList = new ObservableCollection<LogRow>(PagedTable.Last(logList, NumberOfRecPerPage));
                      PageInfo = PageNumberDisplay();
                  }));
            }
        }


        #endregion

        
    }
}
