using System.Text;
using SMbot.Interfaces;
using Telegram.Bot.Types;

namespace SMbot;

public class BotLogicWorker : IBotLogicWorker
{
    public BotLogicWorker(ISalesmateApiContoller apiContoller)
    {
        _apiContoller = apiContoller;
    }
    
    private readonly ISalesmateApiContoller _apiContoller;
    public async Task<string> ProcessUserSearchRequestMessage(Message userMessage)
    {
        var salesmateResponse = await _apiContoller.SearchSalesmate(userMessage.Text!);
        if (salesmateResponse.Data.Data.Count < 1)
        {
            throw new Exception("No response from Salesmate.");
        };
        
        var sb = new StringBuilder();
        foreach (var name in salesmateResponse.Data.Data)
        {
            sb.AppendLine($"{name.Name} {name.Id}");
        }

        return sb.ToString();
    }
}