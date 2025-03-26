using SMbot.Model;

namespace SMbot.Interfaces;

public interface ISalesmateApiContoller
{
    public Task<SalesmateApiResponseModel> SearchSalesmate(string userMessage);
}