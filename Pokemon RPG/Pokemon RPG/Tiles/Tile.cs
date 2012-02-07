namespace Pokemon_RPG.Tiles
{
	using System;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	using Pokemon_RPG.EventHandlers;

	public class Tile
	{
		public static Texture2D TextureAtlas { get; set; }

		public Color DrawColor { get; set; }

		public enum Solidity
		{
			NonSolid,
			Solid,
		}

		public enum Behaviour
		{
			Static,
			Dynamic
		}

		public enum TileId
		{
			Grass,
			Gravel,
			Sand,
			Water,
			ShallowWater,
			Flower
		}

		/// <summary>
		/// The tile type this tile is.
		/// </summary>
		public readonly Solidity SolidityType;

		public Rectangle SourceRectangle { get; protected set; }

		public TileId Id { get; protected set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id">The type of this tile.</param>
		/// <param name="solidityType"></param>
		/// <param name="textureSourceRectangle">The appropriate source rectangle to use. see <c>SourceRects</c>.</param>
		/// <param name="drawColor"></param>
		public Tile(TileId id, Solidity solidityType, Rectangle textureSourceRectangle, Color drawColor)
		{
			Id = id;
			SolidityType = solidityType;
			SourceRectangle = textureSourceRectangle;
			DrawColor = drawColor;
		}

		public void Draw(SpriteBatch sb, Rectangle position, GameTime gt, bool useDrawColor)
		{
			sb.Draw(TextureAtlas, position, this.SourceRectangle, useDrawColor ? this.DrawColor : Color.White);
		}

		#region Events
		/// <summary>
		/// Invoked when a player enters this tiles area.
		/// </summary>
		public event PlayerTileEventArgs.PlayerTileEventHandler PlayerIn;

		/// <summary>
		/// Invoked when a player leaves this tiles area.
		/// </summary>
		public event PlayerTileEventArgs.PlayerTileEventHandler PlayerOut;

		/// <summary>
		/// Invoked when a player is inside this tiles area.
		/// </summary>
		public event PlayerTileEventArgs.PlayerTileEventHandler PlayerOn;

		public virtual void OnPlayerIn(PlayerTileEventArgs args)
		{
			if (PlayerIn != null) PlayerIn(this, args);
		}

		public virtual void OnPlayerOut(PlayerTileEventArgs args)
		{
			if (PlayerOut != null) PlayerOut(this, args);
		}

		public virtual void OnPlayerOn(PlayerTileEventArgs args)
		{
			if (PlayerOn != null) PlayerOn(this, args);
		}
		#endregion
	}
}