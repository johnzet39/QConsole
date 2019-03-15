using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace QConsole.Ext
{
    class Paging<T>
    {
        public int PageIndex { get; set; }

        IList<T> PagedList = new List<T>();

        public IList<T> SetPaging(IList<T> ListToPage, int RecordsPerPage)
        {
            int PageGroup = PageIndex * RecordsPerPage;
            IList<T> PagedList = new List<T>();
            PagedList = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList();
            //DataTable FinalPaging = PagedTable(PagedList);
            IList<T> FinalPaging = PagedList;
            return FinalPaging;
        }

        #region ---FinalPaging, if it is DataTable---
        //private DataTable PagedTable<T>(IList<T> SourceList)
        //{
        //    Type columnType = typeof(T);
        //    DataTable TableToReturn = new DataTable();

        //    foreach (var Column in columnType.GetProperties())
        //    {
        //        TableToReturn.Columns.Add(Column.Name, Column.PropertyType);
        //    }

        //    foreach (object item in SourceList)
        //    {
        //        DataRow ReturnTableRow = TableToReturn.NewRow();
        //        foreach (var Column in columnType.GetProperties())
        //        {
        //            ReturnTableRow[Column.Name] = Column.GetValue(item);
        //        }
        //        TableToReturn.Rows.Add(ReturnTableRow);
        //    }
        //    return TableToReturn;
        //}
        #endregion ---FinalPaging, if it is DataTable---

        public IList<T> Next(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex++;
            if (PageIndex >= ListToPage.Count / RecordsPerPage)
            {
                PageIndex = ListToPage.Count / RecordsPerPage;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public IList<T> Previous(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex--;
            if (PageIndex <= 0)
            {
                PageIndex = 0;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public IList<T> First(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex = 0;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public IList<T> Last(IList<T> ListToPage, int RecordsPerPage)
        {
            PageIndex = ListToPage.Count / RecordsPerPage;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }
    }
}
