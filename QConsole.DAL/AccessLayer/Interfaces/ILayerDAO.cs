using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.DAL.AccessLayer.Entities;

namespace QConsole.DAL.AccessLayer.Interfaces
{
    public interface ILayerDAO
    {
        //layers
        List<Layer> GetLayers();
        //dictionaries
        List<Layer> GetDicts();
        //Change layer
        List<string> ChangeLayer(string tableschema, string tablename, string descript, bool? isupdater, bool? islogger);
        //Get count of changed rows in last days
        int GetCountOfPeriod(string tableshcema, string tablename, int days);
    }
}
