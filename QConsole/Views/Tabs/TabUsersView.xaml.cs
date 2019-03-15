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

namespace QConsole.Views.Tabs
{
    /// <summary>
    /// Логика взаимодействия для TabUsers.xaml
    /// </summary>
    public partial class TabUsersView : UserControl
    {
        public TabUsersView()
        {
            InitializeComponent();
        }

        //rownumers for table
        private void DG_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void BtnUsersUpdate_Click(object sender, RoutedEventArgs e)
        {
            DG_Users.Focus();
        }

    }
}
