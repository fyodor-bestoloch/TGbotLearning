using System.Text;
using System.Text.Json;

namespace SMbot;

public class SalesmateApiRequestProducer
{
    public enum SalesmateApiEndpoint
    {
        GenerateResponse, //488de9d1-7996-11ef-bf9f-9b6c53603f75
        Sample1,
        Sample2
    }
    
    private readonly Dictionary<SalesmateApiEndpoint, string> SalesmateApiEndpointUrls = new()
    {
        { SalesmateApiEndpoint.GenerateResponse, "https://adsterrapublishers.salesmate.io/apis/company/v4/search?rows=250&from=0" },
        { SalesmateApiEndpoint.Sample1, "123" },
        { SalesmateApiEndpoint.Sample2, "321" },
    };
    
    private readonly Dictionary<string, string> SalesmateHeaders = new()
    {
        { "accessToken", "0de94d51-e2a8-11ed-9e4e-1fbf32879444" },
        { "x-linkname", "adsterrapublishers.salesmate.io" }
    };
    
    public async Task<HttpRequestMessage> CreateRequestAskSalesmate(string userMessage)
    {
        var request = new HttpRequestMessage(HttpMethod.Post,
            requestUri: SalesmateApiEndpointUrls[SalesmateApiEndpoint.GenerateResponse]);

        await AddHeaders(request);
        
        var jsonRequestContent = new StringContent($@"
        {{
            ""displayingFields"": [
                ""company.type"",
                ""company.name"",
                ""company.id""
            ],
            ""filterQuery"": {{
                ""group"": {{
                    ""operator"": ""AND"",
                    ""rules"": [
                        {{
                            ""condition"": ""CONTAINS"",
                            ""moduleName"": ""Company"",
                            ""field"": {{
                                ""fieldName"": ""company.name"",
                                ""displayName"": ""Name"",
                                ""type"": ""String""
                            }},
                            ""data"": ""{userMessage}"",  
                            ""eventType"": ""String""
                        }}
                    ]
                }}
            }},
            ""sort"": {{
                ""fieldName"": ""company.annualRevenue"",
                ""order"": ""desc""
            }},
            ""moduleId"": 5,
            ""reportType"": ""get_data"",
            ""getRecordsCount"": true
        }}", Encoding.UTF8, "application/json");
        
        request.Content = jsonRequestContent;
        
        return request;
        
    }
    
    private async Task AddHeaders(HttpRequestMessage request)
    {
        foreach (var header in SalesmateHeaders)
        {
            request.Headers.Add(header.Key, header.Value);
        }
    }
    
}