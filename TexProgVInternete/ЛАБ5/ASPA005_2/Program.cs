
using CustomExceptions;
using DAL004;
using Microsoft.AspNetCore.Diagnostics;
using Validation;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        Repository.JSONFileName = "Celebrities.json";
        app.MapGet("/", () => "Hello World!");
        using (IRepository repository = Repository.Create("Celebrities"))
        {
            
            SurnameFilter.repository = repository;//похволяем фидьтру работать с репозиторием
            PhotoExistFilter.repository = repository;
            PutFilter.repository = repository;
            DeleteFilter.repository = repository;

            app.UseExceptionHandler("/Celebrities/Error");

            // 
            RouteGroupBuilder api = app.MapGroup("/Celebrities");

            api.MapGet("/", () => repository.GetAllCelebrities());

            api.MapGet("/{id:int}", (int id) =>
            {
                Celebrity? celebrity = repository.GetCelebrityById(id);
                return celebrity is null
                    ? Results.NotFound($"Celebrity with Id = {id} not found.")
                    : Results.Ok(celebrity);
            });

            api.MapPut("/{id:int}", (int id, Celebrity celebrity) =>
            {
                int? result = repository.updCelebrityById(id, celebrity);
                if (result == -1)
                    throw new PutCelebrityById($"Celebrity id {id} not found for put.");

                var updatedCelebrity = repository.GetCelebrityById(id);
                
                return Results.Ok(updatedCelebrity);
            })
            .AddEndpointFilter<PutFilter>();

            api.MapDelete("/{id:int}", (int id) =>
            {
                bool deleted = repository.delCelebrity(id);
                if (!deleted)
                    throw new DeleteCelebrityById($"Celebrity with id {id} not found for deletion.");

                //if (repository.saveChanges() <= 0)
                //    throw new SaveException("SaveChanges() <= 0 on delete");
                /*Если saveChanges() возвращает 0 или отрицательное значение, это означает, что изменения не были сохранены, и, скорее всего, операция (например, удаление записи) не была выполнена корректно.*/
                return Results.Ok($"Celebrity with id = {id} deleted");
            })
            .AddEndpointFilter<DeleteFilter>();

            app.MapPost("/Celebrities", (Celebrity celebrity, HttpContext httpContext) =>
            {
                int? id = repository.addCelebrity(celebrity);
                if (id == null)
                    throw new AddCelebrityException("POST /Celebrities error, id == null");

                return Results.Ok(new Celebrity((int)id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath));
            })
            .AddEndpointFilter<SurnameFilter>()
            .AddEndpointFilter<PhotoExistFilter>();

            
            app.Map("/Celebrities/Error", (HttpContext ctx) =>
            {
                Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
                IResult rc = Results.Problem(title: "ASPA005", detail: "Panic", instance: app.Environment.EnvironmentName, statusCode: 500);

                if (ex != null)
                {
                    if (ex is FoundByIdException)
                        rc = Results.NotFound(ex.Message);
                    if (ex is BadHttpRequestException)
                        rc = Results.BadRequest(ex.Message);
                    if (ex is SaveException)
                        rc = Results.Problem(title: "Save", detail: ex.Message, statusCode: 500);
                    if (ex is AddCelebrityException)
                        rc = Results.Problem(title: "Add", detail: ex.Message, statusCode: 500);
                    if (ex is DeleteCelebrityById)
                        rc = Results.Problem(title: "Delete", detail: ex.Message, statusCode: 500);
                    if (ex is PutCelebrityById)
                        rc = Results.Problem(title: "Put", detail: ex.Message, statusCode: 500);
                }

                return rc;
            });
        }

        app.Run();
    }
}
