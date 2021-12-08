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
        public override void FillHeader(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate)
        {
            base.FillRepDate(ws, reportDate);


            /*            foreach (var header in _rs.Header.Rows)
                        {
                            string NextLetter = "A";
                            if (string.IsNullOrWhiteSpace(header.Caption.Data))
                            {
                                ws.Cells[header.Caption.Cell].Value = header.Caption.Text;
                            }
                            else
                            {
                                // Дата неправильно отображается в iOS preview поэтому даты будут в виде текста
                                if (header.Caption.Data == "Date") ws.Cells[header.Caption.Cell].Value = ReportDate.ToString(header.Caption.DateFormat);
                            }
                            if (header.Values != null)
                            {
                                foreach (var val in header.Values.Items)
                                {
                                    if (val.IsPrevDays) { NextLetter = BuildPrevRow(ws, val); }
                                    else
                                    {
                                        string cell = val.Cell;
                                        if (NextLetter != "") { cell = cell.Replace("{#}", NextLetter); }
                                        if (val.IsFormula == true)
                                        {
                                            ws.Cells[cell].Formula = val.Text;
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrWhiteSpace(val.DateFormat))
                                            {
                                                if (!string.IsNullOrWhiteSpace(val.Data)) ws.Cells[cell].Value = ReportDate.ToString(val.DateFormat);
                                            }
                                            else
                                            {
                                                ws.Cells[cell].Value = ReportData.GetValueFromQuery(val.QueryName, val.Filter, val.DataValue, val.Data);
                                            }
                                        }
                                    }
                                }
                            }
                            header.Cell = header.Cell.Replace("{#}", NextLetter);
                            SetStyle(ws.Cells[header.Cell], header.Style);
                        }
            */
        }
    }
}
