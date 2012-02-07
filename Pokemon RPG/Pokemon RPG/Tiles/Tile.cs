namespace Pokemon_RPG.Tiles
{
	using System;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	using Pokemon_RPG.EventHandlers;

	public class Tile
	{
		public static Texture2D TextureAtlas { get; set; }

		public enum TileSide
		{
			Top,
			Right,
			Bottom,
			Left,
			Middle,
			TopLeft,
			TopRight,
			BottomRight,
			BottomLeft
		}

		public Color DrawColor { get; set; }

		/// <summary>
		/// All available tile types.
		/// </summary>
		public enum TileType
		{
			NonSolid,
			Solid
		}

		public enum TileId
		{
			Grass,
			Gravel,
			Sand,
			Water,
			ShallowWater
		}

		public TileSide Side { get; protected set; }

		/// <summary>
		/// The tile type this tile is.
		/// </summary>
		public readonly TileType Type;

		public Rectangle SourceRectangle { get; protected set; }

		public int Id { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the Tile class.
		/// </summary>
		/// <param name="type">The type this tile is of.</param>
		/// <param name="textureSourceRectangle">The source position and size of the texture part to draw for this tile.</param>
		/// <param name="drawColor">The draw color to use. A white color means the texture will look normal.</param>
		public Tile(TileId id, TileType type, Rectangle textureSourceRectangle, Color drawColor, TileSide tileSide = TileSide.Middle)
		{
			Id = (int)id;
			Type = type;
			SourceRectangle = textureSourceRectangle;
			DrawColor = drawColor;
			Side = tileSide;
		}

		public virtual void Update(GameTime gt)
		{
		}

		public virtual void Draw(SpriteBatch sb, Rectangle position, GameTime gt, bool useDrawColor)
		{
			if (useDrawColor) sb.Draw(TextureAtlas, position, SourceRectangle, DrawColor);
			else sb.Draw(TextureAtlas, position, SourceRectangle, Color.White);
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