using Microsoft.Extensions.Hosting;
using SMbot;

public static class Program
{
    public static void Main()
    {
        var app = AppConfigurator.Configure();
        app.Run();
    }
}