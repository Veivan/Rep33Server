using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rep33.Data.Report
{
    [XmlRoot(ElementName = "Freeze")]
    public class Freeze
    {
        [XmlAttribute(AttributeName = "Col")]
        public int Col { get; set; }
        [XmlAttribute(AttributeName = "Row")]
        public int Row { get; set; }
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
        [XmlAttribute(AttributeName = "Style")]
        public string Style { get; set; }
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

        [XmlElement(ElementName = "Freeze")]
        public Freeze Freeze { get; set; }

        [XmlElement(ElementName = "Placeholders")]
        public Placeholders Placeholders { get; set; }

    }
}
