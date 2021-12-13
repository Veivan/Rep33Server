using OfficeOpenXml;
using Rep33.Data.Report;
using Rep33.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rep33.Data.TableBuilders
{
    /// <summary>
    /// Заполнение данными страницы Dynamic
    /// </summary>
    class TBuilderT2 : TableBuiiderBase
    {
        public TBuilderT2(ReportData ReportData, List<DataToSave> _DataToSave) : base(ReportData, _DataToSave)
        { }

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
