using OfficeOpenXml;
using Rep33.Data.Report;
using Rep33.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rep33.Data.WsBuilders
{
    abstract class WsBuilderBase
    {
        protected readonly ReportData reportData;
        protected List<DataToSave> dataToSave;

        public WsBuilderBase(ReportData ReportData, List<DataToSave> _DataToSave)
        {
            reportData = ReportData;
            dataToSave = _DataToSave;
        }

        protected void FillRepDate(ExcelWorksheet ws, DateTime reportDate)
        {
            var cellRepDate = ws.Names["RepDate"];
            if (cellRepDate != null)
                // Дата неправильно отображается в iOS preview поэтому даты будут в виде текста
                cellRepDate.Value = reportDate.ToString("dd.MM.yyyy");
        }

        protected void FormatTable(ExcelWorksheet ws, ReportStructure _rs)
        {
            foreach (var val in _rs.Placeholders.Items)
            {
                var namedCell = ws.Names.FirstOrDefault(x => x.Name == val.Data);
                if (namedCell != null)
                {
                    ws.Row(namedCell.Start.Row).Collapsed = false;
                }
            }
        }

        public abstract void FillHeader(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate);

        public abstract void FillTable(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate);
    }
}
