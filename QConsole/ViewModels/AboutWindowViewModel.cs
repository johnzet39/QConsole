using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.ViewModels
{
    class AboutWindowViewModel : BaseViewModel
    {

        private string _nameProgram;
        public string NameProgram
        {
            get => _nameProgram;
            set
            {
                _nameProgram = value;
                OnPropertyChanged(("NameProgram"));
            }
        }

        private string _versionProgram;
        public string VersionProgram
        {
            get => _versionProgram;
            set
            {
                _versionProgram = value;
                OnPropertyChanged(("VersionProgram"));
            }
        }

        private string _godName;
        public string GodName
        {
            get => _godName;
            set
            {
                _godName = value;
                OnPropertyChanged(("GodName"));
            }
        }

        private string _mail;
        public string Mail
        {
            get => _mail;
            set
            {
                _mail = value;
                OnPropertyChanged(("Mail"));
            }
        }

        private string _mailName;
        public string MailName
        {
            get => _mailName;
            set
            {
                _mailName = value;
                OnPropertyChanged(("MailName"));
            }
        }

        private string _site;
        public string Site
        {
            get => _site;
            set
            {
                _site = value;
                OnPropertyChanged(("Site"));
            }
        }

        private string _siteName;
        public string SiteName
        {
            get => _siteName;
            set
            {
                _siteName = value;
                OnPropertyChanged(("SiteName"));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AboutWindowViewModel()
        {
            VersionProgram = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            NameProgram = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString();
            GodName = "Златанов Е.Г.";
            Mail = "mailto:johnzet@yandex.ru";
            MailName = "johnzet@yandex.ru";
            Site = "https://johnzet39.github.io/apps/qconsole/qconsole.html";
            SiteName = "johnzet39.github/qconsole";
        }

    }
}
