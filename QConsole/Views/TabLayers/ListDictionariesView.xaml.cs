using QConsole.Models;
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
using System.Windows.Shapes;


namespace QConsole.Views.TabLayers
{
    /// <summary>
    /// Логика взаимодействия для ListDictionariesView.xaml
    /// </summary>
    public partial class ListDictionariesView : Window
    {
        public ListDictionariesView()
        {
            InitializeComponent();
        }

        //rownumers for table
        private void DG_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            var table = dg.SelectedItem as InformationSchemaTable;
            tb_schema_name.Text = table.Table_schema;
            tb_table_name.Text = table.Table_name;
        }
    }
}
