using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для TabLoggerView.xaml
    /// </summary>
    public partial class TabLoggerView : UserControl
    {
        public TabLoggerView()
        {
            InitializeComponent();
        }


        //rownumers for table
        private void DG_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        // event select item in combo box with columns
        private void cb_Columns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedIndex > 0)
            {
                txbExtraQuery.Text = txbExtraQuery.Text + "\"" + cb.SelectedItem + "\"";
                cb.SelectedIndex = 0;
                txbExtraQuery.Focus();
                txbExtraQuery.CaretIndex = txbExtraQuery.Text.Length;
            }
        }

        private void Cb_Columns_Loaded(object sender, RoutedEventArgs e)
        {
            cb_Columns.SelectedIndex = 0;
        }

        private void Date_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DatePicker dp = (DatePicker)sender;
            Regex regex = new Regex("[^0-9./]"); //regex that matches allowed text
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
