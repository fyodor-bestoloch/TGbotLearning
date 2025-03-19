using System.Text;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TGbot;

public class Bot
{
    private readonly ITelegramBotClient _botClient;
    private readonly ReceiverOptions _receiverOptions;
    private readonly GptApiContoller _apiContoller;

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

        var gptResponse = await _apiContoller.AskChatGpt(message.Text);
        if (gptResponse.Choices.Count < 1)
        {
            throw new Exception("No response from gpt.");
        }

        await TelegramBotClientExtensions.SendTextMessageAsync(_botClient,
                chatId,
                gptResponse.Choices[0].Message.Content
                ?? throw new Exception("No response from gpt."));

        Console.WriteLine("Отправлен ответ ${user.FirstName}");
    }

    private Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}