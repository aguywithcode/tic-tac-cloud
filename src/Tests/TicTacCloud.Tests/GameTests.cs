using TicTacCloud;
using Xunit;

namespace TicTacCloud.Tests
{
    public class When_creating_a_new_game
    {
        Game game;

        public When_creating_a_new_game()
        {
            game = new Game();
        }

        [Fact]
        public void The_initial_game_state_should_be_a_blank_board()
        {
            Assert.Equal(".........", game.State);
        }

    }
}
