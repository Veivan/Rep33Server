using OfficeOpenXml;
using Rep33.Data.Report;
using Rep33.Domain;
using System;
using System.Collections.Generic;

namespace Rep33.Data.TableBuilders
{
    abstract class TableBuiiderBase
    {
        protected readonly ReportData reportData;
        protected List<DataToSave> dataToSave;

        public TableBuiiderBase(ReportData ReportData, List<DataToSave> _DataToSave)
        {
            reportData = ReportData;
            dataToSave = _DataToSave;
        }

        public abstract void FillTable(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate);
    }
}
