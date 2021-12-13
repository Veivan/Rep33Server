using OfficeOpenXml;
using OfficeOpenXml.ConditionalFormatting;
using Rep33.Domain;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using System.Linq;
using Rep33.Data.HeaderBuilders;
using Rep33.Data.TableBuilders;

namespace Rep33.Data.Report
{
    public class ReportBuilder
    {
        private List<string> _WorksheetsFiles;
        private ReportStructure _rs;
        private List<DataToSave> _DataToSave;

        public ReportBuilder()
        {
            _WorksheetsFiles = new List<string>();
            _DataToSave = new List<DataToSave>();
            IsSaveValues = true;
        }

        public string ReportName { get; set; }
        public ReportData ReportData { get; set; }
        public string FileName { get; set; }
        public bool IsSaveValues { get; set; }
        public DateTime ReportDate { get; set; }
        public List<DataToSave> DataToSave { get { return _DataToSave; } }
        public string Error { get; set; }

        public byte[] excelbin { get; set; }
        public string ReportShablon { get; set; }

        public void AddWorksheet(string XmlForWorksheetsFile)
        {
            _WorksheetsFiles.Add(XmlForWorksheetsFile);
        }

        public bool CreateReportOld()
        {
            if (ReportData == null) return false;
            if (_WorksheetsFiles.Count <= 0) return false;

            var assembly = Assembly.GetExecutingAssembly();

            using (Stream inpStream = assembly.GetManifestResourceStream(ReportShablon))
            using (var outStream = new MemoryStream())
            using (var excel = new ExcelPackage(outStream, inpStream))
            {
                /*               foreach(var wf in _WorksheetsFiles)
                               {
                                   _rs = DeSerializeXML(wf, assembly);
                                   var ws = excel.Workbook.Worksheets.Add(_rs.Name);
                                   AddHeader(ws);
                                   ws.OutLineSummaryBelow = false;
                                   AddTable(ws);
                                   string tablecell = _rs.Table.Cell;
                                   tablecell = tablecell.Replace("{#}", Common.GetColumnLetter((ReportDate.Day).ToString()));
                                   ws.Cells[tablecell].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                   ws.Cells[tablecell].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                   ws.Cells[tablecell].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                   ws.Cells[tablecell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                   //ws.Workbook.CalcMode = ExcelCalcMode.Automatic;
                                   //ws.Cells[rs.Table.Cell].Calculate();
                                   foreach (var nf in _rs.NumberFormats)
                                   {
                                       ws.Cells[nf.Cell].Style.Numberformat.Format = nf.Text;
                                   }


                                   foreach (var cf in _rs.CoditionalFormats)
                                   {
                                       SetCoditionalFormat(ws, new ExcelAddress(cf));
                                   }
                                   foreach (var wt in _rs.WrapTexts)
                                   {
                                       ws.Cells[wt].Style.WrapText = true;
                                   }
                                   ws.Cells[ws.Dimension.Address].AutoFitColumns();
                                   foreach (var merge in _rs.Merges)
                                   {
                                       ws.Cells[merge].Merge = true;
                                   }
                                   foreach (var col in _rs.Columns)
                                   {
                                       if (col.Hide) ws.Column(col.Number).Hidden = true;
                                       if (!string.IsNullOrWhiteSpace(col.Width)) ws.Column(col.Number).Width = Convert.ToDouble(col.Width);
                                   }
                                   if (_rs.Freeze != null) ws.View.FreezePanes(_rs.Freeze.Row, _rs.Freeze.Col);
                               } 
                excel.Workbook.Calculate();
                excel.Workbook.Worksheets[1].View.ZoomScale = 80;
                excel.Workbook.Worksheets[2].View.ZoomScale = 80;
                excel.Workbook.Worksheets[3].View.ZoomScale = 80;*/
                //excel.Workbook.Worksheets[1].Row(6).Collapsed = false;
                //excel.Workbook.Worksheets[1].Row(10).Collapsed = false;
                //excel.Workbook.Worksheets[1].Row(35).Collapsed = false;
                //excel.Workbook.Worksheets[1].Row(40).Collapsed = false;

                excelbin = excel.GetAsByteArray();
            }
            return true;
        }

