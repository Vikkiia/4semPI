using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var app = builder.Build();


        var defaultFilesOptions = new DefaultFilesOptions();
        defaultFilesOptions.DefaultFileNames.Clear();
        defaultFilesOptions.DefaultFileNames.Add("Neumann.html");


        app.MapGet("/static", async context =>
        {
            //объект запроса в который предоставлет данные о запросе 
            context.Response.ContentType = "image/jpeg";//какого тимпа будет ответ 
            app.UseStaticFiles();
            await context.Response.SendFileAsync(
                Path.Combine(app.Environment.ContentRootPath, "Picture", "Neumann.jpg")
            );
        });


        app.Run(); 
    }
}