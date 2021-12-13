using OfficeOpenXml;
using Rep33.Data.Report;
using Rep33.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rep33.Data.TableBuilders
{
    class TBuilderT3 : TableBuiiderBase
    {
        public TBuilderT3(ReportData ReportData, List<DataToSave> _DataToSave) : base(ReportData, _DataToSave)
        { }

        public override void FillTable(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate)
        {
            string cell;
            foreach (var val in _rs.Placeholders.Items)
            {
                var range = ws.Names.FirstOrDefault(x => x.Name == val.Data);
                if (range != null && !val.IsFormula)
                {
                    // Заполнение колонки "Среднее за предыдущий месяц"
                    cell = "D" + range.Start.Row;
                    ws.Cells[cell].Value = reportData.GetValueFromQuery("PrevMonth", val.Filter, val.DataValue, "");

                    // Заполнение колонки "Cреднее месяца {#} года"
                    cell = "K" + range.Start.Row;
                    ws.Cells[cell].Value = reportData.GetValueFromQuery("PrevMonthYear", val.Filter, val.DataValue, "");

                    // Заполнение колонки "Нарастающий итог за месяц {#} года"
                    cell = "L" + range.Start.Row;
                    ws.Cells[cell].Value = reportData.GetValueFromQuery("PrevMonthYearTotal", val.Filter, val.DataValue, "");

                    // Заполнение колонки "Cреднее месяца {#} года"
                    cell = "Q" + range.Start.Row;
                    ws.Cells[cell].Value = reportData.GetValueFromQuery("Prev2MonthYear", val.Filter, val.DataValue, "");

                    // Заполнение колонки "Нарастающий итог за месяц {#} года"
                    cell = "R" + range.Start.Row;
                    ws.Cells[cell].Value = reportData.GetValueFromQuery("Prev2MonthYearTotal", val.Filter, val.DataValue, "");
                }
            }

        }
    }
}
