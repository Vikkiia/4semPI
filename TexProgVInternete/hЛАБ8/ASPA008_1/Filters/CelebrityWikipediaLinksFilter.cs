using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DAL_Celebrity_MSSQL; // Для Celebrity
using System.Collections.Generic;

namespace ASPA008_1.Filters
{
    public class CelebrityWikipediaLinksFilter : IActionFilter
    {
        // Выполняется перед выполнением Action
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Здесь можно получить доступ к параметрам Action
            // Например, если Action "Human" принимает int id
            // if (context.ActionArguments.ContainsKey("id"))
            // {
            //     var id = (int)context.ActionArguments["id"];
            //     // Можно получить знаменитость по id, если нужно для генерации ссылок
            // }
        }

        // Выполняется после выполнения Action
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ViewResult viewResult && viewResult.Model is Celebrity celebrity)
            {
                // Генерируем ссылки на Wikipedia на основе имени знаменитости
                var wikipediaLinks = new List<string>
                {
                    $"https://ru.wikipedia.org/wiki/{Uri.EscapeDataString(celebrity.FullName)}",
                    // Можно добавить другие ссылки, если нужно
                };

                // Передаем ссылки в ViewData, чтобы использовать в представлении Details
                viewResult.ViewData["WikipediaLinks"] = wikipediaLinks;
            }
        }
    }
}