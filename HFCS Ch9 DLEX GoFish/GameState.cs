using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFCS_Ch9_DLEX_GoFish
{
	public class GameState
	{
		public readonly IEnumerable<Player> Players;
		public readonly IEnumerable<Player> Opponents;
		public readonly Player HumanPlayer;
		public bool GameOver { get; private set; } = false;

		public readonly Deck Stock;

		/// <summary>
		/// Constructor creates the players and deals their first hands
		/// </summary>
		/// <param name="humanPlayerName">Name of the human player</param>
		/// <param name="opponentNames">Names of the computer players</param>
		/// <param name="stock">Shuffled stock of cards to deal from</param>
		public GameState(string humanPlayerName, IEnumerable<string> opponentNames, Deck stock)
		{
			Players = new List<Player>();
			Player humanPlayer = new Player( humanPlayerName );
			Players = Players.Append( humanPlayer );
			foreach(string name in opponentNames)
			{
				Player compPlayers = new Player( name );
				Players = Players.Append( compPlayers );
			}

			//deal cards
			foreach (Player player in Players)
			{
				player.GetNextHand( stock );
			}
			


		}

		/// <summary>
		/// Gets a random player that doesn't match the current player
		/// </summary>
		/// <param name="currentPlayer">The current player</param>
		/// <returns>A random player that the current player can ask for a card</returns>
		public Player RandomPlayer(Player currentPlayer) => throw new NotImplementedException();

		public string PlayRound(Player player, Player playerToAsk, Values valueToAskFor, Deck stock)
		{
			throw new NotImplementedException();
		}


		public string CheckForWinner() { throw new NotImplementedException(); }

	}
}
