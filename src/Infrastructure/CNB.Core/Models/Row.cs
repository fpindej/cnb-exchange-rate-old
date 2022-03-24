using System.Xml.Serialization;

namespace ExchangeRate.Infrastructure.CNB.Core.Models;

[XmlRoot(ElementName = "radek")]
public class Row
{
    [XmlAttribute(AttributeName = "kod")]
    public string Code { get; set; }

    [XmlAttribute(AttributeName = "mena")]
    public string Currency { get; set; }

    [XmlAttribute(AttributeName = "mnozstvi")]
    public int Amount { get; set; }

    [XmlAttribute(AttributeName = "kurz")]
    public double Rate { get; set; }

    [XmlAttribute(AttributeName = "zeme")]
    public string Country { get; set; }
}
