using GameStore.Api.Dtos;
using GameStore.Api.Data;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GETGAME = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        // Get
        // /
        app.MapGet("/", () => "Game Store");

        var group = app.MapGroup("games").WithParameterValidation();

        // games/
        group.MapGet("/", async (GameStoreContext dbContext) => 
            await dbContext.Games
            .Include(game => game.Genre)
            .Select(game => game.ToSummaryDto())
            .AsNoTracking()
            .ToListAsync());

        // games/{id}
        group.MapGet("{id}", async (int id, GameStoreContext dbContext) => 
        {
            Game? game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToDetailsDto());
        }).WithName(GETGAME);

        // Post
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {

            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute
            (
                GETGAME,
                new {id = game.Id},
                game.ToDetailsDto()
            );
        });
        
        //Put
        group.MapPut("{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var exists = await dbContext.Games.FindAsync(id);

            if (exists is null)
            {
                return Results.NotFound();
            }
            
            dbContext.Entry(exists)
                .CurrentValues
                .SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();
                
                return Results.NoContent();
        });

        //Delete
        group.MapDelete("{id}", async (int id, GameStoreContext dbContext) =>
        {
           await dbContext
            .Games
            .Where(game => game.Id == id).ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }
}
