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
        }

        private void AddTabs()
        {
            Tabs.Add(new SessionTab());
            Tabs.Add(new UserTab());
            Tabs.Add(new LayerTab());
            Tabs.Add(new GrantTab());
            Tabs.Add(new LoggerTab());
        }

        public ICollection<ITab> Tabs { get; }
        public string Title { get; set; }

    }
}
