using System.Text.Json.Serialization;

namespace MyInvest.REST.Accounts;

public class AddCashRequestDto
{
    public Guid TransactionId { get; set; }
    [JsonPropertyName("mac")]
    public string MAC { get; set; }
    public decimal Amount { get; set; }
}
