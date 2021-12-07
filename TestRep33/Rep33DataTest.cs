using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using Rep33.Data;
using System.IO;

namespace TestRep33
{
    [TestClass]
    public class Rep33DataTest
    {
        [TestMethod]
        public void ReportManagerTest()
        {
            var reportManager = new ReportManager(Common.RepKind.Manual);
            var dt = System.DateTime.Today.AddDays(-1);
            var ret = reportManager.CreateReport(dt);
            Assert.IsTrue(ret);
            reportManager.SaveFile();
            //reportManager.SendMail();            
        }

        [TestMethod]
        public void ReadConfigTest()
        {
            var keyVal = AppSettings.GetAppSetting("t1");
            AppSettings.SetAppSetting("t1", "x");
            keyVal = AppSettings.GetAppSetting("t1");
        }

        [TestMethod]
        public void MakeRepFromShablonTest()
        {
            var shfn = @"d:\Work\repshablon.xlsx";
            FileInfo shablon = new FileInfo(@"d:\Work\repshablon.xlsx");
            FileInfo newFile = new FileInfo(@"d:\Work\repnew.xlsx");

            //var fs = new FileStream(Server.MapPath(@"\Content\NPOITemplate.xls"), FileMode.Open, FileAccess.Read);

//            using (Stream inpStream = File.Open(shfn, FileMode.Open, FileAccess.ReadWrite))

                using var inpStream = new FileStream(shfn, FileMode.Open);

            using (var outStream = new MemoryStream())
            using (var excel = new ExcelPackage(outStream, inpStream))
//            using (var excel = new ExcelPackage(inpStream))
            //using (var excel = new ExcelPackage(shablon, true)) 
            {
                ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Inventory");
                worksheet.Cells[1, 1].Value = "ID";
                excel.SaveAs(newFile);
            }
        }

    }
}
