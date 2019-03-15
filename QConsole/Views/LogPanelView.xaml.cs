using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QConsole.Views
{
    /// <summary>
    /// Логика взаимодействия для BottomLogPanel.xaml
    /// </summary>
    public partial class LogPanelView : UserControl
    {
        public LogPanelView()
        {
            InitializeComponent();
        }

        private void LogPanelTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogPanelScroll.ScrollToEnd();
        }

        private void BtnLogPanelClose_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.LogPanelVisibility = Visibility.Collapsed;
            //this.Visibility = Visibility.Collapsed;
        }
    }
}
