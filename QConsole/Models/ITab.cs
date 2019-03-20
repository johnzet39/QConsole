using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QConsole.Commands;

namespace QConsole.Models
{
    public interface ITab
    {
        string Name { get; set; }
        string IconPath { get; set; }
        bool IsClosable { get; set; }
        ICommand CloseCommand { get; }
        event EventHandler CloseRequested;
    }

    public abstract class Tab : ITab
    {
        public Tab()
        {
            CloseCommand = new RelayCommand(p => CloseRequested?.Invoke(this, EventArgs.Empty));
            IsClosable = false;
        }

        public string Name { get; set; }
        public string IconPath { get; set; }
        public bool IsClosable { get; set; }
        public ICommand CloseCommand { get; }
        public event EventHandler CloseRequested;
    }
}
