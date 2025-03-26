
using System.Text;
using Microsoft.Extensions.Hosting;
using SMbot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SMbot;

public class Bot : BackgroundService
{
    public Bot(IBotLogicWorker worker,
        TelegramBotClient botClient,
        ReceiverOptions receiverOptions)
    {
        _botClient = botClient;
        _receiverOptions = receiverOptions;
        _worker = worker;
    }
    private readonly ITelegramBotClient _botClient;
    private readonly ReceiverOptions _receiverOptions;
    private readonly IBotLogicWorker _worker;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, stoppingToken);

        var me = await _botClient.GetMe();
        Console.WriteLine($"{me.FirstName} запущен!");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    private async Task UpdateHandler(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        Console.WriteLine("Пришло сообщение!");

        var message = update.Message;
        if (message is null || message.Text is null)
        {
            Console.WriteLine("Message was null.");
            return;
        }
        
        var chatId = message.Chat.Id;
        var botResponse = await _worker.ProcessUserSearchRequestMessage(message);
        
        await TelegramBotClientExtensions.SendTextMessageAsync(_botClient,
                chatId,
                botResponse
                ?? throw new Exception("No response from Salesmate."));

        Console.WriteLine("Отправлен ответ ${user.FirstName}");
    }

    private Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        Console.WriteLine(error.ToString());
        return Task.CompletedTask;
    }

    
}