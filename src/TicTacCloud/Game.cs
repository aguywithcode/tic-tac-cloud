using System;
using Newtonsoft.Json;

namespace TicTacCloud
{
    public class Game
    {
        public Game()
        {
            GameId = Guid.NewGuid().ToString();
            State = ".........";
        }

        [JsonProperty(PropertyName="id")]
        public string Id => GameId;
        public string GameId {get;}

        /**
         * The state is represented as a 9 character string representing the tic tac toe cells
         * left to right top to bottom in sequence. A '.' represents an empty cell. Cells filled
         * with an x and o are represented by 'x' and 'o', respectively.
         */
        public string State {get;}
    }
}
