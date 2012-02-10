namespace Pokemon_RPG.Tiles
{
	using System;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	using Pokemon_RPG.EventHandlers;

	public class Tile
	{
		public static Animation WaterAnimation { get; private set; }

		private static Texture2D textureAtlas;

		public static Texture2D TextureAtlas
		{
			get
			{
				return textureAtlas;
			}
			set
			{
				textureAtlas = value;
				WaterAnimation = new Animation(textureAtlas, new Rectangle(0, 3016, 128, 16), 4, 16, 16);
			}
		}

		public static readonly Tile InvalidTile = new Tile(TileId.Invalid, Solidity.NonSolid, Rectangle.Empty, Color.Yellow, Tile.TileSide.InvalidSide);

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
			Flower,
			Tree
		}

		/// <summary>
		/// The type of solidity this tile has.
		/// </summary>
		public readonly Solidity SolidityType;

		public Rectangle SourceRectangle { get; private set; }

		public TileId Id { get; private set; }

		public TileSide Side { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id">The type of this tile.</param>
		/// <param name="solidityType"></param>
		/// <param name="textureSourceRectangle">The appropriate source rectangle to use. see <c>SourceRects</c>.</param>
		/// <param name="drawColor"></param>
		public Tile(TileId id, Solidity solidityType, Rectangle textureSourceRectangle, Color drawColor, TileSide tileSide)
		{
			this.Id = id;
			this.SolidityType = solidityType;
			this.SourceRectangle = textureSourceRectangle;
			this.DrawColor = drawColor;
			this.Side = tileSide;
		}

		public static void UpdateAnimations(GameTime gt)
		{
			WaterAnimation.Update(gt);
		}

		public void Update(GameTime gt)
		{
			
		}

		public void Draw(SpriteBatch sb, Rectangle position, GameTime gt, bool useDrawColor)
		{
			switch (Id)
			{
				default:
					{
						sb.Draw(TextureAtlas, position, this.SourceRectangle, useDrawColor ? this.DrawColor : Color.White);

						break;
					}
				case TileId.Water:
					{
						sb.Draw(TextureAtlas, position, WaterAnimation.GetSourceRectangle(), useDrawColor ? DrawColor : Color.White);

						break;
					}
			}
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
			if (this.PlayerIn != null) this.PlayerIn(this, args);
		}

		public void OnPlayerOut(PlayerTileEventArgs args)
		{
			if (this.PlayerOut != null) this.PlayerOut(this, args);
		}

		public void OnPlayerOn(PlayerTileEventArgs args)
		{
			if (this.PlayerOn != null) this.PlayerOn(this, args);
		}
		#endregion

		public enum TileSide
		{
			InvalidSide,
			Top,
			Bottom,
			Left,
			Right,
			TopLeft,
			TopRight,
			BottomLeft,
			BottomRight,
			All,
			UpperHalf,
			LowerHalf,
			LeftHalf,
			RightHalf,
			Middle
		}
	}
}