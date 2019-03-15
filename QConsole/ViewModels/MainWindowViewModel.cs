using QConsole.Commands;
using QConsole.Models;
using QConsole.Models.Tabs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QConsole.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            Tabs = new ObservableCollection<ITab>();
            AddTabs();

            //IsVisibleLogPanel = Properties.Settings.Default.LogPanelVisibility;
        }

        private void AddTabs()
        {
            Tabs.Add(new SessionTab());
            Tabs.Add(new UserTab());
            Tabs.Add(new LayerTab());
            Tabs.Add(new GrantTab());
        }

        public ICollection<ITab> Tabs { get; }
        public string Title { get; set; }



        //private Visibility _isVisibleLogPanel;
        //public Visibility IsVisibleLogPanel
        //{
        //    get
        //    {
        //        return _isVisibleLogPanel;
        //    }
        //    set
        //    {
        //        _isVisibleLogPanel = value;
        //        Properties.Settings.Default.LogPanelVisibility = value;

        //        OnPropertyChanged("IsVisibleLogPanel");
        //    }
        //}

        //private RelayCommand _closeOrShowLogPanelCommand;
        //public RelayCommand CloseOrShowLogPanelCommand
        //{
        //    get
        //    {
        //        return _closeOrShowLogPanelCommand ??
        //          (_closeOrShowLogPanelCommand = new RelayCommand(obj =>
        //          {
        //              CloseOrShowLogPanel();
        //          }));
        //    }
        //}

        //private void CloseOrShowLogPanel()
        //{
        //    if (IsVisibleLogPanel == Visibility.Collapsed)
        //    {
        //        Console.WriteLine("set visible");
        //        IsVisibleLogPanel = Visibility.Visible;
        //    }
        //    else if (IsVisibleLogPanel == Visibility.Visible)
        //    {
        //        Console.WriteLine("set collapsed");
        //        IsVisibleLogPanel = Visibility.Collapsed;
        //    }
        //}
    }
}
