using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.Models
{
    public interface ITab
    {
        string Name { get; set; }
        string IconPath { get; set; }
    }

    public abstract class Tab : ITab
    {
        public Tab()
        {

        }

        public string Name { get; set; }
        public string IconPath { get; set; }
    }
}
