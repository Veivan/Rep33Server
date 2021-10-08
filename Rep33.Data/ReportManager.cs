using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Serilog;


namespace Rep33.Data
{
    public class ReportManager
    {
        public bool CreateReport(DateTime rd, string FileName, bool IsSave, bool UseSavedData)
        {
            var data = new ReportData("reporter", "RepTi87BnVuy21", "(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=orabase.mcargo)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=CHAOS)))", "MASTER", "all%work", "partner", "172.30.80.49");
            //DB.ReportData data = new DB.ReportData("reporter", "RepTi87BnVuy21", "(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=10.80.15.3)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=CHAOS)))", "MASTER", "all%work", "partner", "10.80.0.48");
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
                rpt.FileName = FileName; 
                rpt.ReportName = "Отчет по грузообороту 33В";
                rpt.IsSaveValues = IsSave; // chkSave.Checked;
                
                Log.Information("Построение отчета");
                if (!rpt.CreateReport()) {
                    Log.Error($"Построение отчета:{rpt.Error}");
                    return false;
                }
//                if (chkSave.Checked && !UseSavedData) data.Save(rpt.DataToSave, rd);
                rpt = null;
            }
            finally
            {
                data.Dispose();
            }
            return true;
        }
    }
}
