namespace HFCS_Ch9_DLEX_GoFish
{
	public class Player
	{
		private List<Card> hand = new();
		private List<Values> books = new();
		public string Name;
		public static Random _ = new();

		/// <summary>
		/// Constructor to create a player.
		/// </summary>
		/// <param name="name">Player's name</param>
		public Player(string name) => Name = name;

		/// <summary>
		/// Alternate constructor(used for unit testing)
		/// </summary>
		/// <param name="name">Player's name</param>
		/// <param name="cards">Initial set of cards</param>
		public Player(string name, IEnumerable<Card> cards)
		{
			Name = name;
			hand.AddRange( cards );
		}

		/// <summary>
		/// The cards in the player's hand
		/// </summary>
		public IEnumerable<Card> Hand => hand;

		/// <summary>
		/// The books that the player has pulled out
		/// </summary>
		public IEnumerable<Values> Books => books;

		/// <summary>
		/// Pluralize a word adding "s" if the value isn't equal to 1.
		/// </summary>
		public static string S(int s) => s == 1 ? "" : "s";

		/// <summary>
		/// Returns the current status of the player: the number of cards and books
		/// </summary>
		public string Status => $"{Name} has {hand.Count()} card{S( hand.Count )} and {books.Count} book{S( books.Count )}";

		/// <summary>
		/// Gets up to five cards from the stock
		/// </summary>
		/// <param name="stock">Stock to get the next hand from</param>
		public void GetNextHand(Deck stock)
		{
			hand.AddRange( stock.Take( 5 ) );
		}

		/// <summary>
		/// If I have any cards that match the value, return them. If I run out of cards,
		/// get the next hand from the deck
		/// </summary>
		/// <param name="value">Value I am asking forparam>
		/// <param name="deck">Deck to draw my next hand from</param>
		/// <returns>The cards that were pulled out of the other player's hand</returns>
		public IEnumerable<Card> DoYouHaveAny(Values value, Deck deck)
		{
			CardComparerByValue comparer = new();
			hand.Sort( comparer );

			var result = hand.Where( card => card.Value == value );

			//removes values
			if(result.Count() >= 1)
			{
				hand = hand.Except( result ).ToList();
				if(hand.Count() == 0)
					GetNextHand(deck);
			}
			return result;
		}

		/// <summary>
		/// When the player receives cards from another player, adds them to the hand
		/// and pulls out any matching books.
		/// </summary>
		/// <param name="cards">Cards from the player to add</param>
		public void AddCardsAndPullOutBooks(IEnumerable<Card> cards)
		{
			//AddCards
			if(cards.Count() >= 1)
				hand.AddRange( cards );
			CardComparerByValue comparer = new();
			hand.Sort( comparer );
			/*
				var groupedCardsByValue = hand
				.GroupBy(card => card.Value)
				.Select(group => group.ToList())
				.ToList();
			*/

			//pull out books
			var groupedCardsByValue =
				from card in hand
				group card by card.Value into valueGroup
				select valueGroup;

			foreach(var group in groupedCardsByValue)
			{
				const int fullBook = 4;
				Values bookValue = group.Key;
				if(group.Count() == fullBook)
				{
					foreach(var card in group)
					{
						hand.Remove( card );
					}
					books.Add( bookValue );
				}
			}
		}

		/// <summary>
		/// Draws a card from the stock and add it to the player's hand
		/// </summary>
		/// <param name="Stock">Stock to draw the card from</param>
		public void DrawCard(Deck Stock)
		{
			if(Stock.Count() > 0)
			{
				AddCardsAndPullOutBooks(new List<Card> { Stock.Deal(0)});
			}
			Console.WriteLine( "Nothing to Draw." );
		}

		/// <summary>
		/// Gets a random value from the player's hand
		/// </summary>
		/// <returns>The value of a randomly selected card in the player's hand</returns>
		public Values RandomValueFromHand()
		{
			/* var reorderedHand = from card in hand
						   orderby card.Value
						   select card.Value;
			*/

			var randomValue = hand
				.OrderBy( card => card.Value )
				.Select( card => card.Value )
				.Skip( _.Next( hand.Count() - 1 ) )
				.Take( 1 );

			return randomValue.First();
		}

		public override string ToString() => Name;
	}
}