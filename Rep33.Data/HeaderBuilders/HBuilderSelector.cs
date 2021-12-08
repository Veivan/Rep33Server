namespace Rep33.Data.HeaderBuilders
{
    /// <summary>
    /// Подбор строителя заголовка для WorkSheet
    /// </summary>
    public class HBuilderSelector
    {
        public static HeaderBuiiderBase GetBuilder(string wsType)
        {
            return wsType switch
            {
                "T2" => new HBuilderT2(),
                "T3" => new HBuilderT3(),
                _ => new HBuilderT1(),
            };
        }
    }
}
