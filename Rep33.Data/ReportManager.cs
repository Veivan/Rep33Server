using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace Rep33.Data
{
    public class ReportManager
    {
        private Common.RepKind state;
        private bool IsSave;
        private bool UseSavedData;
        private List<byte> excelbin = new List<byte>();
        private DateTime dateReport = DateTime.Today;

        /// <summary>
        /// Constructor is used with Common.RepKind.Admin
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
        /// Constructor is used with Common.RepKind.Manual & Auto
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
            dateReport = rd;

            var data = new ReportData("reporter", "RepTi87BnVuy21", "(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=orabase.mcargo)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=CHAOS)))", "MASTER", "all%work", "partner", "172.30.80.49");

            //var data = new ReportData("reporter", "RepTi87BnVuy21", "(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=10.80.15.3)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=CHAOS)))", "MASTER", "all%work", "partner", "10.80.0.48");
            //var data = new ReportData("reporter", "RepTi87BnVuy21", "(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=10.80.15.3)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=CHAOS)))", "MASTER", "all%work", "partner", "172.30.80.49");
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

            string FileName = string.Format("{3}\\Отчет по грузообороту {0}{1:00}{2:00}.xlsx", dateReport.Year, dateReport.Month, dateReport.Day, 
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
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

        public byte[] GetExcelData()
        {
            if (excelbin.Count == 0) return null;
            return excelbin.ToArray();
        }

        public void SendMail() 
        {
            if (excelbin.Count == 0) return;

            Log.Information("Отправка по электронной почте");
            string[] emails = AppSettings.GetAppSetting("emails").Split(',');
            if (emails.Length <= 0) return;
            bool bFirst = true;
            MailMessage m = null;
            MailAddress from = new MailAddress("Report@moscow-cargo.com", "Сервис отчетов");
            foreach (var email in emails)
            {
                MailAddress to;
                if (bFirst)
                {
                    to = new MailAddress(email);
                    m = new MailMessage(from, to);
                    bFirst = false;
                }
                else
                {
                    to = new MailAddress(email);
                    m.CC.Add(to);
                }
            }
            m.Subject = string.Format("Отчет по грузообороту за {0:00}.{1:00}.{2}", dateReport.Day, dateReport.Month, dateReport.Year);
            m.Body = string.Format("<p>Письмо сформировано автоматически. Убедительная просьба не отвечать на него.</p><p>По возникающим вопросам обращаться по адресу электронной почты <a href='mailto:it@moscow-cargo.com'>it@moscow-cargo.com</a> либо по контактному телефону <a href='tel:8 (495) 737 60 60'>8 (495) 737 60 60</a> доб. 3334</p>", dateReport.Day, dateReport.Month, dateReport.Year);
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("172.30.80.42", 25);

            string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var contentType = new System.Net.Mime.ContentType(mimeType);
            contentType.Name = string.Format("Отчет по грузообороту {0}{1:00}{2:00}.xlsx", dateReport.Year, dateReport.Month, dateReport.Day);

            try
            {
                using (MemoryStream stream = new MemoryStream(excelbin.ToArray()))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    Attachment attachment = new Attachment(stream, contentType);
                    m.Attachments.Add(attachment);
                    smtp.Send(m);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Ошибка SendMail:{0}", ex));
            }
        }

    }
}
