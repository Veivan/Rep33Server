namespace Rep33.Data.TableBuilders
{
    /// <summary>
    /// Подбор строителя таблицы для WorkSheet
    /// </summary>
    class TBuilderSelector
    {
        public static TableBuiiderBase GetBuilder(string wsType, Report.ReportBuilder rb)
        {
            return wsType switch
            {
                "T2" => new TBuilderT2(rb.ReportData, rb.DataToSave),
                "T3" => new TBuilderT3(rb.ReportData, rb.DataToSave),
                _ => new TBuilderT1(rb.ReportData, rb.DataToSave),
            };
        }
    }
}
