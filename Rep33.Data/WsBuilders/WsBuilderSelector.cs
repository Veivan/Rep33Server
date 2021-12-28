namespace Rep33.Data.WsBuilders
{
    /// <summary>
    /// Подбор строителя для WorkSheet
    /// </summary>
    class WsBuilderSelector
    {
        public static WsBuilderBase GetBuilder(string wsType, Report.ReportBuilder rb)
        {
            return wsType switch
            {
                "T2" => new WsBuilderT2(rb.ReportData),
                "T3" => new WsBuilderT3(rb.ReportData),
                _ => new WsBuilderT1(rb.ReportData, rb.DataToSave),
            };
        }
    }
}
