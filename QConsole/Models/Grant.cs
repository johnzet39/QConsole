﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.Models
{
    public class Grant
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

    public class GrantColumn : ICloneable
    {
        public string Column_name { get; set; }
        public Boolean IsSelect { get; set; }
        public Boolean IsUpdate { get; set; }
        public Boolean IsInsert { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
