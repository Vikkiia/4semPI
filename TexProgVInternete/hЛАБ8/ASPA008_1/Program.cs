using DAL_Celebrity_MSSQL; // ������ ���� using, ���� Celebrity, LifeEvent � IRepository ����� �������� � Program.cs
using ANC25_WEBAPI_DLL; // ��� ������� ���������� �� ����� ����������

var builder = WebApplication.CreateBuilder(args);

// ����������� ������������ � �������� �� ANC25_WEBAPI_DLL
builder.AddCelebritiesConfiguration();
builder.AddCelebritiesServices();

// ���������� �������� ��� ������������ � �������������
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ASPA008_1.Filters.CelebrityWikipediaLinksFilter>();
var app = builder.Build();

// ��������� HTTP ��������
app.UseHttpsRedirection();

// ��������� ������ (���������� ��� MiddlewareErrorHandler)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // ����������� ���������� ��� �������������� ���������
    // app.UseCelebritiesErrorHandler("ASPA008_1"); // ����� ������������ ����, �� ASP.NET Core MVC ������ ����� ����������
}
// ���� ������ ������������ ���� ���������� ������ ��� ���� �������, ���� � Development
// app.UseCelebritiesErrorHandler("ASPA008_1"); // ����������� ����������� ������

app.UseStaticFiles(); // ��� ������� � ������ �� wwwroot

app.UseRouting(); // ��������� �������������

// ������� Minimal API ���������� �� ANC25_WEBAPI_DLL
app.MapCelebrities();
app.MapLifeevents();
app.MapPhotoCelebrities(); // ��� ����������� ���������� ����� /Photos/{fileName}

app.UseAuthorization(); // ��������� ����������� (���� �����������)

// ������������� ��� MVC ������������
app.MapControllerRoute(
    name: "new_celebrity_form", // ����� ���������� ���
    pattern: "/0",
    defaults: new { Controller = "Celebrities", Action = "NewHumanForm" });

app.MapControllerRoute(
    name: "celebrity_details", // ����� ���������� ���
    pattern: "/{id:int:min(1)}",
    defaults: new { Controller = "Celebrities", Action = "Human" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Celebrities}/{action=Index}/{id?}");

app.Run();