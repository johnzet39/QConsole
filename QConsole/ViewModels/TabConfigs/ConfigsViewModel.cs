using QConsole.BLL.Interfaces;
using QConsole.BLL.Services;
using QConsole.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QConsole.ViewModels.TabConfigs
{
    class ConfigsViewModel : BaseViewModel
    {
        
        public ConfigsViewModel()
        {
            FilePath = Common.CurrentConfigurationStatic.CurrentPath;
            OpenFile();
        }

        bool isFileExists;


        private RelayCommand saveFileCommand;
        public RelayCommand SaveFileCommand
        {
            get
            {
                return saveFileCommand ??
                  (saveFileCommand = new RelayCommand(obj =>
                  {
                      SaveFile();
                  }));
            }
        }

        private RelayCommand refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                return refreshCommand ??
                  (refreshCommand = new RelayCommand(obj =>
                  {
                      if (MessageBox.Show("Несохранненные данные будут утеряны. Продолжить?", 
                                            "Подтверждение", 
                                            MessageBoxButton.OKCancel, 
                                            MessageBoxImage.Question) == MessageBoxResult.OK)
                        OpenFile();
                  }));
            }
        }

        private void SaveFile()
        {
            string text = FileText;
            try
            {
                var fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    streamWriter.Write(text);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            IsSaveButtonEnabled = false;
            Ext.LogPanel.PrintLog($"Файл {FilePath} сохранен");
        }

        private void OpenFile()
        {
            FileText = null;
            try
            {
                string text;
                var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();
                }
                isFileExists = true;
                FileText = text;

            }
            catch (FileNotFoundException)
            {
                isFileExists = false;
                FileText = "Файл не найден";
            }
        }
        

        private string _filePath;
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        private string _fileText;
        public string FileText
        {
            get
            {
                return _fileText;
            }
            set
            {
                if (_fileText == null)
                {
                    IsSaveButtonEnabled = false;
                }
                else if (!isFileExists)
                {
                    IsSaveButtonEnabled = false;
                }
                else
                {
                    IsSaveButtonEnabled = true;
                }

                _fileText = value;
                OnPropertyChanged("FileText");
            }
        }

        private bool _IsSaveButtonEnabled;
        public bool IsSaveButtonEnabled
        {
            get { return _IsSaveButtonEnabled; }
            set
            {
                _IsSaveButtonEnabled = value;
                OnPropertyChanged("IsSaveButtonEnabled");
            }
        }
    }


    
}
