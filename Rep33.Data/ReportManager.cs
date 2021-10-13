﻿using Serilog;
using System;
using System.Collections.Generic;
using System.IO;


namespace Rep33.Data
{
    public class ReportManager
    {
        private Common.RepKind state;
        private bool IsSave;
        private bool UseSavedData;

        private List<byte> excelbin = new List<byte>();


        /// <summary>
        /// Constructor used with Common.RepKind.Admin
        /// </summary>
        /// <param name="_state"></param>
        /// <param name="isSave"></param>
        /// <param name="useSavedData"></param>
        public ReportManager(Common.RepKind _state, bool isSave, bool useSavedData)
        {
            this.state = _state;
            this.IsSave = isSave;
            this.UseSavedData = useSavedData;
        }

        /// <summary>
        /// Constructor used with Common.RepKind.Manual & Auto
        /// </summary>
        /// <param name="_state"></param>
        public ReportManager(Common.RepKind _state)
        {
            this.state = _state;
            if (_state == Common.RepKind.Manual)
            {
                this.IsSave = false;
                this.UseSavedData = true;
            }
            else
            {
                this.IsSave = true;
                this.UseSavedData = false;
            }
        }

        public bool CreateReport(DateTime rd)
        {
            excelbin.Clear();
            var data = new ReportData("reporter", "RepTi87BnVuy21", "(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=orabase.mcargo)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=CHAOS)))", "MASTER", "all%work", "partner", "172.30.80.49");

            //var data = new ReportData("reporter", "RepTi87BnVuy21", "(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=10.80.15.3)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=CHAOS)))", "MASTER", "all%work", "partner", "10.80.0.48");
            //DB.ReportData data = new DB.ReportData("reporter", "RepTi87BnVuy21", "(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=10.80.15.3)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=CHAOS)))", "MASTER", "all%work", "partner", "172.30.80.49");
            try
            {
                if (!string.IsNullOrWhiteSpace(data.Error))
                {
                    Log.Error($"Подключение к БД:{data.Error}");
                    return false; 
                }
                data.ReportDate = rd;
                data.UseSavedData = UseSavedData;
                if (UseSavedData && !data.GetSavedData())
                {
                    Log.Error($"Загрузка данных:{data.Error}");
                    return false;
                }
                if (!data.RunQueries())
                {
                    return false;
                }

                Report.ReportBuilder rpt = new Report.ReportBuilder();
                Log.Information("Загрузка шаблонов");
                // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
                rpt.AddWorksheet("Rep33.Data.ReportsWorksheets.ReportDay.xml");
                rpt.AddWorksheet("Rep33.Data.ReportsWorksheets.Dynamic.xml");
                rpt.AddWorksheet("Rep33.Data.ReportsWorksheets.Comparison.xml");
                rpt.ReportDate = rd;
                rpt.ReportData = data;
                rpt.FileName = ""; // not used
                rpt.ReportName = "Отчет по грузообороту 33В";
                rpt.IsSaveValues = IsSave; 
                
                Log.Information("Построение отчета");
                if (!rpt.CreateReport()) {
                    Log.Error($"Построение отчета:{rpt.Error}");
                    return false;
                }

                excelbin.AddRange(rpt.excelbin);
                //Array.Copy(rpt.excelbin, this.excelbin, rpt.excelbin.Length);

                if (IsSave && !UseSavedData) data.Save(rpt.DataToSave, rd);
            }
            finally
            {
                data.Dispose();
            }
            return true;
        }

        public void SaveFile()
        {
            if (excelbin.Count == 0) return;

            string FileName = "Отчет по грузообороту 33В.xlsx";
            if (File.Exists(FileName))
            {
                try
                {
                    File.Delete(FileName);
                }
                catch
                {
                    FileName = Common.GetNextAvailableFilename(FileName);
                }
            }
            try
            {
                File.WriteAllBytes(FileName, excelbin.ToArray());
            }
            catch (Exception ex)
            {
                Log.Error($"Ошибка excel.SaveAs:{ex}");
            }
        }
    }
}
