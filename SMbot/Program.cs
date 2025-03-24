using SMbot;

public static class Program
{
    static async Task Main()
    {
        var bot = new Bot();
        
        using var cts = new CancellationTokenSource();
        await bot.Initialize(cts);

        while (!cts.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}