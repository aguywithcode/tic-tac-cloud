using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;

namespace TicTacCloud.Cli
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
                Console.WriteLine("Beginning operations...\n");

                var service = new GameService();
                var game = await service.StartGame();
                Console.WriteLine($"GameId: {game.GameId}");
                Console.WriteLine($"State: {game.State}");
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
        }
    }
}
