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
            int i = 1;
            string letter = "";
            var lastRow = 98;
            for (int day = 1; day <= reportDate.Day; day++)
            {
                DateTime currentDate = new DateTime(reportDate.Year, reportDate.Month, day);
                letter = Common.GetColumnLetter((i++).ToString());


                /*var range = epplusWs.Workbook.Names[rangeName];
                range.Address = "YourNewRange";
                epplusWs.Workbook.Names.Remove(rangeName);
                epplusWs.Workbook.Names.Add(rangeName, range); */

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

                /* 
                 * 'Отчет за день'!Q24
currentWorksheet.Cells["C4"].Formula = "SUM(C2:C3)";             
                 * for (int j = 4; j <= lastRow; j++)
                 {
                     string cellStand = standart + j;
                     string cell = letter + j;
                     ws.Names
                 } */
            }
        }
    }
}
