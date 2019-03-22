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
using QConsole.Ext;

namespace QConsole.Views.Tabs
{
    /// <summary>
    /// Логика взаимодействия для TabQueryView.xaml
    /// </summary>
    public partial class TabQueryView : UserControl
    {
        public TabQueryView()
        {
            InitializeComponent();
        }

        private void Tb_Query_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBoxHelper.SetSelectedText(tb_Query, tb_Query.SelectedText);

            int row = tb_Query.GetLineIndexFromCharacterIndex(tb_Query.CaretIndex);
            int col = tb_Query.CaretIndex - tb_Query.GetCharacterIndexFromLineIndex(row);
            lblCursorPosition.Text = "Line " + (row + 1) + ", Char " + (col + 1);
        }
    }
}
