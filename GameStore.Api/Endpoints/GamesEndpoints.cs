using GameStore.Api.Dtos;
namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GETGAME = "GetGame";

    private static List<GameDto> games = [
        new (
            1,
            "Street Fighter II",
            "Fighting",
            19.99M,
            new DateOnly(1992, 7, 15)
        ),
        new (
            2,
            "Final Fantasy XIV",
            "Role Playing",
            59.99M,
            new DateOnly(2010, 9, 30)
        ),
        new (
            3,
            "FIFA",
            "Sports",
            69.99M,
            new DateOnly(2022, 9, 27)
        )
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Game Store");

        var group = app.MapGroup("games").WithParameterValidation();

        group.MapGet("/", () => games);

        group.MapGet("{id}", (int id) => 
        {
            GameDto? game = games.Find((g) => g.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
            }).WithName(GETGAME);


        group.MapPost("/", (CreateGameDto newGame) =>
        {
            if (string.IsNullOrEmpty(newGame.Name))
            {
                return Results.BadRequest("Name is required.");
            }

            GameDto game = new GameDto(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );

            games.Add(game);

            return Results.CreatedAtRoute(GETGAME, new {id = game.Id}, game);
        });

        group.MapPut("{id}", (int id, UpdateGameDto updatedGame) =>
        {
            int index = games.FindIndex((game) => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }
            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate);
                
                return Results.NoContent();
        });

        group.MapDelete("{id}", (int id) =>
        {
            games.RemoveAll((games) => games.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}
