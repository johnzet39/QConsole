using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL.DTO;
using QConsole.BLL.Interfaces;
using QConsole.DAL.AccessLayer.Entities;
using QConsole.DAL.AccessLayer.Repositories;
using AutoMapper;

namespace QConsole.BLL.Services
{
    public class LayerService : ILayerService
    {
        LayerRepository _layerRepository;

        public LayerService(string conn)
        {
            _layerRepository = new LayerRepository(conn);
        }

        public List<string> ChangeLayer(string tableschema, string tablename, string descript, bool? isupdater, bool? islogger)
        {
            return _layerRepository.ChangeLayer(tableschema, tablename, descript, isupdater, islogger);
        }

        public int GetCountOfPeriod(string tableshcema, string tablename, int days)
        {
            return _layerRepository.GetCountOfPeriod(tableshcema, tablename, days);
        }

        public List<LayerDTO> GetDicts()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Layer, LayerDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Layer>, List<LayerDTO>>(_layerRepository.GetDicts());
        }

        public List<LayerDTO> GetLayers()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Layer, LayerDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Layer>, List<LayerDTO>>(_layerRepository.GetLayers());
        }
    }
}
