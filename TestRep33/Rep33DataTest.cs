using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using Rep33.Data;
using System.Data;
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
            //var dt = System.DateTime.Today.AddDays(-1);
            var dt = new System.DateTime(2021, 11, 15);
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
            var shfn = @"d:\Work\temp\repshablon.xlsx";
            FileInfo shablon = new FileInfo(@"d:\Work\temp\repshablon.xlsx");
            FileInfo newFile = new FileInfo(@"d:\Work\temp\repnew.xlsx");
            //using (Stream inpStream = File.Open(shfn, FileMode.Open, FileAccess.ReadWrite))

            using var inpStream = new FileStream(shfn, FileMode.Open);
            using (var outStream = new MemoryStream())
            using (var excel = new ExcelPackage(outStream, inpStream))
            //using (var excel = new ExcelPackage(inpStream))
            //using (var excel = new ExcelPackage(shablon, true)) 
            {
                var wsSrc = excel.Workbook.Worksheets[1];
                var prevFormat = wsSrc.Cells[18, 2].StyleID;
                ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Inventory");
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 1].StyleID = prevFormat;
                excel.SaveAs(newFile);
            }
        }

        [TestMethod]
        public void Row_Multiple_Grouping_Test()
        {
            //https://stackoverflow.com/questions/57925761/how-to-add-multiple-collapse-in-same-outline-using-epplus-c-sharp

            //Throw in some data
            var dataTable = new DataTable("tblData");
            dataTable.Columns.AddRange(new[]
            {
        new DataColumn("Header", typeof (string)),
        new DataColumn("Col1", typeof (int)),
        new DataColumn("Col2", typeof (int)),
        new DataColumn("Col3", typeof (object))
    });

            for (var i = 0; i < 10; i++)
            {
                var row = dataTable.NewRow();
                row[0] = $"Header {i}";
                row[1] = i; row[2] = i * 10;
                row[3] = Path.GetRandomFileName();
                dataTable.Rows.Add(row);
            }

            //Create a test file
            var fi = new FileInfo(@"d:\Work\temp\Row_Multiple_Grouping_Test.xlsx");
            if (fi.Exists)
                fi.Delete();

            using (var pck = new ExcelPackage(fi))
            {
                var worksheet = pck.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells.LoadFromDataTable(dataTable, true);

                //Create the grouping
                for (var i = 2; i <= 11; i++)
                {
                    worksheet.Row(i).OutlineLevel = 1;
                    worksheet.Row(i).Collapsed = true;
                }

                //Create a gap - cant shrink or hide it because it hides the collapse button in GUI
                worksheet.InsertRow(7, 1);

                pck.Save();
            }
        }
    }
}
