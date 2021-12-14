    using OfficeOpenXml;
    using Rep33.Data.Report;
    using Rep33.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;

namespace Rep33.Data.WsBuilders
{
    /// <summary>
    /// Формирование страницы ReportDay
    /// </summary>
    class WsBuilderT1 : WsBuilderBase
    {
        public WsBuilderT1(ReportData ReportData, List<DataToSave> _DataToSave) : base(ReportData, _DataToSave)
        { }

        public override void FillHeader(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate)
        {
            base.FillRepDate(ws, reportDate);
        }

        public override void FillTable(ExcelWorksheet ws, ReportStructure _rs, DateTime reportDate)
        {
            foreach (var val in _rs.Placeholders.Items)
            {
                //var namedCell = ws.Names[val.Data];
                var namedCell = ws.Names.FirstOrDefault(x => x.Name == val.Data);
                if (namedCell != null && !val.IsFormula)
                {
                    decimal dval = reportData.GetValueFromQuery(val.QueryName, val.Filter, val.DataValue, val.Data);
                    namedCell.Value = dval;
                    if (!string.IsNullOrWhiteSpace(val.Data))
                        dataToSave.Add(new DataToSave() { ReportDate = reportDate, ValueName = val.Data, Value = dval });
                }
            }
            base.FormatTable(ws, _rs);
        }
    }
}
