using OfficeOpenXml;
using Rep33.Data.WsBuilders;
using Rep33.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

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

                    var wsBuilder = WsBuilderSelector.GetBuilder(_rs.wsType, this);
                    wsBuilder.FillHeader(ws, _rs, ReportDate);
                    if (_rs.Freeze != null) 
                        ws.View.FreezePanes(_rs.Freeze.Row, _rs.Freeze.Col);
                    wsBuilder.FillTable(ws, _rs, ReportDate);
                }
                excel.Workbook.Calculate();
                excel.Workbook.Worksheets[0].View.ZoomScale = 80;
                excel.Workbook.Worksheets[1].View.ZoomScale = 80;
                excel.Workbook.Worksheets[2].View.ZoomScale = 80;

                excelbin = excel.GetAsByteArray();
            }
            return true;
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
    }
}
