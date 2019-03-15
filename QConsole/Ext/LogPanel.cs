using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.Ext
{
    public static class LogPanel
    {
    
        public static int Count { get; set; }

        static LogPanelNotify _currentMessage = new LogPanelNotify();
        public static LogPanelNotify CurrentMessage
        {
            get
            {
                return _currentMessage;
            }

        }

        static LogPanel()
        {
            CurrentMessage.Count = 0;
            CurrentMessage.Text = string.Empty;
        }

        public static void PrintLog(string sql)
        {
            CurrentMessage.Count += 1;
            CurrentMessage.Text += String.Format("\n {0}>  {1}", CurrentMessage.Count.ToString(), sql);
        }

        public static void PrintLog(IEnumerable<String> sqls)
        {
            foreach (string sql in sqls)
            {
                CurrentMessage.Count += 1;
                CurrentMessage.Text += String.Format("\n {0}>  {1}", CurrentMessage.Count.ToString(), sql);
            }
        }
    }

    public class LogPanelNotify : INotifyPropertyChanged
    {
        private int _count;
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
            }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
