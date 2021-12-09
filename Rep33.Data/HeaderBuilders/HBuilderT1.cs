using OfficeOpenXml;
using Rep33.Data.Report;
using System;

namespace Rep33.Data.HeaderBuilders
{
    /// <summary>
    /// Заполнение заголовка для ReportDay
    /// </summary>
    class HBuilderT1 : HeaderBuiiderBase
    {
        public override void FillHeader(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate)
        {
            base.FillRepDate(ws, reportDate);
        }
    }
}