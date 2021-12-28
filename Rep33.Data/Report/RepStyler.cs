using OfficeOpenXml;

namespace Rep33.Data.Report
{
    public class RepStyler
    {
        public static int SetStyle(ExcelRange er, string StyleName)
        {
            int level = 0;
            switch (StyleName)
            {
                case "Header1":
                    SetHeader1Style(er);
                    break;
                case "Header2":
                    SetHeader2Style(er);
                    break;
                case "Header3":
                    SetHeader3Style(er);
                    level = 1;
                    break;
                case "Header4":
                    SetHeader4Style(er);
                    level = 2;
                    break;
                case "Header5":
                    SetHeader5Style(er);
                    level = 3;
                    break;
                case "Header6":
                    SetHeader6Style(er);
                    level = 4;
                    break;
            }
            return level;
        }

        /// <summary>
        /// Стили для строк
        /// </summary>
        /// <param name="er">Диапозон ячеек</param>
        private static void SetHeader1Style(ExcelRange er)
        {
            er.Style.Font.Size = 12;
            er.Style.Font.Bold = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }

        private static void SetHeader2Style(ExcelRange er)
        {
            er.Style.Font.Size = 12;
            er.Style.Font.Bold = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        }

        private static void SetHeader3Style(ExcelRange er)
        {
            er.Style.Font.Size = 11;
            er.Style.Font.Bold = true;
            er.Style.Font.Italic = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            er[er.Start.Row, er.Start.Column].Style.Indent = 2;
        }

        private static void SetHeader4Style(ExcelRange er)
        {
            er.Style.Font.Size = 11;
            er.Style.Font.Bold = true;
            er.Style.Font.Italic = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            er[er.Start.Row, er.Start.Column].Style.Indent = 3;
        }

        private static void SetHeader5Style(ExcelRange er)
        {
            er.Style.Font.Size = 10;
            er.Style.Font.Italic = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            er[er.Start.Row, er.Start.Column].Style.Indent = 5;
        }

        private static void SetHeader6Style(ExcelRange er)
        {
            er.Style.Font.Size = 10;
            er.Style.Font.Italic = true;
            er.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            er.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
            if (er.Start.Column != er.End.Column) er.Worksheet.Cells[er.Start.Row, er.Start.Column + 1, er.End.Row, er.End.Column].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            er[er.Start.Row, er.Start.Column].Style.Indent = 6;
        }
    }
}
