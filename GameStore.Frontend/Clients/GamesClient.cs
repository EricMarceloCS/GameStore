using GameStore.Frontend.Models;
using GameStore.Frontend.Clients;

namespace GameStore.Frontend.Clients;

public class GamesClient
{
    private readonly Genre[] genres = new GenresClient().GetGenres();
    
    private readonly List<GameSummary> games =
    [
        new(){
            Id = 1,
            Name = "Street Fighter II",
            Genre = "Fighting",
            Price = 19.99M,
            ReleaseDate = new DateOnly(1992, 7, 15)
        },
        new(){
            Id = 2,
            Name = "Final Fantasy XIV",
            Genre = "Roleplaying",
            Price = 59.99M,
            ReleaseDate = new DateOnly(2010, 9, 30)
        },
        new(){
            Id = 3,
            Name = "FIFA 23",
            Genre = "Sports",
            Price = 69.99M,
            ReleaseDate = new DateOnly(2022, 9, 27)
        }
    ];

    public GameSummary[] GetGames() => [..games];

    public void AddGame(GameDetails gameDetails)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(gameDetails.GenreId);
        var genre = genres.Single(genre => genre.Id == int.Parse(gameDetails.GenreId));
        GameSummary gameSummary = new ()
        {
            Id = games.Count + 1,
            Name = gameDetails.Name,
            Genre = genre.Name,
            Price = gameDetails.Price,
            ReleaseDate = gameDetails.ReleaseDate
        };
        games.Add(gameSummary);
    }

}
