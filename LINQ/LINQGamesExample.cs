// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace LINQ
{
    /// <summary>
    /// Question Criteria:
    /// Select 3 games that
    ///     - score of 9 or above
    ///     - released after 2048
    ///     - must be random
    /// 
    /// Ref : https://www.youtube.com/watch?v=dqheDZH_mNc&t=110s&ab_channel=Tarodev
    /// </summary>
    public class LINQGamesExample : MonoBehaviour
    {
        private Random _random = new Random();
        private List<Game> _games;
        private void Start()
        {
            InitializeData();
            Answer();
        }

        private void InitializeData()
        {
            _games = new List<Game>
            {
                new Game{Name = "Death Stranding",ReleaseDate = new DateTime(2019,11,8), SteamScore = 9},
                new Game{Name = "Dark Souls 3",ReleaseDate = new DateTime(2015,3,24), SteamScore = 9},
                new Game{Name = "Cyberpunk 2077",ReleaseDate = new DateTime(2020,9,17), SteamScore = 7},
                new Game{Name = "Valheim",ReleaseDate = new DateTime(2021,2,2), SteamScore = 10},
                new Game{Name = "Loop Hero",ReleaseDate = new DateTime(2021,3,4), SteamScore = 9},
                new Game{Name = "The Forest",ReleaseDate = new DateTime(2014,5,30), SteamScore = 8},
                new Game{Name = "Factorio",ReleaseDate = new DateTime(2016,2,21), SteamScore = 10},
                new Game{Name = "Mass Effect 3",ReleaseDate = new DateTime(2012,3,6), SteamScore = 7},
            };
        }

        private void Answer()
        {
            var suggestedGames = _games.Where(g => g.SteamScore >= 9 && g.ReleaseDate.Year > 2018)
                .OrderBy(g => _random.Next())
                .Take(3);
            
            // RespectToRate
            // var suggestedGames = _games.Where(g => g.SteamScore >= 9 && g.ReleaseDate.Year > 2018)
            //     .OrderBy(g => _random.Next())
            //     .Take(3)
            //     .AddRatingToNames();

            foreach (var game in suggestedGames)
            {
                Debug.Log("Game : " + game.Name);
            }
        }
    }

    public class Game
    {
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int SteamScore { get; set; }
    }

    public static class Helpers
    {
        public static IEnumerable<Game> AddRatingToNames(this IEnumerable<Game> games)
        {
            foreach (var game in games)
            {
                game.Name = $"{game.Name} - {game.SteamScore}";
            }

            return games;
        }
    }
}
