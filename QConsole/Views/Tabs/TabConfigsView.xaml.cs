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
using QConsole.Models.Tabs;

namespace QConsole.Views.Tabs
{
    /// <summary>
    /// Логика взаимодействия для TabQueryView.xaml
    /// </summary>
    public partial class TabConfigsView : UserControl
    {
        public TabConfigsView()
        {
            InitializeComponent();
        }

        private void Tb_text_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBoxHelper.SetSelectedText(tb_text, tb_text.SelectedText);

            int row = tb_text.GetLineIndexFromCharacterIndex(tb_text.CaretIndex);
            int col = tb_text.CaretIndex - tb_text.GetCharacterIndexFromLineIndex(row);
            lblCursorPosition.Text = "Line " + (row + 1) + ", Char " + (col + 1);
        }

        private void BtnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            tb_text.Focus();
            tb_text.SelectAll();
        }
    }
}
