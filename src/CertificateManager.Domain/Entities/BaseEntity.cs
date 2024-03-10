using Newtonsoft.Json;

namespace CertificateManager.Domain.Entities;

public class BaseEntity
{
    [JsonProperty(Order = 1)]
    public Guid Id { get; set; }
}