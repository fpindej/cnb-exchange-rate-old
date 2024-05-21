using System.ComponentModel.DataAnnotations;

namespace ExchangeRate.Infrastructure.CNB.Client;

public sealed class CnbConfiguration
{
    public const string SectionName = "CNB";

    [Required]
    public required string BaseUrl { get; init; }

    [Required]
    public required string RequestUrl { get; init; }

    public string DefaultCurrency { get; init; } = "CZK";
}
