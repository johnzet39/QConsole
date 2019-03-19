using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QWpfControls
{
    public partial class TextBoxWithClear : UserControl
    {
        public TextBoxWithClear()
        {
            InitializeComponent();
            //this.FocusableChanged += new DependencyPropertyChangedEventHandler(TextBoxWithClear_IsVisibleChanged);
            ClearButton.Visibility = Visibility.Collapsed;
        }


        //Using a DependencyProperty as the backing store for Text.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBoxWithClear), new PropertyMetadata(""));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        //public new void Focus()
        //{
        //    tBox.Focus();
        //    tBox.CaretIndex = tBox.Text.Length;
        //}


        // Using a DependencyProperty as the backing store for CaretIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaretIndexProperty = DependencyProperty.Register("CaretIndex", typeof(int), typeof(TextBoxWithClear));
        public int CaretIndex
        {
            get { return (int)GetValue(CaretIndexProperty); }
            set { SetValue(CaretIndexProperty, value); }
        }
        
        // click clear button.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tBox.Text = string.Empty;
            tBox.Focus();
            
        }
        // ----------------------------------------????
        // Set focus to textbox
        public void TextBoxWithClear_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                Dispatcher.BeginInvoke(
                DispatcherPriority.ContextIdle,
                new Action(delegate ()
                {
                    tBox.Focus();
                }));
            }
        }

        private void TBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tBox.Text.Length > 0)
                ClearButton.Visibility = Visibility.Visible;
            else
                ClearButton.Visibility = Visibility.Collapsed;
        }
    }


}

