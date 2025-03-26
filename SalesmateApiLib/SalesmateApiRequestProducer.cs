using System.Text;
using System.Text.Json;
using SMbot.Interfaces;

namespace SMbot;

public class SalesmateApiRequestProducer : ISalesmateApiRequestProducer
{
    public enum SalesmateApiEndpoint
    {
        CompanySearch 
    }
    
    private readonly Dictionary<SalesmateApiEndpoint, string> SalesmateApiEndpointUrls = new()
    {
        { SalesmateApiEndpoint.CompanySearch, "https://adsterrapublishers.salesmate.io/apis/company/v4/search?rows=250&from=0" }
    };
    
    private readonly Dictionary<string, string> SalesmateHeaders = new()
    {
        { "accessToken", "0de94d51-e2a8-11ed-9e4e-1fbf32879444" },
        { "x-linkname", "adsterrapublishers.salesmate.io" }
    };
    
    public async Task<HttpRequestMessage> CreateRequestSearchSalesmate(string userMessage)
    {
        var request = new HttpRequestMessage(HttpMethod.Post,
            requestUri: SalesmateApiEndpointUrls[SalesmateApiEndpoint.CompanySearch]);

        await AddHeaders(request);

        var jsonRequestContent = JsonSerializer.Serialize(new
        {

            displayingFields = new[]
            {
                "company.type",
                "company.name",
                "company.id"
            },
            filterQuery = new
            {
                group = new
                {
                    @operator = "AND",
                    rules = new[]
                    {
                        new
                        {
                            condition = "CONTAINS",
                            moduleName = "Company",
                            field = new
                            {
                                fieldName = "company.name",
                                displayName = "Name",
                                type = "String"
                            },
                            data = userMessage,
                            eventType = "String"
                        }
                    }
                }
            },
            sort = new
            {
                fieldName = "company.annualRevenue",
                order = "desc"
            },
            moduleId = 5,
            reportType = "get_data",
            getRecordsCount = true
        });
        
        request.Content = new StringContent(
            content: jsonRequestContent,
            encoding: Encoding.UTF8, 
            mediaType: "application/json");
        
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