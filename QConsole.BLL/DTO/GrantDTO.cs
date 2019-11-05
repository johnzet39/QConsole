﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.BLL.DTO
{
    public class GrantDTO
    {
        public string Table_schema { get; set; }
        public string Table_name { get; set; }
        public string Descript { get; set; }
        public Boolean IsSelect { get; set; }
        public Boolean IsUpdate { get; set; }
        public Boolean IsInsert { get; set; }
        public Boolean IsDelete { get; set; }
        public string ColumnsSelect { get; set; }
        public string ColumnsUpdate { get; set; }
        public string ColumnsInsert { get; set; }
    }

    public class GrantColumnDTO
    {
        public string Column_name { get; set; }
        public Boolean IsSelect { get; set; }
        public Boolean IsUpdate { get; set; }
        public Boolean IsInsert { get; set; }
    }
}
