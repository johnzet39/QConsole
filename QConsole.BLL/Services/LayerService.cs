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
using QConsole.DAL.AccessLayer.Interfaces;
using QConsole.DAL.AccessLayer.Manager;

namespace QConsole.BLL.Services
{
    public class LayerService : ILayerService
    {
        IManagerDAL _managerDAL;
        UnitOfWork _unitOfWork;
        readonly string _conn;

        public LayerService(string conn)
        {
            _conn = conn;
            _managerDAL = new ManagerDAL(_conn);
        }

        public void ChangeLayer(string tableschema, string tablename, string descript, bool? isupdater, bool? islogger)
        {
            _managerDAL.LayerAccess.ChangeLayer(tableschema, tablename, descript, isupdater, islogger);
        }

        public int GetCountOfPeriod(string tableshcema, string tablename, int days)
        {
            return _managerDAL.LayerAccess.GetCountOfPeriod(tableshcema, tablename, days);
        }

        public List<LayerDTO> GetDicts()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Layer, LayerDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Layer>, List<LayerDTO>>(_managerDAL.LayerAccess.GetDicts());
        }

        public List<LayerDTO> GetLayers()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Layer, LayerDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Layer>, List<LayerDTO>>(_managerDAL.LayerAccess.GetLayers());
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
            return mapper.Map<IEnumerable<InformationSchemaTable>, List<InformationSchemaTableDTO>>(_managerDAL.LayerAccess.GetAllTables());
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

        public List<LayerGrantsDTO> GetGrantsToLayer(string schemaname, string tablename)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<LayerGrants, LayerGrantsDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<LayerGrants>, List<LayerGrantsDTO>>(_managerDAL.LayerAccess.GetGrantsToLayer(schemaname, tablename));
        }
    }
}
