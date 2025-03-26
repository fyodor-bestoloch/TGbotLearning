using Telegram.Bot.Types;

namespace SMbot.Interfaces;

public interface IBotLogicWorker
{
    public Task<string> ProcessUserSearchRequestMessage(Message userMessage);
}