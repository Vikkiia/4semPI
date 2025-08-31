var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();

app.UseWelcomePage("/aspnetcore");
app.UseStaticFiles();


app.MapGet("/aspnetcore", () => "Hello World!");

app.Run();