        public bool CreateReport()
        {
            if (ReportData == null) return false;
            if (_WorksheetsFiles.Count <= 0) return false;

            var assembly = Assembly.GetExecutingAssembly();

            using (Stream inpStream = assembly.GetManifestResourceStream(ReportShablon))
            using (var outStream = new MemoryStream())
            using (var excel = new ExcelPackage(outStream, inpStream))
            {
                foreach (var wf in _WorksheetsFiles)
                {
                    _rs = DeSerializeXML(wf, assembly);
                    var ws = excel.Workbook.Worksheets[_rs.Name];
                    ws.OutLineSummaryBelow = false;

                    var hBuilder = HBuilderSelector.GetBuilder(_rs.wsType);
                    hBuilder.FillHeader(ws, _rs, ReportDate);

                    var tBuilder = TBuilderSelector.GetBuilder(_rs.wsType, this);
                    tBuilder.FillTable(ws, _rs, ReportDate);


                    /*                    AddTable(ws);
                                        string tablecell = _rs.Table.Cell;
                                        tablecell = tablecell.Replace("{#}", Common.GetColumnLetter((ReportDate.Day).ToString()));
                                        ws.Cells[tablecell].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        ws.Cells[tablecell].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        ws.Cells[tablecell].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        ws.Cells[tablecell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                                        foreach (var nf in _rs.NumberFormats)
                                        {
                                            ws.Cells[nf.Cell].Style.Numberformat.Format = nf.Text;
                                        }
                                        foreach (var cf in _rs.CoditionalFormats)
                                        {
                                            SetCoditionalFormat(ws, new ExcelAddress(cf));
                                        }
                                        foreach (var wt in _rs.WrapTexts)
                                        {
                                            ws.Cells[wt].Style.WrapText = true;
                                        }
                                        ws.Cells[ws.Dimension.Address].AutoFitColumns();
                                        foreach (var merge in _rs.Merges)
                                        {
                                            ws.Cells[merge].Merge = true;
                                        }
                                        foreach (var col in _rs.Columns)
                                        {
                                            if (col.Hide) ws.Column(col.Number).Hidden = true;
                                            if (!string.IsNullOrWhiteSpace(col.Width)) ws.Column(col.Number).Width = Convert.ToDouble(col.Width);
                                        }
                                        if (_rs.Freeze != null) ws.View.FreezePanes(_rs.Freeze.Row, _rs.Freeze.Col); */
                }
                excel.Workbook.Calculate();
                excel.Workbook.Worksheets[0].View.ZoomScale = 80;
                excel.Workbook.Worksheets[1].View.ZoomScale = 80;
                excel.Workbook.Worksheets[2].View.ZoomScale = 80;

                excelbin = excel.GetAsByteArray();
            }
            return true;
        }

        private void AddTable(ExcelWorksheet ws)
        {
            foreach (var row in _rs.Table.Rows)
            {
                int level;
                string NextLetter = "A";
                ws.Cells[row.Caption.Cell].Value = row.Caption.Text;
                if (row.Values != null)
                {
                    foreach (var val in row.Values.Items)
                    {
                        if (val.IsPrevDays) { NextLetter = BuildPrevRow(ws, val); }
                        else
                        {
                            string cell = val.Cell;
                            if (NextLetter != "") { cell = cell.Replace("{#}", NextLetter); }
                            if (string.IsNullOrWhiteSpace(val.DateFormat))
                            {
                                ws.Cells[cell].Style.Numberformat.Format = "0";
                            }
                            if (val.IsFormula == true)
                            {
                                ws.Cells[cell].Formula = val.Text;
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(val.DateFormat))
                                {
                                    if (!string.IsNullOrWhiteSpace(val.Data))
                                    {
                                        if (val.Data == "ContainDate")
                                        {
                                            ws.Cells[cell].Value = val.Text.Replace("{#}", ReportDate.AddYears(val.AddYear).ToString(val.DateFormat));
                                        }
                                        else
                                        {
                                            ws.Cells[cell].Value = ReportDate.ToString(val.DateFormat);
                                        }
                                    }
                                }
                                else
                                {
                                    if (val.IsNotConvert)
                                    {
                                        try
                                        {
                                            decimal dd = Convert.ToDecimal(val.Text);
                                            ws.Cells[cell].Value = dd;
                                        }
                                        catch
                                        {
                                            ws.Cells[cell].Value = val.Text;
                                        }
                                    }
                                    else
                                    {
                                        decimal dval = ReportData.GetValueFromQuery(val.QueryName, val.Filter, val.DataValue, val.Data);
                                        ws.Cells[cell].Value = dval;
                                        //ws.Cells[cell].Value = val.Data;
                                        if (!string.IsNullOrWhiteSpace(val.Data)) _DataToSave.Add(new DataToSave() { ReportDate = ReportDate, ValueName = val.Data, Value = dval });
                                    }
                                }
                            }
                        }
                    }
                }
                row.Cell = row.Cell.Replace("{#}", NextLetter);
                level = SetStyle(ws.Cells[row.Cell], row.Style);
                ws.Row(ws.Cells[row.Cell].Start.Row).OutlineLevel = level;
                ws.Row(ws.Cells[row.Cell].Start.Row).Collapsed = row.Collapsed;
            }
        }

