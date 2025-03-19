using System.Text.Json.Serialization;

namespace TGbot.Model;

public class GtpApiResponseModel
{
    [JsonPropertyName("choices")]
    public List<GtpApiResponseModelChoice> Choices { get; set; }
}

public class GtpApiResponseModelChoice
{
    [JsonPropertyName("message")]
    public GtpApiResponseModelMessage Message { get; set; }
}

public class GtpApiResponseModelMessage
{
    [JsonPropertyName("content")]
    public string Content { get; set; }
}

