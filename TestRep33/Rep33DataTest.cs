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
            var ret = reportManager.CreateReport(System.DateTime.Today);
            Assert.IsTrue(ret);
            reportManager.SaveFile();
            reportManager.SendMail();            
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
