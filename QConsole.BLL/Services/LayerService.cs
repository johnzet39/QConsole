using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL.DTO;
using QConsole.BLL.Interfaces;
using QConsole.DAL.AccessLayer.Entities;
using QConsole.DAL.AccessLayer.DAO;
using AutoMapper;
using QConsole.DAL.EF.UnitOfWork;
using QConsole.DAL.EF.EDM;

namespace QConsole.BLL.Services
{
    public class LayerService : ILayerService
    {
        LayerDAO _layerRepository;
        UnitOfWork _unitOfWork;
        readonly string _conn;

        public LayerService(string conn)
        {
            _conn = conn;
            _layerRepository = new LayerDAO(_conn);
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

        public List<DictionaryDTO> GetListOfDictionaries()
        {
            _unitOfWork = new UnitOfWork(_conn);
            List<dictionaries> ListDictionaries;
            ListDictionaries = _unitOfWork.DictionariesRepository.Get().OrderBy(r => r.schema_name).OrderBy(r => r.table_name).ToList();

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<dictionaries, DictionaryDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<dictionaries>, List<DictionaryDTO>>(ListDictionaries);
        }

        public List<InformationSchemaTableDTO> GetListOfAllTables()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<InformationSchemaTable, InformationSchemaTableDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<InformationSchemaTable>, List<InformationSchemaTableDTO>>(_layerRepository.GetAllTables());
        }

        public void AddTableToDictionaries(string schemaname, string tablename)
        {
            Console.WriteLine(String.Format("{0}, {1}", schemaname, tablename));
            var dictionary = new dictionaries()
            {
                schema_name = schemaname,
                table_name = tablename
            };
            _unitOfWork = new UnitOfWork(_conn);
            _unitOfWork.DictionariesRepository.Create(dictionary);
            _unitOfWork.Save();
        }

        public void RemoveTableFromDictionaries(int id)
        {
            _unitOfWork = new UnitOfWork(_conn);
            var dictionary = _unitOfWork.DictionariesRepository.Get().Where(o => o.id == id).FirstOrDefault();
            _unitOfWork.DictionariesRepository.Remove(dictionary);
            _unitOfWork.Save();
        }
    }
}
