

using DAL004;
using CustomExceptions;

namespace Validation
{
    public class SurnameFilter : IEndpointFilter
    {
        public static IRepository repository = null!;//SurnameFilter.repository задает нал


        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var celebrity = context.GetArgument<Celebrity>(0);
            if (string.IsNullOrWhiteSpace(celebrity.Surname) || celebrity.Surname.Length < 2)
            {
                return Results.Conflict("POST /Celebrities error, Surname is wrong");
            }

            if (repository.GetAllCelebrities().Any(c => c.Surname == celebrity.Surname))
            {
                //throw new AddCelebrityException("POST /Celebrities error, Surname is doubled");
                return Results.Conflict("POST /Celebrities error, Surname is doubled");
            }

            return await next(context);
        }
    }

    public class PhotoExistFilter : IEndpointFilter
    {
        public static IRepository repository = null!;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
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
        }
    }

    public class PutFilter : IEndpointFilter
    {
        public static IRepository repository = null!;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.GetArgument<int>(0);
            if (repository.GetCelebrityById(id) == null)
            {
                throw new PutCelebrityById($"Celebrity id {id} not found for put.");
            }

            return await next(context);
        }
    }

    public class DeleteFilter : IEndpointFilter
    {
        public static IRepository repository = null!;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.GetArgument<int>(0);
            if (repository.GetCelebrityById(id) == null)
            {
                throw new DeleteCelebrityById($"Celebrity with id {id} not found for deletion");
            }

            return await next(context);
        }
    }
}
