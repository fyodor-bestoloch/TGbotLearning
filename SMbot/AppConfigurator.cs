using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using SMbot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace SMbot;

public static class AppConfigurator
{
    public static IHost Configure()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddHttpClient(Options.DefaultName)
            .AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError().RetryAsync(0))
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                return handler;
            });

        builder.Services.AddHostedService<Bot>();

        builder.Services.AddKeyedSingleton<TelegramBotClient>(
            new TelegramBotClient("7759148716:AAGJ4eGfEhTVudFJyFJ7gvCKL5FYqfe4ulw"));
        builder.Services.AddSingleton<ReceiverOptions>(new ReceiverOptions
        {
            AllowedUpdates = new[]
            {
                UpdateType.Message
            }
        });

        builder.Services.AddScoped<ISalesmateApiContoller, SalesmateApiContoller>();
        builder.Services.AddScoped<ISalesmateApiRequestProducer, SalesmateApiRequestProducer>();
        builder.Services.AddScoped<IBotLogicWorker, BotLogicWorker>();
        
        return builder.Build();
    }
}