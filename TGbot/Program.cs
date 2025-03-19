
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

class TGbot
{
    private static readonly string apiKey = "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"; 
    private static readonly string endpoint = "https://api.openai.com/v1/chat/completions";
    
    private static ITelegramBotClient _botClient;
    
    private static ReceiverOptions _receiverOptions;
    
    static async Task Main()
    {
        
        _botClient = new TelegramBotClient("7759148716:AAGJ4eGfEhTVudFJyFJ7gvCKL5FYqfe4ulw"); 
        _receiverOptions = new ReceiverOptions 
        {
            AllowedUpdates = new[] 
            {
                UpdateType.Message
            }
            
        };
        
        using var cts = new CancellationTokenSource();
        
        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token); 
        
        var me = await _botClient.GetMe(); 
        Console.WriteLine($"{me.FirstName} запущен!");
        
        await Task.Delay(-1); 
    }
    
   
    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                {
                    Console.WriteLine("Пришло сообщение!");
                    var message = update.Message;
                    var chat = message.Chat;
                    var chatId = chat.Id;
                    
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                        var requestBody = new
                        {
                            model = "gpt-3.5-turbo", 
                            messages = new[]
                            {
                                new { role = "system", content = "Act like a talking dog" },
                                new { role = "user", content = message.Text }
                            }
                        };

                        string jsonBody = JsonConvert.SerializeObject(requestBody);
                        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync(endpoint, content);
                        
                        string responseString = await response.Content.ReadAsStringAsync();

                        Console.WriteLine(responseString);

                        dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                        
                        await TelegramBotClientExtensions.SendTextMessageAsync(_botClient, chatId, jsonResponse.choices[0].message.content.ToString());
                        Console.WriteLine("Отправлен ответ ${user.FirstName}");
                        return;
                       
                    }
                    
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    
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