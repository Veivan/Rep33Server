using OfficeOpenXml;
using Rep33.Data.Report;
using Rep33.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rep33.Data.WsBuilders
{
    /// <summary>
    /// Формирование страницы Dynamic
    /// </summary>
    class WsBuilderT2 : WsBuilderBase
    {
        const string hRange2 = "A2:{#}2";
        const string hRange3 = "A3:{#}3";

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
                cell = letter + "3";
                ws.Cells[cell].Value = currentDate.ToString("dd.MMM");
            }
            var rng = hRange2.Replace("{#}", letter);
            RepStyler.SetStyle(ws.Cells[rng], "Header2");
            rng = hRange3.Replace("{#}", letter);
            RepStyler.SetStyle(ws.Cells[rng], "Header1");
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
                }
            }
        }
    }
}
