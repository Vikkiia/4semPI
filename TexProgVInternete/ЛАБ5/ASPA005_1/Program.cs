﻿using CustomExceptions;
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
        {/////////////////////
            app.UseExceptionHandler("/Celebrities/Error");

            app.MapGet("/Celebrities", () => repository.GetAllCelebrities());

            app.MapGet("/Celebrities/{id:int}", (int id) =>
            {
                Celebrity? celebrity = repository.GetCelebrityById(id);
                return celebrity is null
                    ? Results.NotFound($"Celebrity with Id = {id} not found.")
                    : Results.Ok(celebrity);
            });

            app.MapPut("/Celebrities/{id:int}", (int id, Celebrity celebrity) =>
            {
                var result = repository.updCelebrityById(id, celebrity);

                if (result == -1)
                {
                    throw new PutCelebrityById($"Celebrity id {id} not found for put.");
                }

                var updatedCelebrity = repository.GetCelebrityById(id);

                return Results.Ok(updatedCelebrity);
            });

            app.MapDelete("/Celebrities/{id:int}", (int id) =>
            {
                bool deleted = repository.delCelebrity(id);
                if (!deleted)
                {
                    return Results.NotFound($"Celebrity with id {id} not found for deletion.");
                }

                if (repository.saveChanges() <= 0)
                {
                    return Results.Ok($"Celebrity with id = {id} deleted.");
                }
                return Results.NoContent();
            });

            app.MapPost("/Celebrities", (Celebrity celebrity, HttpContext httpContext) =>
            {
                int? id = repository.addCelebrity(celebrity);
                if (id == null)
                    throw new AddCelebrityException("POST /Celebrities error, id == null");

                //string fileName = Path.GetFileName(celebrity.PhotoPath);
                //string celebritiesFolder = Path.Combine(
                //    httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().ContentRootPath,
                //    "Celebrities");

                //string[] filesInFolder = Directory.GetFiles(celebritiesFolder);
                //bool fileFound = filesInFolder.Any(file =>
                //    Path.GetFileName(file).Equals(fileName, StringComparison.OrdinalIgnoreCase));

                return Results.Ok(new Celebrity((int)id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath));
            })
            ///////////
          
            .AddEndpointFilter(async (context, next) =>
            {
                var celebrity = context.GetArgument<Celebrity>(0);
                if (celebrity == null)
                    throw new BadHttpRequestException("Celebrity object is null");

                if (string.IsNullOrWhiteSpace(celebrity.Surname) || celebrity.Surname.Length < 2)
                    return Results.Conflict("POST /Celebrities error, Surname is wrong");

                return await next(context);
            })


            
            .AddEndpointFilter(async (context, next) =>
            {
                var celebrity = context.GetArgument<Celebrity>(0);
                
                var allCelebrities = repository.GetAllCelebrities();

                if (allCelebrities.Any(c => c.Surname == celebrity.Surname))
                    return Results.Conflict("POST /Celebrities error, Surname is doubled");

                return await next(context);
            })


            .AddEndpointFilter(async (context, next) =>
            {

                var celebrity = context.GetArgument<Celebrity>(0);
                
                var httpContext = context.GetArgument<HttpContext>(1);

                string fileName = Path.GetFileName(celebrity.PhotoPath);
                string celebritiesFolder = Path.Combine(
                    httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().ContentRootPath,
                    "Celebrities");

                string[] filesInFolder = Directory.GetFiles(celebritiesFolder);
                bool fileFound = filesInFolder.Any(file =>
                    Path.GetFileName(file).Equals(fileName, StringComparison.OrdinalIgnoreCase));
                
                
                if (!fileFound)
                {

                    httpContext.Response.Headers.Add("X-Celebrity", $"NotFound={fileName}");

                    
                    return await next(context);
                }
                
                return await next(context);
            });

            app.MapFallback((HttpContext ctx) => Results.NotFound(new { error = $"Path {ctx.Request.Path} not supported" }));
            //////////////////////
            app.Map("/Celebrities/Error", (HttpContext ctx) =>
            {
                Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
                IResult rc = Results.Problem(detail: "Panic", instance: app.Environment.EnvironmentName, title: "ASPA004", statusCode: 500);

                if (ex != null)
                {
                    if (ex is FoundByIdException)
                        rc = Results.NotFound(ex.Message);
                    if (ex is BadHttpRequestException)
                        rc = Results.BadRequest(ex.Message);
                    if (ex is SaveException)
                        rc = Results.Problem(title: "ASP004/SaveChanges", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
                    if (ex is AddCelebrityException)
                        rc = Results.Problem(title: "ASP004/AddCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
                    if (ex is DeleteCelebrityById)
                        rc = Results.Problem(title: "ASP004/delCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
                    if (ex is PutCelebrityById)
                        rc = Results.Problem(title: "ASP004/putCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
                }

                return rc;
            });
        }

        app.Run();
    }
}