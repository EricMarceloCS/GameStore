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
        app.MapGet("/", () => "Game Store");

        var group = app.MapGroup("games").WithParameterValidation();

        group.MapGet("/", (GameStoreContext dbContext) => dbContext.Games
        .Include(game => game.Genre)
        .Select(game => game.ToSummaryDto())
        .AsNoTracking());

        group.MapGet("{id}", (int id, GameStoreContext dbContext) => 
        {
            Game? game = dbContext.Games.Find(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToDetailsDto());
        }).WithName(GETGAME);

        // Post
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {

            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute
            (
                GETGAME,
                new {id = game.Id},
                game.ToDetailsDto()
            );
        });
        
        //Put
        group.MapPut("{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var exists = dbContext.Games.Find(id);

            if (exists == null)
            {
                return Results.NotFound();
            }
            
            dbContext.Entry(exists)
            .CurrentValues
            .SetValues(updatedGame.ToEntity(id));

            dbContext.SaveChanges();
                
                return Results.NoContent();
        });

        //Delete
        group.MapDelete("{id}", (int id, GameStoreContext dbContext) =>
        {
           dbContext.Games.Where(game => game.Id == id).ExecuteDelete();

            return Results.NoContent();
        });

        return group;
    }
}
