using OfficeOpenXml;
using Rep33.Data.Report;
using System;

namespace Rep33.Data.HeaderBuilders
{
    /// <summary>
    /// Формирование и заполнение заголовка для Dynamic
    /// </summary>
    public class HBuilderT2 : HeaderBuiiderBase
    {
        const string hRange2 = "A2:{#}2";
        const string hRange3 = "A3:{#}3";

        public override void FillHeader(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate)
        {
            base.FillRepDate(ws, reportDate);

            int i = 1;
            string letter = "";
            for (int day = 1; day <= reportDate.Day; day++)
            {
                DateTime currentDate = new DateTime(reportDate.Year, reportDate.Month, day);
                letter = Common.GetColumnLetter((i++).ToString());
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
    }
}
