using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL.DTO;

namespace QConsole.BLL.Interfaces
{
    public interface ILayerService
    {
        //layers
        List<LayerDTO> GetLayers();
        //dictionaries
        List<LayerDTO> GetDicts();
        //Change layer
        List<string> ChangeLayer(string tableschema, string tablename, string descript, bool? isupdater, bool? islogger);
        //Get count of changed rows in last days
        int GetCountOfPeriod(string tableshcema, string tablename, int days);
    }
}
