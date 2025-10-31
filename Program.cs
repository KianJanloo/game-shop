using WebApplication1.Data;
using WebApplication1.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = "Data Source=GameShop.db";
builder.Services.AddSqlite<GameShopContext>(connString);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGamesEndpoints();

app.Run();
