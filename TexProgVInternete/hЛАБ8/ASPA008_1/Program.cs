using DAL_Celebrity_MSSQL; // Добавь этот using, если Celebrity, LifeEvent и IRepository нужны напрямую в Program.cs
using ANC25_WEBAPI_DLL; // Для методов расширения из твоей библиотеки

var builder = WebApplication.CreateBuilder(args);

// Подключение конфигурации и сервисов из ANC25_WEBAPI_DLL
builder.AddCelebritiesConfiguration();
builder.AddCelebritiesServices();

// Добавление сервисов для контроллеров и представлений
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ASPA008_1.Filters.CelebrityWikipediaLinksFilter>();
var app = builder.Build();

// Настройка HTTP запросов
app.UseHttpsRedirection();

// Обработка ошибок (используем наш MiddlewareErrorHandler)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Стандартный обработчик для неразвивающего окружения
    // app.UseCelebritiesErrorHandler("ASPA008_1"); // Можно использовать свой, но ASP.NET Core MVC обычно имеет встроенный
}
// Если хочешь использовать свой обработчик ошибок для всех случаев, даже в Development
// app.UseCelebritiesErrorHandler("ASPA008_1"); // Подключение обработчика ошибок

app.UseStaticFiles(); // Для доступа к файлам из wwwroot

app.UseRouting(); // Включение маршрутизации

// Маппинг Minimal API эндпоинтов из ANC25_WEBAPI_DLL
app.MapCelebrities();
app.MapLifeevents();
app.MapPhotoCelebrities(); // Для отображения фотографий через /Photos/{fileName}

app.UseAuthorization(); // Включение авторизации (если понадобится)

// Маршрутизация для MVC контроллеров
app.MapControllerRoute(
    name: "new_celebrity_form", // Дадим уникальное имя
    pattern: "/0",
    defaults: new { Controller = "Celebrities", Action = "NewHumanForm" });

app.MapControllerRoute(
    name: "celebrity_details", // Дадим уникальное имя
    pattern: "/{id:int:min(1)}",
    defaults: new { Controller = "Celebrities", Action = "Human" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Celebrities}/{action=Index}/{id?}");

app.Run();