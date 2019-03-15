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
using System.Configuration;
using QConsole.BLL;
using QConsole.ViewModels;

namespace QConsole
{
    /// <summary>
    /// Логика взаимодействия для loginWindow.xaml
    /// </summary>
    /// 

    public partial class LoginWindow : Window
    {
        private string _conn_string;
        //private string _providerName = "Npgsql";   
        private string versionNumberString = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
            System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + "." +
            System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();

        public LoginWindow()
        {
            InitializeComponent();
            //file with connectionStrings
            Common.ConnectionStrings._connectionStringsFilePath = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    "ConnectionStrings.xml"
                    );
        }

        protected void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txbVersion.Text = versionNumberString;
            gb_NewConnectionName.Visibility = Visibility.Collapsed;
            PopulateComboBoxConnections();
            //select item by index from Settings
            cb_Connections.SelectedIndex = Properties.Settings.Default.CurrentConnectionIndex;

            tbPassword.Focus();
            tbPassword.PasswordChanged += txb_TextChanged;

        }

        //populate combobox by connections, converted to objects
        private void PopulateComboBoxConnections()
        {
            List<ConnectionBase> ConnList = new List<ConnectionBase>();
            #region ConnectionStrings from app.config
            //ConnectionStringSettingsCollection connections = System.Configuration.ConfigurationManager.ConnectionStrings;
            //foreach (ConnectionStringSettings conn in connections)
            //{
            //    if (conn.ProviderName != _providerName)
            //        continue;
            //    string connection_name = conn.Name;
            //    string connection_string = conn.ConnectionString;
            //    ConnectionBase conn_base = new ConnectionBase();
            //    conn_base.SetConnectionFromString(connection_string, connection_name);
            //    ConnList.Add(conn_base);
            //}
            //cb_Connections.ItemsSource = ConnList;
            //cb_Connections.DisplayMemberPath = "ConnName";
            #endregion
            try
            {
                ConnList = Common.ConnectionStrings.LoadConnectionStrings();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            cb_Connections.ItemsSource = ConnList;
            cb_Connections.DisplayMemberPath = "ConnName";
        }



        protected void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // OK button
        protected void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            //check selected connection iin combobox
            if (cb_Connections.SelectedIndex < 0)
            {
                //MessageBox.Show("Выберите подключение");
                Ext.UIHelper.ShowToolTip("Выберите подключение", cb_Connections, 2);
                return;
            }
            //check empty password
            if (tbPassword.Password.ToString().Length < 1)
            {
                //tbPassword.Background = Brushes.MistyRose;
                Ext.UIHelper.ShowToolTip("Введите пароль", tbPassword, 3);
                tbPassword.Focus();
                return;
            }
            ConnectionBase connBase = new ConnectionBase();
            connBase.Database = tbBD.Text;
            connBase.Host = tbHost.Text;
            connBase.Port = tbPort.Text;
            connBase.Username = tbUsername.Text;
            connBase.Password = tbPassword.Password.ToString();
            connBase.ApplicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " " + versionNumberString;
            //create ConnectionString from ConnectionBase object
            _conn_string = connBase.BuildConnectionString();
            //Test connection. If OK - show MainWondow
            Exception access_accept = connBase.TestConnectionAccess(_conn_string);
            if (access_accept == null)
            {
                this.Hide();
                string conn_stringTitle = String.Format("{4} [{0}:{1}, {2}, {3}]", connBase.Host, connBase.Port, connBase.Database, connBase.Username, connBase.ApplicationName);
                Common.ConnectionStrings.ConnectionString = _conn_string;
                //Views.MainWindow mainWindow = new Views.MainWindow();
                //mainWindow.Title = conn_stringTitle;
                //mainWindow.Show();
                ShowMainWindow(conn_stringTitle);
            }
            else
            {
                MessageBox.Show(access_accept.Message);
            }
        }

        public async void ShowMainWindow(string conn_stringTitle)
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel()
            {
                Title = conn_stringTitle
            };
                
            await displayRootRegistry.ShowModalPresentation(mainWindowViewModel);
        }

        //Populate TextBoxes after selection in ComboBox with connections
        protected void Cb_Connections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_Connections.SelectedIndex > -1)
            {
                ComboBox cb = sender as ComboBox;
                ConnectionBase connBase = cb.SelectedItem as ConnectionBase;
                tbBD.Text = connBase.Database;
                tbHost.Text = connBase.Host;
                tbPort.Text = connBase.Port;
                tbUsername.Text = connBase.Username;
                tbPassword.Focus();
            }
        }

        //remove selected connection
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            ConnectionBase connectionBase = cb_Connections.SelectedItem as ConnectionBase;
            string connName = connectionBase.ConnName;
            MessageBoxResult result = MessageBox.Show("Удалить запись " + connName + "?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                int CurrConnIndex = cb_Connections.SelectedIndex;
                RemoveSelectedConnection(connName);
                PopulateComboBoxConnections();
                //select non-empty item after delete first item in combobox
                if (CurrConnIndex - 1 < 0)
                    cb_Connections.SelectedIndex = CurrConnIndex;
                else
                    cb_Connections.SelectedIndex = CurrConnIndex - 1;
            }
        }

        //add new connection button
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ConnectionWindow NewConnWindow = new ConnectionWindow();
            NewConnWindow.ShowDialog();
            if ((bool)NewConnWindow.DialogResult.HasValue && NewConnWindow.DialogResult.Value)
            {
                //GetNewConnectionValues(NewConnWindow, out string NewConnString, out string newConnName);
                //AddNewConnection(NewConnString, newConnName);
                ConnectionBase new_conn_base = new ConnectionBase()
                {
                    Database = NewConnWindow.tbBD.Text.ToString().Trim(),
                    Host = NewConnWindow.tbHost.Text.ToString().Trim(),
                    Port = NewConnWindow.tbPort.Text.ToString().Trim(),
                    Username = NewConnWindow.tbUsername.Text.ToString().Trim(),
                    ConnName = NewConnWindow.tbNewConnectionName.Text.ToString().Trim()
                };
                AddNewConnection(new_conn_base);
            }
        }

        //edit selected connection button
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (cb_Connections.SelectedIndex < 0)
                return;
            //populate TextBoxes on Window by selected connection values
            ConnectionWindow EditConnWindow = new ConnectionWindow();
            ConnectionBase connectionBase = cb_Connections.SelectedItem as ConnectionBase;
            EditConnWindow.tbBD.Text = connectionBase.Database;
            EditConnWindow.tbHost.Text = connectionBase.Host;
            EditConnWindow.tbPort.Text = connectionBase.Port;
            EditConnWindow.tbUsername.Text = connectionBase.Username;
            EditConnWindow.tbNewConnectionName.Text = connectionBase.ConnName;
            EditConnWindow.ShowDialog();

            if ((bool)EditConnWindow.DialogResult.HasValue && EditConnWindow.DialogResult.Value)
            {
                //GetNewConnectionValues(EditConnWindow, out string NewConnString, out string newConnName);
                //ModifySelectedConnection(NewConnString, newConnName, connectionBase.ConnName);
                ConnectionBase new_conn_base = new ConnectionBase()
                {
                    Database = EditConnWindow.tbBD.Text.ToString().Trim(),
                    Host = EditConnWindow.tbHost.Text.ToString().Trim(),
                    Port = EditConnWindow.tbPort.Text.ToString().Trim(),
                    Username = EditConnWindow.tbUsername.Text.ToString().Trim(),
                    ConnName = EditConnWindow.tbNewConnectionName.Text.ToString().Trim()
                };
                ModifySelectedConnection(new_conn_base, connectionBase.ConnName);
            }
        }
        #region ConnectionStrings in app.config
        //get new values from Dialog controls
        private void GetNewConnectionValues(ConnectionWindow win, out string NewConnString, out string newConnName)
        {
            ConnectionBase new_conn_base = new ConnectionBase();
            new_conn_base.Database = win.tbBD.Text.ToString().Trim();
            new_conn_base.Host = win.tbHost.Text.ToString().Trim();
            new_conn_base.Port = win.tbPort.Text.ToString().Trim();
            new_conn_base.Username = win.tbUsername.Text.ToString().Trim();
            new_conn_base.ConnName = win.tbNewConnectionName.Text.ToString().Trim();

            NewConnString = new_conn_base.BuildConnectionString();
            newConnName = new_conn_base.ConnName;
        }
        #endregion

        //Add new connection
        private void AddNewConnection(string NewConnString, string NewConnName)
        {
            #region ConnectionStrings in app.config
            //ConnectionStringSettings cSS = new ConnectionStringSettings(NewConnName, NewConnString, _providerName);
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //config.ConnectionStrings.ConnectionStrings.Add(cSS);
            //config.Save(ConfigurationSaveMode.Modified, true);
            //ConfigurationManager.RefreshSection("connectionStrings");
            #endregion

            List<ConnectionBase> ConnList;
            try
            {
                ConnList = Common.ConnectionStrings.LoadConnectionStrings();
            }
            catch
            {
                ConnList = new List<ConnectionBase>();
            }

            ConnectionBase cb = new ConnectionBase();
            cb.SetConnectionFromString(NewConnString, NewConnName);
            ConnList.Add(cb);
            Common.ConnectionStrings.SaveConnectionStrings(ConnList);

            //populate combobox with connections
            PopulateComboBoxConnections();
            //select new connection in combobox
            var it = from ConnectionBase item in cb_Connections.Items
                     where String.Compare(item.ConnName, NewConnName) == 0
                     select item;
            cb_Connections.SelectedItem = it.FirstOrDefault() as ConnectionBase;
        }

        //Add new connection
        private void AddNewConnection(ConnectionBase cb)
        {
            List<ConnectionBase> ConnList = (List<ConnectionBase>)cb_Connections.ItemsSource;
            ConnList.Add(cb);
            Common.ConnectionStrings.SaveConnectionStrings(ConnList);
            //populate combobox with connections
            PopulateComboBoxConnections();

            //select new connection in combobox
            var it = from ConnectionBase item in cb_Connections.Items
                     where String.Compare(item.ConnName, cb.ConnName) == 0
                     select item;
            cb_Connections.SelectedItem = it.FirstOrDefault() as ConnectionBase;
        }

        //Modyfi selected connection
        private void ModifySelectedConnection(string NewConnString, string NewConnName, string OldConnName)
        {
            #region ConnectionStrings in app.config
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //config.ConnectionStrings.ConnectionStrings[OldConnName].ConnectionString = NewConnString;
            //config.ConnectionStrings.ConnectionStrings[OldConnName].Name = NewConnName;
            //config.Save(ConfigurationSaveMode.Modified, true);
            //ConfigurationManager.RefreshSection("connectionStrings");
            #endregion
            List<ConnectionBase> ConnList;
            try
            {
                ConnList = Common.ConnectionStrings.LoadConnectionStrings();
                int i = 0;
                foreach (ConnectionBase cb in ConnList)
                {
                    if (cb.ConnName == OldConnName)
                    {
                        cb.SetConnectionFromString(NewConnString, NewConnName);
                        ConnList[i] = cb;
                        break;
                    }
                    ++i;
                }
                Common.ConnectionStrings.SaveConnectionStrings(ConnList);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            //populate combobox with connections
            PopulateComboBoxConnections();
            //select new connection in combobox
            var it = from ConnectionBase item in cb_Connections.Items
                     where String.Compare(item.ConnName, NewConnName) == 0
                     select item;
            cb_Connections.SelectedItem = it.FirstOrDefault() as ConnectionBase;
        }

        //Modyfi selected connection
        private void ModifySelectedConnection(ConnectionBase cb_new, string OldConnName)
        {
            List<ConnectionBase> ConnList = (List<ConnectionBase>)cb_Connections.ItemsSource;
            ConnList[cb_Connections.SelectedIndex] = cb_new;
            Common.ConnectionStrings.SaveConnectionStrings(ConnList);
            //populate combobox with connections
            PopulateComboBoxConnections();

            //select new connection in combobox
            var it = from ConnectionBase item in cb_Connections.Items
                     where String.Compare(item.ConnName, cb_new.ConnName) == 0
                     select item;
            cb_Connections.SelectedItem = it.FirstOrDefault() as ConnectionBase;
        }

        //REMOVE selected connection from combobox.
        private void RemoveSelectedConnection(string connName)
        {
            #region ConnectionStrings in app.config
            //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //config.ConnectionStrings.ConnectionStrings.Remove(connName);
            //config.Save(ConfigurationSaveMode.Modified, true);
            //ConfigurationManager.RefreshSection("connectionStrings");
            #endregion
            List<ConnectionBase> ConnList = (List<ConnectionBase>)cb_Connections.ItemsSource;
            ConnList.RemoveAt(cb_Connections.SelectedIndex);
            Common.ConnectionStrings.SaveConnectionStrings(ConnList);

            //Find all textBoxes on container and clear values
            IEnumerable<TextBox> tbs = Ext.UIHelper.FindVisualChildrenByType<TextBox>(this);
            foreach (TextBox tb in tbs)
                tb.Clear();
            tbPassword.Clear();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.CurrentConnectionIndex = cb_Connections.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        //EVENT TextChanged for sender
        protected void txb_TextChanged(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;
                tb.Background = Brushes.White;
            }
            if (sender is PasswordBox)
            {
                PasswordBox tb = sender as PasswordBox;
                tb.Background = Brushes.White;
            }
        }


    }
}
