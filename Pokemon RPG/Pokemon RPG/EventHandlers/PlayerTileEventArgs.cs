namespace Pokemon_RPG.EventHandlers
{
	using Pokemon_RPG.Tiles;

	public class PlayerTileEventArgs
	{
		public delegate void PlayerTileEventHandler(object sender, PlayerTileEventArgs args);

		/// <summary>
		/// The player that invoked the tiles event.
		/// </summary>
		public Player Player { get; private set; }

		/// <summary>
		/// The tile the player invoked an event on.
		/// </summary>
		public Tile Tile { get; private set; }

		/// <summary>
		/// Tells whether or not the tile contained the player.
		/// </summary>
		public bool ContainedPlayer;

		/// <summary>
		/// Initializes a new instance of the PlayerTileEventArgs class.
		/// </summary>
		/// <param name="player">The player that invoked the event.</param>
		/// <param name="tile">The tile that contained the event.</param>
		/// <param name="containedPlayer">Whether or not the player was inside the tile's area.</param>
		public PlayerTileEventArgs(Player player, Tile tile, bool containedPlayer)
		{
			Player = player;
			Tile = tile;
			ContainedPlayer = containedPlayer;
		}
	}
}