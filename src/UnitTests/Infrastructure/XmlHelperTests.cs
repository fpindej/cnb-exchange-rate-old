using System;
using ExchangeRate.Infrastructure.Common.Helper;
using Xunit;

namespace ExchangeRate.UnitTests.Infrastructure;

public class XmlHelperTests
{
    [Fact]
    public void ShouldReturnExchangeRateHasRows()
    {
        var xmlStr =
            @"<?xml version=""1.0"" encoding=""UTF-8""?><kurzy banka=""CNB"" datum=""30.03.2022"" poradi=""63""><tabulka typ=""XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU""><radek kod=""AUD"" mena=""dolar"" mnozstvi=""1"" kurz=""16,518"" zeme=""Austrálie""/><radek kod=""BRL"" mena=""real"" mnozstvi=""1"" kurz=""4,635"" zeme=""Brazílie""/></tabulka></kurzy>";

        var xmlObj = xmlStr.FromXml<ExchangeRate.Infrastructure.CNB.Core.Models.ExchangeRate>();

        Assert.NotNull(xmlObj);
        Assert.NotNull(xmlObj.Table);
        Assert.NotNull(xmlObj.Table.Rows);
        var firstRow = xmlObj.Table.Rows[0];

        Assert.Equal(2, xmlObj.Table.Rows.Count);
        Assert.Equal("AUD", firstRow.Code);
        Assert.Equal("dolar", firstRow.Currency.ToString());
        Assert.Equal(1, firstRow.Amount);
        Assert.Equal((decimal)16.518, firstRow.Rate, 4);
        Assert.Equal("Austrálie", firstRow.Country);
    }

    [Fact]
    public void ShouldReturnExchangeRateHasNoTables()
    {
        var xmlStr =
            @"<?xml version=""1.0"" encoding=""UTF-8""?><kurzy banka=""CNB"" datum=""30.03.2022"" poradi=""63""><nonExistingTable typ=""XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU""><radek kod=""AUD"" mena=""dolar"" mnozstvi=""1"" kurz=""16,518"" zeme=""Austrálie""/><radek kod=""BRL"" mena=""real"" mnozstvi=""1"" kurz=""4,635"" zeme=""Brazílie""/></nonExistingTable></kurzy>";

        var xmlObj = xmlStr.FromXml<ExchangeRate.Infrastructure.CNB.Core.Models.ExchangeRate>();

        Assert.NotNull(xmlObj);
        Assert.Null(xmlObj.Table);
    }

    [Fact]
    public void ShouldThrowInvalidOperationException()
    {
        var xmlStr = "abcd";
        var expectedMessagePrefix = "There is an error in XML document";

        var ex = Record.Exception(() => xmlStr.FromXml<ExchangeRate.Infrastructure.CNB.Core.Models.ExchangeRate>());

        Assert.NotNull(ex);
        Assert.True(ex is InvalidOperationException);
        Assert.StartsWith(expectedMessagePrefix, ex.Message);
    }
}
