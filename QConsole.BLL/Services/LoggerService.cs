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
using QConsole.DAL.EF.EDM;
using QConsole.DAL.EF.UnitOfWork;
using AutoMapper;
using QConsole.DAL.AccessLayer.Interfaces;

namespace QConsole.BLL.Services
{
    public class LoggerService : ILoggerService
    {
        ILoggerRepository _loggerRepository;
        UnitOfWork _unitOfWork;
        string Conn;

        public LoggerService(string conn)
        {
            _loggerRepository = new LoggerRepository(conn);
            _unitOfWork = new UnitOfWork(conn);
            Conn = conn;
        }

        public string BuildExtraDateString(DateTime? dateFrom, DateTime? dateTo)
        {
            return _loggerRepository.BuildExtraDateString(dateFrom, dateTo);
        }

        public string BuildExtraFirstRowsString()
        {
            return _loggerRepository.BuildExtraFirstRowsString();
        }

        public List<string> GetColumnsList()
        {
            return _loggerRepository.GetColumnsList();
        }

        //public List<LogRowDTO> GetLogList(string ExtraQueryFull, string FirstRowsQuery)
        //{
        //    var mapper = new MapperConfiguration(cfg => cfg.CreateMap<LogRow, LogRowDTO>()).CreateMapper();
        //    return mapper.Map<IEnumerable<LogRow>, List<LogRowDTO>>(_loggerRepository.GetLogList(ExtraQueryFull, FirstRowsQuery));
        //}

        public string UnionExtraStrings(IList<string> str)
        {
            return _loggerRepository.UnionExtraStrings(str);
        }

        public List<LogRowDTO> GetLogList(string ExtraQueryFull, string FirstRowsQuery)
        {
            if (ExtraQueryFull != null && ExtraQueryFull.Trim().Length == 0 && FirstRowsQuery != null && FirstRowsQuery.Trim().Length == 0)
            {
                //EF DAL
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<logtable, LogRowDTO>()).CreateMapper();
                return mapper.Map<IEnumerable<logtable>, List<LogRowDTO>>(_unitOfWork.LogtableRepository.GetAllOrdered());
            }
            else
            {
                //AccessLayer DAL
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<LogRow, LogRowDTO>()).CreateMapper();
                return mapper.Map<IEnumerable<LogRow>, List<LogRowDTO>>(_loggerRepository.GetLogList(ExtraQueryFull, FirstRowsQuery));
            }
        }

        public int GetCountByOperation(string operation, int period)
        {
            var count = _unitOfWork.LogtableRepository.GetCountByOperation(operation, period);
            return count;
        }

        public int GetCountInserts(string schema, string layer, int period)
        {
            var count = _unitOfWork.LogtableRepository.GetCountInserts(schema, layer, period);
            return count;
        }

        public int GetCountInsertsMonth(string schema, string layer, int month, int year)
        {
            var count = _unitOfWork.LogtableRepository.GetCountInsertsMonth(schema, layer, month, year);
            return count;
        }
    }
}
