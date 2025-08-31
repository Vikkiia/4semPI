using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddFilter("Microsoft.AspNetCore.Diagnostics", LogLevel.None);
        // Фильтр сообщений 

        var app = builder.Build();

        app.UseExceptionHandler("/error");  // Путь к обработчику исключений

        app.MapGet("/", () => "Start");

        app.MapGet("/test1", () =>
        {
            throw new Exception("-- Exception Test --"); // Пользовательское исключение
        });

        app.MapGet("/test2", () =>
        {
            int x = 0, y = 5, z = 0;
            z = y / x;  // Деление на 0
            return "test2";
        });

        app.MapGet("/test3", () =>
        {
            int[] x = new int[3] { 1, 2, 3 };
            int y = x[3]; // Выход за границы массива
            return "test3";
        });

        app.Map("/error", async (ILogger<Program> logger, HttpContext context) =>
        {
            IExceptionHandlerFeature? exobj = context.Features.Get<IExceptionHandlerFeature>();
            await context.Response.WriteAsync("<h1>Oops!</h1>");
            logger.LogError(exobj?.Error, "ExceptionHandler");//логирует в консоль
        });

        app.Run();
    }
}
