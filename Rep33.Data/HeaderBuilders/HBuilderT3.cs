using OfficeOpenXml;
using Rep33.Data.Report;
using System;

namespace Rep33.Data.HeaderBuilders
{
    /// <summary>
    /// Формирование и заполнение заголовка для Comparison
    /// </summary>
    class HBuilderT3 : HeaderBuiiderBase
    {
        public override void FillHeader(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate)
        {
            base.FillRepDate(ws, reportDate);

            string[] h1 = { "AvgMonthDevTonn", "AvgMonthDevPerc", "AvgMonth", "ItogMonth", "ItogDevTonn", "ItogDevPerc" };
            string[] h2 = { "AvgMonthDevPrev", "AvgMonthPrev", "ItogMonthPrev", "ItogDevPrev" };

            var range = ws.Names["RepDateHead"];
            if (range != null)
                range.Value = reportDate.ToString("dd.MM.yyyy");

            foreach (var header in h1)
            {
                range = ws.Names[header];
                if (range != null)
                    range.Value = ((string)range.Value).Replace("{#}", (reportDate.Year - 1).ToString());
            }

            foreach (var header in h2)
            {
                range = ws.Names[header];
                if (range != null)
                    range.Value = ((string)range.Value).Replace("{#}", (reportDate.Year - 2).ToString());
            }

            ws.View.FreezePanes(_rs.Freeze.Row, _rs.Freeze.Col);
        }
    }
}
