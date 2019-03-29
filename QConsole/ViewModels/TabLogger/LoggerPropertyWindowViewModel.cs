using QConsole.BLL.Interfaces;
using QConsole.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.ViewModels.TabLogger
{
    class LoggerPropertyWindowViewModel : BaseViewModel
    {
        DisplayRootRegistry DisplayRootRegistry;
        IList<LogRow> ListHist;
        LogRow SelectedLogRow;

        //Selected LogRow
        private ObservableCollection<string> _currentLogRow;
        public ObservableCollection<string> CurrentLogRow
        {
            get
            {
                return _currentLogRow;
            }
            set
            {
                _currentLogRow = value;
                OnPropertyChanged("CurrentLogRow");
            }
        }

        //All history for current LogRow
        private ObservableCollection<string> _allLogRow;
        public ObservableCollection<string> AllLogRow
        {
            get
            {
                return _allLogRow;
            }
            set
            {
                _allLogRow = value;
                OnPropertyChanged("AllLogRow");
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public LoggerPropertyWindowViewModel(LogRow selectedLogRow, IList<LogRow> listHist, DisplayRootRegistry displayRootRegistry)
        {
            DisplayRootRegistry = displayRootRegistry;
            SelectedLogRow = selectedLogRow;
            ListHist = listHist;

            PopulateCurrentRow();
            PopulateHistoryRows();
        }

        private void PopulateCurrentRow()
        {
            string curString = BuildStringItem(1, SelectedLogRow);
            CurrentLogRow = new ObservableCollection<string>( new List<string> {curString} );
            
        }

        private void PopulateHistoryRows()
        {
            List<string> histStrings = new List<string>();
            int i = 0;
            foreach (LogRow lr in ListHist)
            {
                histStrings.Add(BuildStringItem(++i, lr));
            }
            AllLogRow = new ObservableCollection<string>(histStrings);
        }

        private string BuildStringItem(int i, LogRow row)
        {
            string rowstring = "";
            rowstring = string.Format("{0}. {1} - {2} ({3})\n   {4}\n\n", i, row.Timechange, row.Username, row.Action, (row.Context).Replace("; ", "\n   "));
            return rowstring;
        }
    }
}
