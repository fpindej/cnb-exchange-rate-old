using System.Xml.Serialization;

namespace ExchangeRate.Infrastructure.CNB.Core.Models;

[XmlRoot(ElementName = "kurzy")]
public class ExchangeRate
{
    [XmlElement(ElementName = "tabulka")]
    public Table Table { get; set; }

    [XmlAttribute(AttributeName = "banka")]
    public string Bank { get; set; }

    [XmlAttribute(AttributeName = "datum")]
    public string Date { get; set; }

    [XmlAttribute(AttributeName = "poradi")]
    public int OrderNo { get; set; }
}
