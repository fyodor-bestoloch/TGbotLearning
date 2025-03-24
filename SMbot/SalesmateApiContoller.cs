using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using SMbot.Model;

namespace SMbot;

public class SalesmateApiContoller
{
    public SalesmateApiContoller()
    {
        _requestProducer = new();
    }
    
    private readonly SalesmateApiRequestProducer _requestProducer;

    public async Task<SalesmateApiResponseModel> AskSalesmate(string userMessage)
    {
        using var httpClient = new HttpClient();

        var request = await _requestProducer.CreateRequestAskSalesmate(userMessage);
        
        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(response.ToString());
            throw new Exception("Couldn't get valid response from Salesmate Api.");
        }
        //string responseString = await response.Content.ReadAsStringAsync();
        //Console.WriteLine("response");
        //Console.WriteLine(responseString);
        var deserializedResponse = await response.Content.ReadFromJsonAsync<SalesmateApiResponseModel>();
        if (deserializedResponse is null)
        {
            Console.WriteLine(response.ToString());
            throw new Exception("Couldn't get valid response from Salesmate Api.");
        }

        return deserializedResponse;
    }
}