        private void AddHeader(ExcelWorksheet ws)
        {
            foreach (var header in _rs.Header.Rows)
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
        }

        // Read XML from disk
        private ReportStructure DeSerializeXML(string wf)
        {
            ReportStructure rs = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ReportStructure));
            StreamReader reader = new StreamReader(wf);
            rs = (ReportStructure)serializer.Deserialize(reader);
            reader.Close();
            return rs;
        }

        // Read XML from embedded recourse
        private ReportStructure DeSerializeXML(string wf, Assembly assembly)
        {
            ReportStructure rs = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ReportStructure));

            using (Stream stream = assembly.GetManifestResourceStream(wf))
            using (StreamReader reader = new StreamReader(stream))
            {
                rs = (ReportStructure)serializer.Deserialize(reader);
                reader.Close();
            }
            return rs;
        }

        private string BuildPrevRow(ExcelWorksheet ws, Value val)
        {
            int i = 1;
            for (int day = 1; day < ReportDate.Day; day++)
            {
                DateTime currentDate = new DateTime(ReportDate.Year, ReportDate.Month, day);
                string letter = Common.GetColumnLetter((i++).ToString());
                string cell = val.Cell.Replace("{#}", letter);
                if (string.IsNullOrWhiteSpace(val.DateFormat))
                {
                    ws.Cells[cell].Style.Numberformat.Format = "0";
                }
                if (val.IsFormula == true)
                {
                    ws.Cells[cell].Formula = val.Text.Replace("{#}", letter);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(val.DateFormat))
                    {
                        ws.Cells[cell].Value = currentDate.ToString(val.DateFormat);
                    }
                    else
                    {
                        ws.Cells[cell].Value = ReportData.GetValueFromQuery(val.QueryName, val.Filter.Replace("{#}", currentDate.ToString("MM/dd/yyyy")), val.DataValue, val.Data);
                    }
                }
            }
            return Common.GetColumnLetter((i).ToString());
        }

        private void SetCoditionalFormat(ExcelWorksheet ws, ExcelAddress addr)
        {
            //ExcelAddress addr = new ExcelAddress("Q4:Q76");
            var ic = ws.ConditionalFormatting.AddThreeIconSet(addr, eExcelconditionalFormatting3IconsSetType.Arrows);
            ic.Icon1.Type = eExcelConditionalFormattingValueObjectType.Percent;
            ic.Icon1.Value = 0;
            ic.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
            ic.Icon2.Value = 0;
            ic.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;
            ic.Icon3.Value = 0;
            var node = ic.Node.ChildNodes[0].ChildNodes[2];
            var attr = node.OwnerDocument.CreateAttribute("gte");
            attr.Value = "0";
            node.Attributes.Append(attr);
        }

        private int SetStyle(ExcelRange er, string StyleName)
        {
            int level = 0;
            switch (StyleName)
            {
                case "Header1":
                    SetHeader1Style(er);
                    break;
                case "Header2":
                    SetHeader2Style(er);
                    break;
                case "Header3":
                    SetHeader3Style(er);
                    level = 1;
                    break;
                case "Header4":
                    SetHeader4Style(er);
                    level = 2;
                    break;
                case "Header5":
                    SetHeader5Style(er);
                    level = 3;
                    break;
                case "Header6":
                    SetHeader6Style(er);
                    level = 4;
                    break;
            }
            return level;
        }

        /// <summary>
        /// Стили для строк
        /// </summary>
        /// <param name="er">Диапозон ячеек</param>
        private void SetHeader1Style(ExcelRange er)
        {
            er.Style.Font.Size = 12;
            er.Style.Font.Bold = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }

        private void SetHeader2Style(ExcelRange er)
        {
            er.Style.Font.Size = 12;
            er.Style.Font.Bold = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }

        private void SetHeader3Style(ExcelRange er)
        {
            er.Style.Font.Size = 11;
            er.Style.Font.Bold = true;
            er.Style.Font.Italic = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            er[er.Start.Row, er.Start.Column].Style.Indent = 2;
        }

        private void SetHeader4Style(ExcelRange er)
        {
            er.Style.Font.Size = 11;
            er.Style.Font.Bold = true;
            er.Style.Font.Italic = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            er[er.Start.Row, er.Start.Column].Style.Indent = 3;
        }

        private void SetHeader5Style(ExcelRange er)
        {
            er.Style.Font.Size = 10;
            er.Style.Font.Italic = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            er[er.Start.Row, er.Start.Column].Style.Indent = 5;
        }

        private void SetHeader6Style(ExcelRange er)
        {
            er.Style.Font.Size = 10;
            er.Style.Font.Italic = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            er[er.Start.Row, er.Start.Column].Style.Indent = 6;
        }
    }
}
