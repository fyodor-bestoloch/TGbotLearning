
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SMbot;

public class Bot
{
    public Bot()
    {

        _botClient = new TelegramBotClient("7759148716:AAGJ4eGfEhTVudFJyFJ7gvCKL5FYqfe4ulw");
        _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[]
            {
                UpdateType.Message
            }
        };

        _apiContoller = new();
    }
    private readonly ITelegramBotClient _botClient;
    private readonly ReceiverOptions _receiverOptions;
    private readonly SalesmateApiContoller _apiContoller;
    public async Task Initialize(CancellationTokenSource cts)
    {
        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);

        var me = await _botClient.GetMe();
        Console.WriteLine($"{me.FirstName} запущен!");
    }

    private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine("Пришло сообщение!");

        var message = update.Message;
        if (message is null || message.Text is null)
        {
            Console.WriteLine("Message was null.");
            return;
        }
        
        var chatId = message.Chat.Id;

        var salesmateResponse = await _apiContoller.SearchSalesmate(message.Text);
        if (salesmateResponse.Data.Data.Count < 1)
        {
            throw new Exception("No response from Salesmate.");
        };
        StringBuilder text = new ("");
        foreach (var name in salesmateResponse.Data.Data)
        {
            text.Append($"{name.Name} {name.Id}\n");
        }
        await TelegramBotClientExtensions.SendTextMessageAsync(_botClient,
                chatId,
                text.ToString()
                ?? throw new Exception("No response from Salesmate."));

        Console.WriteLine("Отправлен ответ ${user.FirstName}");
    }

    private Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        Console.WriteLine(error.ToString());
        return Task.CompletedTask;
    }
}