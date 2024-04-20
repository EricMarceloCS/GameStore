using GameStore.Frontend.Models;
using GameStore.Frontend.Clients;

namespace GameStore.Frontend.Clients;

public class GamesClient(HttpClient httpClient)
{

    public async Task<GameSummary[]> GetGamesAsync() => await httpClient.GetFromJsonAsync<GameSummary[]>("games") ?? [];

    public async Task AddGameAsyc(GameDetails gameDetails) => await httpClient.PostAsJsonAsync("games", gameDetails);

    public async Task<GameDetails> GetGameAsync(int id) => await httpClient.GetFromJsonAsync<GameDetails>($"games/{id}") ?? 
        throw new Exception("Could not find game.");

    public async Task UpdateGameAsync(GameDetails updatedGame) => await httpClient.PutAsJsonAsync($"games/{updatedGame.Id}", updatedGame);

    public async Task DeleteGameAsync(int id) => await httpClient.DeleteAsync($"games/{id}");

}
