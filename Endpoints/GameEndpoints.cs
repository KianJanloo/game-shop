using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Entities;
using WebApplication1.Mapping;

namespace WebApplication1.Endpoints;

public static class GameEndpoints
{
    const string getGameName = "GetName";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                                .WithParameterValidation();

        // GET games
        group.MapGet("/", (GameShopContext dbContext) =>
        {
            return dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToGameSummaryDto())
                    .AsNoTracking();
        });

        // GET game
        group.MapGet("/{id}", (int id, GameShopContext dbContext) =>
        {
            Game? game = dbContext.Games.Find(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        }).WithName(getGameName);

        // POST game
        group.MapPost("/", (CreateGameDto newGame, GameShopContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            game.Genre = dbContext.Genres.Find(newGame.GenreId);

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute(getGameName, new { id = game.Id }, game.ToGameDetailsDto());
        });

        // PUT game
        group.MapPut("/{id}", (int id, UpdateGameDto updateGame, GameShopContext dbContext) =>
        {
            Game? existingGame = dbContext.Games.Find(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
                    .CurrentValues
                    .SetValues(updateGame.ToEntity(id));

            dbContext.SaveChanges();

            return Results.NoContent();
        });

        // DELETE game
        group.MapDelete("/{id}", (int id, GameShopContext dbContext) =>
        {
            Game? existingGame = dbContext.Games.Find(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Games
                    .Where(game => game.Id == id)
                    .ExecuteDelete();

            return Results.NoContent();
        });

        return group;
    }
}
