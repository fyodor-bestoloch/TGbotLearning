namespace SMbot.Interfaces;

public interface ISalesmateApiRequestProducer
{
    public Task<HttpRequestMessage> CreateRequestSearchSalesmate(string userMessage);
}