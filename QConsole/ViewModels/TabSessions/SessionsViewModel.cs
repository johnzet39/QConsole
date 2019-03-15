using QConsole.Commands;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.Models;
using QConsole.BLL.Services;
using QConsole.BLL.Interfaces;
using QConsole.BLL.DTO;
using AutoMapper;
using System.Windows.Threading;

namespace QConsole.ViewModels.TabSessions
{
    class SessionsViewModel : BaseViewModel
    {
        private readonly string _connectionString = Common.ConnectionStrings.ConnectionString;
        private static DispatcherTimer aTimer;

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

        // Constructor.
        public SessionsViewModel()
        {
            Console.WriteLine("sessions: " + this.GetHashCode());
            GetSessionsAsync();
            SetTimerStatus();
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
                Interval = TimeSpan.FromSeconds(2)
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
    }
}
