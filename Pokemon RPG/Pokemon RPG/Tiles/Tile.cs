namespace Pokemon_RPG.Tiles
{
	using System;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	using Pokemon_RPG.EventHandlers;

	public class Tile
	{
		public static Texture2D TextureAtlas { get; set; }

		public static readonly Tile InvalidTile = new Tile(TileId.Invalid, Solidity.NonSolid, Rectangle.Empty, Color.Yellow, TileSystem.TileSide.InvalidSide);

		public Color DrawColor { get; set; }

		public enum Solidity
		{
			NonSolid,
			Solid,
		}

		public enum TileId
		{
			Invalid,
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

		public Rectangle SourceRectangle { get; private set; }

		public TileId Id { get; private set; }

		public TileSystem.TileSide Side { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id">The type of this tile.</param>
		/// <param name="solidityType"></param>
		/// <param name="textureSourceRectangle">The appropriate source rectangle to use. see <c>SourceRects</c>.</param>
		/// <param name="drawColor"></param>
		public Tile(TileId id, Solidity solidityType, Rectangle textureSourceRectangle, Color drawColor, TileSystem.TileSide tileSide)
		{
			Id = id;
			SolidityType = solidityType;
			SourceRectangle = textureSourceRectangle;
			DrawColor = drawColor;
			Side = tileSide;
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

		public void OnPlayerIn(PlayerTileEventArgs args)
		{
			if (PlayerIn != null) PlayerIn(this, args);
		}

		public void OnPlayerOut(PlayerTileEventArgs args)
		{
			if (PlayerOut != null) PlayerOut(this, args);
		}

		public void OnPlayerOn(PlayerTileEventArgs args)
		{
			if (PlayerOn != null) PlayerOn(this, args);
		}
		#endregion
	}
}