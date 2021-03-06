﻿using AutoMapper;
using LiveCharts.Wpf;
using LiveCharts;
using QConsole.BLL.DTO;
using QConsole.BLL.Interfaces;
using QConsole.BLL.Services;
using QConsole.Commands;
using QConsole.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System;
using System.Windows.Media;

namespace QConsole.ViewModels.TabSessions
{
    class SessionsViewModel : BaseViewModel
    {
        private readonly string _connectionString = Common.ConnectionStrings.ConnectionString;
        private static DispatcherTimer aTimer;
        private DateTime _startTime;

        // Constructor.
        public SessionsViewModel()
        {
            Console.WriteLine("sessions: " + this.GetHashCode());
            _startTime = DateTime.Now;

            CreatePlot();
            GetSessionsAsync();
            SetTimerStatus();
            
        }

        // Refresh button command.
        private RelayCommand refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                return refreshCommand ??
                  (refreshCommand = new RelayCommand(obj =>
                  {
                      RefreshTab();
                  }));
            }
        }

        // Timer toggle button command.
        private RelayCommand timerCommand;
        public RelayCommand TimerCommand
        {
            get
            {
                return timerCommand ??
                  (timerCommand = new RelayCommand(obj =>
                  {
                      TimerExecute(obj);
                  }));
            }
        }


        // Session list for datagrid.
        private ObservableCollection<Session> _sessionsList;
        public ObservableCollection<Session> SessionsList
        {
            get => _sessionsList;
            set
            {
                _sessionsList = value;
                OnPropertyChanged(("SessionsList"));
            }
        }

        //Selected Session
        private Session _selectedSession;
        public Session SelectedSession
        {
            get { return _selectedSession; }
            set
            {
                _selectedSession = value;
                OnPropertyChanged("SelectedSession");
            }
        }

        //SessionsCount
        private int _sessionsCount;
        public int SessionsCount
        {
            get { return _sessionsCount; }
            set
            {
                _sessionsCount = value;
                App.Current.Dispatcher.Invoke(() =>
                {
                    AddPlotPoint();
                });
                OnPropertyChanged("SessionsCount");
            }
        }

        //Period
        private int _timerPeriod;
        public int TimerPeriod
        {
            get {
                _timerPeriod = Int32.Parse(Properties.Settings.Default.ButtonTimerPeriod);
                return _timerPeriod;
            }
            set
            {
                aTimer.Interval = TimeSpan.FromSeconds(value);
                Properties.Settings.Default.ButtonTimerPeriod = value.ToString();
                _timerPeriod = value;
                OnPropertyChanged("TimerPeriod");
            }
        }


        // Get sessions async
        private async void GetSessionsAsync()
        {

            Session cur_row = SelectedSession;
            await Task.Run(() => GetSessions());
            if (cur_row != null)
            {
                SelectedSession = SessionsList.Where(p => p.Pid == cur_row.Pid).FirstOrDefault();
            }
        }
        private void GetSessions()
        {
            try
            {
                ISessionService service = new SessionService(_connectionString);
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SessionDTO, Session>()).CreateMapper();
                var sessions = mapper.Map<IEnumerable<SessionDTO>, List<Session>>(service.GetSessions());
                SessionsList = new ObservableCollection<Session>(sessions);
                SessionsCount = SessionsList.Count;
            }
            catch (Exception e)
            {
                RemoveTimer();
                Properties.Settings.Default.ButtonTimerChecked = false;
                SessionsList = null;
                MessageBox.Show(e.Message, "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RefreshTab()
        {
            GetSessionsAsync();
        }

        private void SetTimerStatus()
        {
            object state = Properties.Settings.Default.ButtonTimerChecked;
            TimerExecute(state);
        }

        private void TimerExecute(object state)
        {
            if ((bool)state)
            {
                SetTimer(); ;

                SetEnabledPlot();//Change color
            }
            else
            {
                RemoveTimer();

                SetDisabledPlot();//Change color
            }
        }

        //Timer for tabSession.
        private void SetTimer()
        {
            aTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(TimerPeriod)
            };
            aTimer.Tick += OnTimedEvent;
            aTimer.Start();
        }

        //Remove timer.
        private void RemoveTimer()
        {
            if (aTimer != null)
            {
                aTimer.Stop();
                aTimer = null;
            }
        }

        //Timer Tick EVENT.
        private void OnTimedEvent(Object source, EventArgs e)
        {
            RefreshTab();
        }


        #region Plot

        // #####################
        // ####### PLOT ########
        // #####################

        private readonly int _PlotPointsCount = 100;

        private void CreatePlot()
        {
            SeriesCollection = new SeriesCollection();
            SeriesCollection.Add(new LineSeries
            {
                Title = "Cессии",
                Values = new ChartValues<int>(GenerateStartValues()),
                PointGeometry = DefaultGeometries.Circle,
                PointGeometrySize = 3.5,
                LineSmoothness = 0,
                StrokeThickness = 1.5
            });

            GenerateLabels();
            YFormatter = value => value.ToString();
        }

        private void SetEnabledPlot()
        {
            var Serie1 = SeriesCollection[0] as LineSeries;
            Serie1.Fill = new SolidColorBrush
            {
                Color = System.Windows.Media.Color.FromArgb(50 , 33, 149, 242)
            };
            Serie1.Stroke = new SolidColorBrush
            {
                Color = System.Windows.Media.Color.FromArgb(190, 33, 149, 242)
            };
        }

        private void SetDisabledPlot()
        {
            var Serie1 = SeriesCollection[0] as LineSeries;
            Serie1.Fill = new SolidColorBrush
            {
                Color = System.Windows.Media.Color.FromArgb(50, 150, 150, 150)
            };
            Serie1.Stroke = new SolidColorBrush
            {
                Color = System.Windows.Media.Color.FromArgb(150, 33, 149, 242)
            };
        }

        private IList<int> GenerateStartValues()
        {
            IList<int> values = new List<int>();
            for (int i = 1; i <=_PlotPointsCount; i++)
            {
                values.Add(-200);
            }
            return values;
        }

        private void GenerateLabels()
        {
            Labels = new List<string>();

            for (int i = 1; i <= _PlotPointsCount; i++)
            {
                Labels.Add(" ");
            }
        }

        private void AddPlotPoint()
        {
            var Serie1 = SeriesCollection[0];
            var values = Serie1.Values;

            Serie1.Values.RemoveAt(0);
            values.Add(SessionsCount);

            string newlabel;
            //newlabel = GetNewLabelFromDuration();
            newlabel = GetLabelFromTimeNow();

            Labels.RemoveAt(0);
            Labels.Add(newlabel);
        }

        private string GetNewLabelFromDuration()
        {
            double countSeconds;
            countSeconds = DateTime.Now.Subtract(_startTime).TotalSeconds;
            TimeSpan timeFromSeconds = TimeSpan.FromSeconds(countSeconds);
            string resultString = string.Format("{0:D2}.{1:D2}:{2:D2}:{3:D2}", 
                                                    timeFromSeconds.Days, 
                                                    timeFromSeconds.Hours, 
                                                    timeFromSeconds.Minutes, 
                                                    timeFromSeconds.Seconds);
            return resultString;
        }

        private string GetLabelFromTimeNow()
        {
            string resultString = DateTime.Now.ToString("HH:mm:ss");
            return resultString;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public IList<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        #endregion Plot
    }
}
