using System.Xml.Serialization;
using ExchangeRate.Domain.Entities;

namespace ExchangeRate.Infrastructure.CNB.Core.Models;

[XmlRoot(ElementName = "radek")]
public class Row
{
    [XmlAttribute(AttributeName = "kod")]
    public string Code { get; set; }

    [XmlIgnore]
    public Currency Currency { get; set; }

    [XmlAttribute(AttributeName = "mena")]
    public string CurrencyFormatted
    {
        get => Currency.Code;
        set => Currency = new Currency(value);
    }

    [XmlAttribute(AttributeName = "mnozstvi")]
    public int Amount { get; set; }

    [XmlIgnore]
    public decimal Rate { get; set; }

    [XmlAttribute(AttributeName = "kurz")]
    public string RateFormatted
    {
        get => Rate.ToString(CNBConstants.RateFormat);
        set => Rate = decimal.Parse(value, CNBConstants.RateFormat);
    }

    [XmlAttribute(AttributeName = "zeme")]
    public string Country { get; set; }
}
