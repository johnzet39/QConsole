﻿using QConsole.Commands;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using QConsole.Models;
using QConsole.BLL.Services;
using QConsole.BLL.Interfaces;
using System.Windows;
using System.Text.RegularExpressions;

namespace QConsole.ViewModels.TabLayers
{
    class LayerPropertyWindowViewModel : BaseViewModel
    {
        private ILayerService layerService;
        private readonly string _connectionString = Common.ConnectionStrings.ConnectionString;
        DisplayRootRegistry DisplayRootRegistry;

        private string Tableschema;
        private string _oldDescription;
        private bool? _oldIsUpdater;
        private bool? _oldIsAudit;

        public bool DialogResult = false;


        // Grant role button command.
        private RelayCommand _okCommand;
        public RelayCommand OkCommand
        {
            get
            {
                return _okCommand ??
                  (_okCommand = new RelayCommand(obj =>
                  {
                      OkButton(obj);
                  }));
            }
        }

        //Username
        private string  _tablename;
        public string Tablename
        {
            get => _tablename;
            set
            {
                _tablename = value;
                OnPropertyChanged(("Tablename"));
            }
        }

        //Description
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(("Description"));
            }
        }

        //IsUpdater
        private bool? _isIpdater;
        public bool? IsUpdater
        {
            get => _isIpdater;
            set
            {
                _isIpdater = value;
                OnPropertyChanged(("IsUpdater"));
            }
        }

        //IsAudit
        private bool? _isAudit;
        public bool? IsAudit
        {
            get => _isAudit;
            set
            {
                _isAudit = value;
                OnPropertyChanged(("IsAudit"));
            }
        }




        /// <summary>
        /// Costructor for Edit mode.
        /// </summary>
        public LayerPropertyWindowViewModel(Layer currentLayer, DisplayRootRegistry displayRootRegistry)
        {
            DisplayRootRegistry = displayRootRegistry;

            Tableschema = currentLayer.Table_schema;
            Tablename = currentLayer.Table_name;
            Description = currentLayer.Descript;
            _oldDescription = Description;
            IsUpdater = currentLayer.Isupdater;
            _oldIsUpdater = IsUpdater;
            IsAudit = currentLayer.Islogger;
            _oldIsAudit = IsAudit;

        }

        private void OkButton(object parameter)
        {
            try
            {
                layerService = new LayerService(_connectionString);
                var result = layerService.ChangeLayer(
                    Tableschema,
                    Tablename,
                    (Description != _oldDescription) ? Description : null,
                    (IsUpdater != _oldIsUpdater) ? IsUpdater : null,
                    (IsAudit != _oldIsAudit) ? IsAudit : null
                    );

                Ext.LogPanel.PrintLog((List<string>)result);
                

            }
            catch (Exception ex)
            {
                Ext.LogPanel.PrintLog(ex.Message.ToString());
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            this.CloseWindow();
        }

        private void CloseWindow()
        {
            DisplayRootRegistry.HidePresentation(this);
        }


    }
}
