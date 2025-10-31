using WebApplication1.Data;
using WebApplication1.Endpoints;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);

Batteries.Init();

var connString = builder.Configuration.GetConnectionString("GameShop");
builder.Services.AddSqlite<GameShopContext>(connString);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
