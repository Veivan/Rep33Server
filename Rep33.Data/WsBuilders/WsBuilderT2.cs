using OfficeOpenXml;
using Rep33.Data.Report;
using System;
using System.Linq;

namespace Rep33.Data.WsBuilders
{
    /// <summary>
    /// Формирование страницы Dynamic
    /// </summary>
    class WsBuilderT2 : WsBuilderBase
    {
        public WsBuilderT2(ReportData ReportData) : base(ReportData, null)
        { }

        public override void FillHeader(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate)
        {
            base.FillRepDate(ws, reportDate);

            string letter = "";
            for (int day = 1; day <= reportDate.Day; day++)
            {
                DateTime currentDate = new DateTime(reportDate.Year, reportDate.Month, day);
                letter = Common.GetColumnLetter((day).ToString());
                string cell = letter + "2";
                ws.Cells[cell].Value = currentDate.ToString("ddd");
                ws.Cells[cell].StyleID = ws.Cells["B2"].StyleID;
                cell = letter + "3";
                ws.Cells[cell].Value = currentDate.ToString("dd.MMM");
                ws.Cells[cell].StyleID = ws.Cells["B3"].StyleID;
            }
        }

        public override void FillTable(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate)
        {
            string letter = "";
            for (int day = 1; day < reportDate.Day; day++)
            {
                DateTime currentDate = new DateTime(reportDate.Year, reportDate.Month, day);
                letter = Common.GetColumnLetter(day.ToString());
                foreach (var val in _rs.Placeholders.Items)
                {
                    var range = ws.Names.FirstOrDefault(x => x.Name == val.Data);
                    if (range != null)
                    {
                        string cell = letter + range.Start.Row;
                        if (val.IsFormula)
                        {
                            ws.Cells[cell].FormulaR1C1 = range.FormulaR1C1;
                        }
                        else
                        {
                            ws.Cells[cell].Value = reportData.GetValueFromQuery(val.QueryName,
                                val.Filter.Replace("{#}", currentDate.ToString("MM/dd/yyyy")), val.DataValue, "");
                        }
                        ws.Cells[cell].StyleID = range.StyleID;
                    }
                }
            }
            // Заполнение последнего дня
            letter = Common.GetColumnLetter(reportDate.Day.ToString());
            foreach (var val in _rs.Placeholders.Items)
            {
                var range = ws.Names.FirstOrDefault(x => x.Name == val.Data);
                if (range != null)
                {
                    string cell = letter + range.Start.Row;
                    ws.Cells[cell].Formula = "'Отчет за день'!Q" + range.Start.Row;
                    ws.Cells[cell].StyleID = range.StyleID;
                }
            }
            base.FormatTable(ws, _rs);
        }
    }
}
