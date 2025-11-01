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
        group.MapGet("/", async (GameShopContext dbContext) =>
        {
            return await dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToGameSummaryDto())
                    .AsNoTracking()
                    .ToListAsync();
        });

        // GET game
        group.MapGet("/{id}", async (int id, GameShopContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        }).WithName(getGameName);

        // POST game
        group.MapPost("/", async (CreateGameDto newGame, GameShopContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            game.Genre = await dbContext.Genres.FindAsync(newGame.GenreId);

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(getGameName, new { id = game.Id }, game.ToGameDetailsDto());
        });

        // PUT game
        group.MapPut("/{id}", async (int id, UpdateGameDto updateGame, GameShopContext dbContext) =>
        {
            Game? existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
                    .CurrentValues
                    .SetValues(updateGame.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE game
        group.MapDelete("/{id}", async (int id, GameShopContext dbContext) =>
        {
            Game? existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            await dbContext.Games
                    .Where(game => game.Id == id)
                    .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }
}
