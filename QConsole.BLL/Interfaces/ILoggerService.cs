﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL.DTO;

namespace QConsole.BLL.Interfaces
{
    public interface ILoggerService
    {
        //LOG LIST main
        List<LogRowDTO> GetLogList(string ExtraQueryFull, string FirstRowsQuery);
        //build date string subquery
        string BuildExtraDateString(DateTime? dateFrom, DateTime? dateTo);
        //build 1000rows string subquery
        string BuildExtraFirstRowsString();
        //union extra subquery strings
        string UnionExtraStrings(IList<string> str);
        //get column list for combobox
        List<string> GetColumnsList();
    }
}