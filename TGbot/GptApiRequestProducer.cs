using System.Text;
using System.Text.Json;

namespace TGbot;

public class GptApiRequestProducer
{
    public enum GptApiEndpoint
    {
        GenerateResponse,
        Sample1,
        Sample2
    }
    
    private readonly Dictionary<GptApiEndpoint, string> GptApiEndpointUrls = new()
    {
        { GptApiEndpoint.GenerateResponse, "https://api.openai.com/v1/chat/completions" },
        { GptApiEndpoint.Sample1, "123" },
        { GptApiEndpoint.Sample2, "321" },
    };
    
    private readonly Dictionary<string, string> GptHeaders = new()
    {
        { "Authorization", "" }
    };
    
    public async Task<HttpRequestMessage> CreateRequestAskChatGpt(string userMessage)
    {
        var request = new HttpRequestMessage(HttpMethod.Post,
            requestUri: GptApiEndpointUrls[GptApiEndpoint.GenerateResponse]);

        await AddHeaders(request);
        
        var jsonRequestContent = JsonSerializer.Serialize(new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = "Act like a talking dog" },
                new { role = "user", content = userMessage }
            }
        });
        
        request.Content = new StringContent(
            content: jsonRequestContent,
            encoding: Encoding.UTF8,
            mediaType: "application/json");
        
        return request;
    }
    
    private async Task AddHeaders(HttpRequestMessage request)
    {
        foreach (var header in GptHeaders)
        {
            request.Headers.Add(header.Key, header.Value);
        }
    }
    
}