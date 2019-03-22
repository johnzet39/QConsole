using AutoMapper;
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
        private readonly int _timerInterval = 2;

        // Constructor.
        public SessionsViewModel()
        {
            Console.WriteLine("sessions: " + this.GetHashCode());

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
                //if (SeriesCollection == null && value > 0)
                //{
                //    CreatePlot();
                //}
                //if (SeriesCollection != null && value > 0)
                    AddPlotPoint();
                OnPropertyChanged("SessionsCount");
            }
        }


        // Get sessions async
        private async void GetSessionsAsync()
        {

            Session cur_row = SelectedSession;
            await Task.Run(() => GetSessions());
            //Select selected session in datagrid
            if (cur_row != null)
            {
                SelectedSession = SessionsList.Where(p => p.Pid == cur_row.Pid).First();
            }
        }
        private void GetSessions()
        {
            ISessionService service = new SessionService(_connectionString);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SessionDTO, Session>()).CreateMapper();
            var sessions = mapper.Map<IEnumerable<SessionDTO>, List<Session>>(service.GetSessions());
            SessionsList = new ObservableCollection<Session>(sessions);
            SessionsCount = SessionsList.Count;
            //    AddPlotPoint();
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
            }
            else
            {
                RemoveTimer();
            }
        }

        //Timer for tabSession.
        private void SetTimer()
        {
            aTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(_timerInterval)
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

        private void CreatePlot()
        {
            SeriesCollection = new SeriesCollection();
            SeriesCollection.Add(new LineSeries
            {
                Title = "Cессии",
                Values = new ChartValues<int>(),
                //Values = new ChartValues<int> { SessionsCount },
                PointGeometry = DefaultGeometries.Circle,
                PointGeometrySize = 4,
                LineSmoothness = 0,
                //Stroke = new SolidColorBrush(Colors.Red),
                //Fill = new SolidColorBrush(Colors.Red)
            });

            GenerateLabels();
            YFormatter = value => value.ToString();
        }

        private void GenerateLabels()
        {
            Labels = new List<string>();
            for (int i = 0; i < 61 * _timerInterval; i=i + _timerInterval)
            {
                Labels.Add(i.ToString());
            }
        }

        private void AddPlotPoint()
        {
            var Serie1 = SeriesCollection[0];
            var values = Serie1.Values;

            if (values.Count > 60)
            {
                Serie1.Values.RemoveAt(0);
                values.Add(SessionsCount);

                Labels.RemoveAt(0);
                Labels.Add((Int32.Parse(Labels.Last()) + _timerInterval).ToString());
            }
            else
            {
                Serie1.Values.Add(SessionsCount);
            }
        }

        public SeriesCollection SeriesCollection { get; set; }
        public IList<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        #endregion
    }
}
