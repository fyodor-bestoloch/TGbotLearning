using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using TGbot.Model;

namespace TGbot;

public class GptApiContoller
{
    public GptApiContoller()
    {
        _requestProducer = new();
    }
    
    private readonly GptApiRequestProducer _requestProducer;

    public async Task<GtpApiResponseModel> AskChatGpt(string userMessage)
    {
        using var httpClient = new HttpClient();

        var request = await _requestProducer.CreateRequestAskChatGpt(userMessage);
        
        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Couldn't get valid response from Gpt Api.");
        }
        
        var deserializedResponse = await response.Content.ReadFromJsonAsync<GtpApiResponseModel>();
        if (deserializedResponse is null)
        {
            throw new Exception("Couldn't get valid response from Gpt Api.");
        }

        return deserializedResponse;
    }
}