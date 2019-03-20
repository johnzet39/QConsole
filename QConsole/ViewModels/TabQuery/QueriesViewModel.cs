using QConsole.BLL.Interfaces;
using QConsole.BLL.Services;
using QConsole.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QConsole.ViewModels.TabQuery
{
    class QueriesViewModel : BaseViewModel
    {
        private readonly string _connectionString = Common.ConnectionStrings.ConnectionString;
        private IQueryService _queryService;


        private RelayCommand executeQueryCommand;
        public RelayCommand ExecuteQueryCommand
        {
            get
            {
                return executeQueryCommand ??
                  (executeQueryCommand = new RelayCommand(obj =>
                  {
                      ExecuteQuery();
                  },
                  obj =>
                  {
                      return QueryString != null && QueryString.Trim().Length > 0;
                  }));
            }
        }

        

        private string _queryString;
        public string QueryString
        {
            get
            {
                return _queryString;
            }
            set
            {
                _queryString = value;
                OnPropertyChanged("QueryString");
            }
        }

        private string _selectedText;
        public string SelectedText
        {
            get
            {
                return _selectedText;
            }
            set
            {
                _selectedText = value;
                OnPropertyChanged("SelectedText");
            }
        }

        private DataTable _executeResult;
        public DataTable ExecuteResult
        {
            get
            {
                return _executeResult;
            }
            set
            {
                _executeResult = value;
                OnPropertyChanged("ExecuteResult");
            }
        }



        private void ExecuteQuery()
        {
            string sql;
            if (SelectedText != null && SelectedText.Length > 0)
                sql = SelectedText;
            else
                sql = QueryString;

            if (CheckOnlySelectQuery(sql)) // We can't execute with UPDATE, DELETE, INSERT operators.
            {
                _queryService = new QueryService(_connectionString);
                try
                {
                    ExecuteResult = null;
                    ExecuteResult = _queryService.ExecuteQuery(sql);
                }
                catch (Exception ex)
                {
                    Ext.LogPanel.PrintLog(ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("В целях безопасности в приложении отключена возможность выполнять sql-запросы с операторами INSERT, UPDATE, DELETE");
            }

        }


        private bool CheckOnlySelectQuery(string sql)
        {
            if (sql.ToLower().Contains("insert ") || sql.ToLower().Contains("update ") || sql.ToLower().Contains("delete "))
            {
                return false;
            }
            return true;
        }
    }


    
}
