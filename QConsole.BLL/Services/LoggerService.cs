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
    public class LoggerService : ILoggerService
    {
        LoggerRepository _loggerRepository;

        public LoggerService(string conn)
        {
            _loggerRepository = new LoggerRepository(conn);
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

        public List<LogRowDTO> GetLogList(string ExtraQueryFull, string FirstRowsQuery)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<LogRow, LogRowDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<LogRow>, List<LogRowDTO>>(_loggerRepository.GetLogList(ExtraQueryFull, FirstRowsQuery));
        }

        public string UnionExtraStrings(IList<string> str)
        {
            return _loggerRepository.UnionExtraStrings(str);
        }
    }
}
