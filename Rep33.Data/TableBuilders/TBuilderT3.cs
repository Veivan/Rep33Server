using OfficeOpenXml;
using Rep33.Data.Report;
using Rep33.Domain;
using System;
using System.Collections.Generic;

namespace Rep33.Data.TableBuilders
{
    class TBuilderT3 : TableBuiiderBase
    {
        public TBuilderT3(ReportData ReportData, List<DataToSave> _DataToSave) : base(ReportData, _DataToSave)
        { }

        public override void FillTable(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate)
        {
           // throw new NotImplementedException();
        }
    }
}
