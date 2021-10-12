using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rep33.Data;

namespace TestRep33
{
    [TestClass]
    public class Rep33DataTest
    {
        [TestMethod]
        public void ReportManagerTest()
        {
            var reportManager = new ReportManager(Common.RepKind.Manual);
            reportManager.CreateReport(System.DateTime.Today);
//            reportManager.CreateReport(System.DateTime.Today, "d:\\Work\\Temp\\r33.xls", false, true);
        }

        [TestMethod]
        public void ReadConfigTest()
        {
            var keyVal = AppSettings.GetAppSetting("t1");
            AppSettings.SetAppSetting("t1", "x");
            keyVal = AppSettings.GetAppSetting("t1");
        }


    }
}
