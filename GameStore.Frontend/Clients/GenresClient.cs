using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GenresClient
{
    private readonly Genre[] genres =
    [
        new Genre(){
            Id = 1,
            Name="Fighting"
        },
        new Genre(){
            Id = 2,
            Name="Roleplaying"
        },
        new Genre(){
            Id = 3,
            Name="Sports"
        },
        new Genre(){
            Id = 4,
            Name="Racing"
        },
        new Genre(){
            Id = 5,
            Name="Kids and Family"
        }
    ];

    public Genre[] GetGenres()
    {
        return genres;
    }
}
