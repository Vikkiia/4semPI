using DAL_Celebrity;
using DAL_Celebrity_MSSQL;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace ASPA008_1.Filters
{
    public class InfoAsyncActionFilter : Attribute, IAsyncActionFilter
    {
        public const string Wikipedia = "WIKI";
        public const string Facebook = "FACE";

        private readonly string _infoType;

        public InfoAsyncActionFilter(string infoType = "")
        {
            _infoType = infoType.ToUpper();
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var repo = context.HttpContext.RequestServices.GetService<IRepository>();
            if (repo == null)
            {
                await next();
                return;
            }
            
            if (!context.ActionArguments.TryGetValue("id", out var idObj) || !(idObj is int id) || id <= 0)
            {
                await next();
                return;
            }

            var celebrity = repo.GetCelebrityById(id);
            if (celebrity == null)
            {
                await next();// // Если объект не найден в БД — не подгружаем внешние данные
                return;
            }
         
            if (_infoType.Contains(Wikipedia))
            {
                var wikiReferences = await WikiInfoCelebrity.GetReferences(celebrity.FullName);
                context.HttpContext.Items[Wikipedia] = wikiReferences;
                /* Сохраняем результат в HttpContext.Items, чтобы потом извлечь в контроллере или представлении.*/
            }

            if (_infoType.Contains(Facebook))
            {
                context.HttpContext.Items[Facebook] = GetFromFacebook(celebrity.FullName);
            }

            await next();
        }

        private static string GetFromFacebook(string fullName)
        {
            return "Info from Face";
        }
    }
    public class WikiInfoCelebrity//для получения информации из Wikipedia по имени знаменитости.
    {
        private readonly HttpClient _client;
        private readonly Dictionary<string, string> _wikiReferences;
        private readonly string _wikiURI;
       
        private WikiInfoCelebrity(string fullName)
        {
            _client = new HttpClient();
            _wikiReferences = new Dictionary<string, string>();
            _wikiURI = $"https://en.wikipedia.org/w/api.php?action=opensearch&search={Uri.EscapeDataString(fullName)}&prop=info&format=json";
        }


        public static async Task<Dictionary<string, string>> GetReferences(string fullName)
        {
            var info = new WikiInfoCelebrity(fullName);
            HttpResponseMessage message = await info._client.GetAsync(info._wikiURI);//оздаём объект и выполняем GET-запрос к Wikipedia API.
            /* Если запрос успешен — читаем JSON-ответ как список объектов (в структуре Wikipedia OpenSearch).*/
            if (message.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await message.Content.ReadFromJsonAsync<List<object>>();

                if (result != null && result.Count >= 4)
                {
                    var titles = JsonSerializer.Deserialize<List<string>>(result[1].ToString());// Извлекаем заголовки (result[1]) и ссылки (result[3]) из массива результата.
                    var urls = JsonSerializer.Deserialize<List<string>>(result[3].ToString());// Сериализуем каждый элемент в List<string>.

                    if (titles != null && urls != null && titles.Count == urls.Count)
                    {
                        for (int i = 0; i < titles.Count; i++)
                        {
                            info._wikiReferences[titles[i]] = urls[i]; 
                        }
                    }
                }
            }

            return info._wikiReferences; 
        }
    }
}
