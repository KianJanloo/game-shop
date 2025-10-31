using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Entities;
using WebApplication1.Mapping;

namespace WebApplication1.Endpoints;

public static class GameEndpoints
{
    const string getGameName = "GetName";

    private static readonly List<GameDto> games = [
        new (
            1,
            "Call Of Duty",
            "Action",
            19.2M,
            new DateOnly(2020, 02, 02)
        ),
        new (
            2,
            "Minecraft",
            "Role Playing",
            199.2M,
            new DateOnly(2000, 11, 02)
        ),
        new (
            3,
            "Victoria III",
            "Strategic",
            5.32M,
            new DateOnly(1998, 11, 12)
        ),
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                                .WithParameterValidation();

        // GET games
        group.MapGet("/", () => games);

        // GET game
        group.MapGet("/{id}", (int id) =>
        {
            GameDto? game = games.Find((game) => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        }).WithName(getGameName);

        // POST game
        group.MapPost("/", (CreateGameDto newGame, GameShopContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            game.Genre = dbContext.Genres.Find(newGame.GenreId);

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute(getGameName, new { id = game.Id }, game.ToDto());
        });

        // PUT game
        group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
        {
            var index = games.FindIndex((game) => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updateGame.Name,
                updateGame.Genre,
                updateGame.Price,
                updateGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // DELETE game
        group.MapDelete("/{id}", (int id) =>
        {
            var index = games.FindIndex((game) => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games.RemoveAll((game) => game.Id == id);
            return Results.NoContent();
        });

        return group;
    }
}
