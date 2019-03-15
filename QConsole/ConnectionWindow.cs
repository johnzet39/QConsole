using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.Serialization.Formatters.Binary;
using QConsole.BLL;
using System.IO;

namespace QConsole
{
    class ConnectionWindow : LoginWindow
    {
        private string _CurrentConnectionName { get; set; }

        public ConnectionWindow()
        {
            InitializeComponent();

            btnOk.Click -= base.BtnOk_Click;
            btnOk.Click += btnOkNew_Click;
            btnExit.Click -= base.BtnExit_Click;
            btnExit.Click += btnExitNew_Click;
            this.Loaded -= base.Window_Loaded;
            this.Loaded += WindowNew_Loaded;

            _CurrentConnectionName = tbNewConnectionName.Text;
        }

        private void WindowNew_Loaded(object sender, RoutedEventArgs e)
        {
            _CurrentConnectionName = tbNewConnectionName.Text;

            login_image.Visibility = Visibility.Collapsed;//image collapse
            login_image_panel.Visibility = Visibility.Collapsed;//image collapse

            gb_Connections.Visibility = Visibility.Collapsed;
            tbPassword.Visibility = Visibility.Collapsed;
            label_password.Visibility = Visibility.Collapsed;
            gb_NewConnectionName.Visibility = Visibility.Visible;

            this.Title = "Создание/Редактирование подключения";
            btnExit.Content = "Отмена";

            //TextBoxes
            tbHost.IsReadOnly = false;
            tbPort.IsReadOnly = false;
            tbBD.IsReadOnly = false;
            tbUsername.IsReadOnly = false;

            //Find all textBoxes on container and create Event on TextChanged
            IEnumerable<TextBox> tbs = Ext.UIHelper.FindVisualChildrenByType<TextBox>(this);
            foreach (TextBox tb in tbs)
                tb.TextChanged += txb_TextChanged;

            tbNewConnectionName.Focus();
        }

        private void btnOkNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChechValues();//checking of entered values in text fields
                DialogResult = true;
                this.Close();
            }
            catch (PortException ex)
            {
                //MessageBox.Show(ex.Message);
                Ext.UIHelper.ShowToolTip(ex.Message, ex.Ctrl, 2);
                tbPort.Focus();
            }
            catch (EmptyValueException ex)
            {
                ex.Ctrl.Focus();
                ex.Ctrl.Background = Brushes.MistyRose;
                Ext.UIHelper.ShowToolTip(ex.Message, ex.Ctrl, 2);
            }
            catch (ExistsConnectionNameException ex)
            {
                Ext.UIHelper.ShowToolTip(ex.Message, ex.Ctrl, 2);
                tbNewConnectionName.Focus();
            }
        }

        private void btnExitNew_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //checking of entered values in text fields
        private void ChechValues()
        {
            if (_CurrentConnectionName != tbNewConnectionName.Text.Trim())
            {
                if (IsExistsConnectionName(tbNewConnectionName.Text.Trim()))
                {
                    throw new ExistsConnectionNameException("Подключение с таким именем уже существует", tbNewConnectionName);
                }
            }
            //check only digit symbols in textbox Prot
            if (!IsDigitsOnly(tbPort.Text))
                throw new PortException("Введите корректное значение порта. Значение не может быть строковым.", tbPort);
            //check empty values in All textboxes of container (gb_ConnValues)
            IEnumerable<TextBox> tbs = Ext.UIHelper.FindVisualChildrenByType<TextBox>(this);
            foreach (TextBox tb in tbs)
            {
                if (tb.Text.Length < 1)
                {
                    throw new EmptyValueException(String.Format("Значение не может быть пустым."), tb);
                }
            }
        }

        //check exists connection name in combobox
        static bool IsExistsConnectionName(string NewConnName)
        {
            List<ConnectionBase> ConnList;
            try
            {
                ConnList = Common.ConnectionStrings.LoadConnectionStrings();//deserialize list of connection from file
            }
            catch
            {
                return false;
            }
            var matches = ConnList.Where(p => p.ConnName == NewConnName);
            if (matches.Count() > 0)
                return true;
            return false;
        }

        //check only digit symbols in textbox Prot
        static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
    }

    class ExistsConnectionNameException : Exception
    {
        public Control Ctrl { get; }
        public ExistsConnectionNameException(string message, Control ctrl)
            : base(message)
        {
            Ctrl = ctrl;
        }
    }

    class PortException : Exception
    {
        public Control Ctrl { get; }
        public PortException(string message, Control ctrl)
            : base(message)
        {
            Ctrl = ctrl;
        }
    }

    class EmptyValueException : Exception
    {
        public TextBox Ctrl { get; }
        public EmptyValueException(string message, TextBox ctrl)
            : base(message)
        {
            Ctrl = ctrl;
        }
    }
}
