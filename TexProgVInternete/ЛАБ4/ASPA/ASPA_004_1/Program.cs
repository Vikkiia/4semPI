
using CustomExceptions;
using DAL004;
using Microsoft.AspNetCore.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");


        Repository.JSONFileName = "Celebrities.json";
        using (IRepository repository = Repository.Create("Celebrities")) 
        {
            app.UseExceptionHandler("/Celebrities/Error");

            app.MapGet("/Celebrities", () => repository.GetAllCelebrities());


            app.MapGet("/Celebrities/{id:int}", (int id) =>
            {
                Celebrity? celebrity = repository.GetCelebrityById(id);
                if (celebrity == null)
                {
                    throw new FoundByIdException($"Celebrity id = {id}");
                }
                return celebrity;
            });
            app.MapPost("/Celebrities", async (HttpContext context) =>
            {
                var celebrity = await context.Request.ReadFromJsonAsync<Celebrity>();
                if (celebrity == null)
                    return Results.BadRequest("������������ ������ ������.");

                string fileName = Path.GetFileName(celebrity.PhotoPath);
                string celebritiesFolder = Path.Combine(
                    context.RequestServices.GetRequiredService<IWebHostEnvironment>().ContentRootPath,
                    "Celebrities");

                string[] filesInFolder = Directory.GetFiles(celebritiesFolder);
                bool fileFound = filesInFolder.Any(file =>
                    Path.GetFileName(file).Equals(fileName, StringComparison.OrdinalIgnoreCase));

                if (!fileFound)
                    throw new SaveException($"Could not find file '{fileName}' in folder '{celebritiesFolder}'");

                int? id = repository.addCelebrity(celebrity);
                int result = repository.saveChanges();

               

                return Results.Ok(new Celebrity((int)(id ?? -1), celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath));
            });


            app.MapFallback((HttpContext ctx) => Results.NotFound(new { error = $"Path {ctx.Request.Path} not supported" }));
            app.Map("/Celebrities/Error", (HttpContext ctx) =>
            {
                Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
                IResult rc = Results.Problem(detail: "Panic", instance: app.Environment.EnvironmentName, title: "ASPA004", statusCode: 500);
                if (ex != null)
                {
                    if(ex is FoundByIdException)
                    {
                        rc = Results.NotFound(ex.Message);
                    }
                    if(ex is BadHttpRequestException)
                    {
                        rc = Results.BadRequest(ex.Message);
                    }
                    if(ex is SaveException)
                    {
                        rc = Results.Problem(title: "ASP004/SaveChanges", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
                    }
                    if(ex is AddCelebrityException)
                    {
                        rc = Results.Problem(title: "ASP004/AddCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
                    }
                }
                return rc;
            });
        }

        app.Run();
    }
}
