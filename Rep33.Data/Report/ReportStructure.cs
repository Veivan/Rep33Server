using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rep33.Data.Report
{
    [XmlRoot(ElementName = "Row")]
    public class Row
    {
        [XmlAttribute(AttributeName = "Cell")]
        public string Cell { get; set; }
        [XmlAttribute(AttributeName = "Style")]
        public string Style { get; set; }
        [XmlAttribute(AttributeName = "Collapsed")]
        public bool Collapsed { get; set; }
        [XmlText]
        public string Text { get; set; }
        [XmlElement(ElementName = "Values")]
        public Values Values { get; set; }
        [XmlElement(ElementName = "Caption")]
        public Caption Caption { get; set; }
    }

    [XmlRoot(ElementName = "Header")]
    public class Header
    {
        [XmlElement(ElementName = "Row")]
        public List<Row> Rows { get; set; }
    }

    [XmlRoot(ElementName = "Value")]
    public class Value
    {
        public Value()
        {
            AddYear = 0;
        }
        [XmlAttribute(AttributeName = "Cell")]
        public string Cell { get; set; }
        [XmlAttribute(AttributeName = "IsFormula")]
        public bool IsFormula { get; set; }
        [XmlAttribute(AttributeName = "IsPrevDays")]
        public bool IsPrevDays { get; set; }
        [XmlText]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "DateFormat")]
        public string DateFormat { get; set; }
        [XmlAttribute(AttributeName = "AddYear")]
        public int AddYear { get; set; }
        [XmlAttribute(AttributeName = "IsNotConvert")]
        public bool IsNotConvert { get; set; }
        [XmlAttribute(AttributeName = "Data")]
        public string Data { get; set; }
        [XmlAttribute(AttributeName = "QueryName")]
        public string QueryName { get; set; }
        [XmlAttribute(AttributeName = "Filter")]
        public string Filter { get; set; }
        [XmlAttribute(AttributeName = "DataValue")]
        public string DataValue { get; set; }
    }

    [XmlRoot(ElementName = "Values")]
    public class Values
    {
        [XmlElement(ElementName = "Value")]
        public List<Value> Items { get; set; }
    }

    [XmlRoot(ElementName = "Freeze")]
    public class Freeze
    {
        [XmlAttribute(AttributeName = "Col")]
        public int Col { get; set; }
        [XmlAttribute(AttributeName = "Row")]
        public int Row { get; set; }
    }

    [XmlRoot(ElementName = "Caption")]
    public class Caption
    {
        [XmlAttribute(AttributeName = "Cell")]
        public string Cell { get; set; }
        [XmlAttribute(AttributeName = "Data")]
        public string Data { get; set; }
        [XmlAttribute(AttributeName = "DateFormat")]
        public string DateFormat { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Table")]
    public class Table
    {
        [XmlElement(ElementName = "Row")]
        public List<Row> Rows { get; set; }
        [XmlAttribute(AttributeName = "Cell")]
        public string Cell { get; set; }
    }

    [XmlRoot(ElementName = "NumberFormat")]
    public class NumberFormat
    {
        [XmlText]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "Cell")]
        public string Cell { get; set; }
    }

    [XmlRoot(ElementName = "Column")]
    public class Column
    {
        [XmlText]
        public int Number { get; set; }
        [XmlAttribute(AttributeName = "Width")]
        public string Width { get; set; }
        [XmlAttribute(AttributeName = "Hide")]
        public bool Hide { get; set; }
    }

    [XmlRoot(ElementName = "PValue")]
    public class PValue
    {
        [XmlAttribute(AttributeName = "Caption")]
        public string Caption { get; set; }
        [XmlAttribute(AttributeName = "Data")]
        public string Data { get; set; }
        [XmlAttribute(AttributeName = "QueryName")]
        public string QueryName { get; set; }
        [XmlAttribute(AttributeName = "Filter")]
        public string Filter { get; set; }
        [XmlAttribute(AttributeName = "DataValue")]
        public string DataValue { get; set; }
        [XmlAttribute(AttributeName = "IsFormula")]
        public bool IsFormula { get; set; }
    }

    [XmlRoot(ElementName = "Placeholders")]
    public class Placeholders
    {
        [XmlElement(ElementName = "PValue")]
        public List<PValue> Items { get; set; }
    }

    [XmlRoot(ElementName = "ReportStructure")]
    public class ReportStructure
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "wsType")]
        public string wsType { get; set; }


        [XmlElement(ElementName = "Header")]
        public Header Header { get; set; }
        [XmlElement(ElementName = "Table")]
        public Table Table { get; set; }
        [XmlElement(ElementName = "Column")]
        public List<Column> Columns { get; set; }
        [XmlElement(ElementName = "CoditionalFormat")]
        public List<string> CoditionalFormats { get; set; }
        [XmlElement(ElementName = "WrapText")]
        public List<string> WrapTexts { get; set; }
        [XmlElement(ElementName = "Merge")]
        public List<string> Merges { get; set; }
        [XmlElement(ElementName = "NumberFormat")]
        public List<NumberFormat> NumberFormats { get; set; }
        [XmlElement(ElementName = "Freeze")]
        public Freeze Freeze { get; set; }

        [XmlElement(ElementName = "Placeholders")]
        public Placeholders Placeholders { get; set; }

    }
}
