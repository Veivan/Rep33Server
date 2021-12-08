using OfficeOpenXml;
using Rep33.Data.Report;
using System;

namespace Rep33.Data.HeaderBuilders
{
    public abstract class HeaderBuiiderBase
    {
        protected void FillRepDate(ExcelWorksheet ws, DateTime reportDate)
        {
            var cellRepDate = ws.Names["RepDate"];
            if (cellRepDate != null)
                // Дата неправильно отображается в iOS preview поэтому даты будут в виде текста
                cellRepDate.Value = reportDate.ToString("dd.MM.yyyy");
        }

        public abstract void FillHeader(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate);
    }
}
