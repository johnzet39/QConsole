using QConsole.Commands;
using QConsole.Models;
using QConsole.Models.Tabs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QConsole.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private readonly ObservableCollection<ITab> tabs;
        public MainWindowViewModel()
        {
            NewTabCommand = new RelayCommand(p => NewTab());

            tabs = new ObservableCollection<ITab>();
            tabs.CollectionChanged += Tabs_CollectionChanged;

            Tabs = tabs;
            //Tabs = new ObservableCollection<ITab>();
            AddTabs();
        }

        public ICommand NewTabCommand { get; }
        public Collection<ITab> Tabs { get; }
        public string Title { get; set; }

        private void AddTabs()
        {
            Tabs.Add(new SessionTab());
            Tabs.Add(new UserTab());
            Tabs.Add(new LayerTab());
            Tabs.Add(new GrantTab());
            Tabs.Add(new StatsTab());
            Tabs.Add(new LoggerTab());
        }

        private void NewTab()
        {
            Tabs.Add(new QueryTab());
        }

        private void Tabs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ITab tab;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    tab = (ITab)e.NewItems[0];
                    tab.CloseRequested += OnTabCloseRequested;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    tab = (ITab)e.OldItems[0];
                    tab.CloseRequested -= OnTabCloseRequested;
                    break;
            }
        }

        private void OnTabCloseRequested(object sender, EventArgs e)
        {
            Tabs.Remove((ITab)sender);
        }

    }
}
