namespace Pokemon_RPG
{
	using System;
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;

	using Pokemon_RPG.Tiles;

	using Utilities;

	public class TileSystem
	{
		public const int LayerCount = 2;

		/// <summary>
		/// The tile's sizes.
		/// </summary>
		public int TileSize
		{
			get
			{
				return this.tileSize;
			}
			set
			{
				this.tileSize = value > 0 ? value : 1;
			}
		}

		/// <summary>
		/// Total tiles on the X axis.
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		/// Total tiles on the Y axis.
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		/// The array that contains all tiles. Layer 0 draws first.
		/// </summary>
		public Tile[][,] Tiles { get; private set; }

		private int tileSize;

		/// <summary>
		/// Inititalizes a new instance of the TileSystem class.
		/// </summary>
		/// <param name="width">Total amount of tiles in the X axis.</param>
		/// <param name="height">Total amount of tiles in the Y axis.</param>
		/// <param name="tileSize">The tile size in pixels.</param>
		public TileSystem(int width, int height, int tileSize)
		{
			Width = width;
			Height = height;
			TileSize = tileSize;

			Tiles = new Tile[LayerCount][,];

			for(int i = 0; i < LayerCount; i++) Tiles[i] = new Tile[Width,Height];
		}

		/// <summary>
		/// Updates the tiles within the given rectangle.
		/// </summary>
		/// <param name="area">The area of tiles to update. Pass empty to update all tiles.</param>
		/// <param name="gt"></param>
		public void Update(Rectangle area, GameTime gt)
		{
			Rectangle _area;
			if (area != Rectangle.Empty) _area = area;
			else _area = new Rectangle(0, 0, Tiles[0].GetLength(0), Tiles[0].GetLength(1));

			for (int i = 0; i < LayerCount; i++ )
				for (int y = _area.Top; y < _area.Bottom; y++)
				{
					for (int x = _area.Left; x < _area.Right; x++)
					{
						if (Tiles[0][x, y] == null) continue;

						//Tiles[i][x, y].Update(gt);
					}
				}
		}

		/// <summary>
		/// Converts an area to be used in array, then returns it.
		/// </summary>
		/// <param name="posSize">The starting X, Y, and Width, Height</param>
		/// <returns></returns>
		public Rectangle GetArea(Rectangle posSize)
		{
			Rectangle retVal = posSize;

			retVal.X /= TileSize;
			retVal.Y /= TileSize;
			retVal.Width /= TileSize;
			retVal.Height /= TileSize;

			retVal.X = (int)MathHelper.Clamp(retVal.X, 0, Width);
			retVal.Y = (int)MathHelper.Clamp(retVal.Y, 0, Height);

			if (retVal.Right >= Width) retVal.X -= (retVal.Right - Width);
			if (retVal.Bottom >= Height) retVal.Y -= (retVal.Bottom - Width);

			return retVal;
		}

		/// <summary>
		/// Returns the area around the camera position, uses the window size to determine the size and position of the rectangle.
		/// </summary>
		/// <param name="cameraPosition"></param>
		/// <returns></returns>
		public Rectangle GetArea(Vector2 cameraPosition)
		{
			var retVal = new Rectangle(
				(int)(cameraPosition.X - Helper.GetWindowSize().X / 2f),
				(int)(cameraPosition.Y - Helper.GetWindowSize().Y / 2f),
				(int)Helper.GetWindowSize().X,
				(int)Helper.GetWindowSize().Y);

			return GetArea(retVal);
		}

		/// <summary>
		/// Returns the tile at the given position. Position is in pixels, not tiles.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Tile GetTileAt(Vector2 position)
		{
			int x = (int)Math.Floor(position.X / TileSize);
			int y = (int)Math.Floor(position.Y / TileSize);

			if (x >= 0 && x < Tiles[0].GetLength(0) && y >= 0 && y < Tiles[0].GetLength(1))
				return Tiles[0][x, y];

			return null;
		}

		/// <summary>
		/// Returns the tiles within the given area. Area is in pixels, not tiles.
		/// </summary>
		/// <param name="area">Area is in pixels, not tiles.</param>
		/// <param name="addNullItems">Whether or not to add null tiles.</param>
		/// <returns></returns>
		public IEnumerable<Tile> GetTilesAt(Rectangle area, bool addNullItems)
		{
			var tiles = new List<Tile>();

			int x = area.Left / TileSize;
			int y = area.Top / TileSize;
			int width = area.Width / TileSize;
			int height = area.Height / TileSize;

			for (int _y = y; _y < height; _y++)
			{
				for (int _x = x; _x < width; _x++)
				{
					if (_x < 0 || _x >= this.Tiles[0].GetLength(0) || _y < 0 || _y >= this.Tiles[0].GetLength(1)) continue;

					if (!addNullItems && Tiles[0][_x, _y] == null) continue;

					tiles.Add(Tiles[0][_x, _y]);
				}
			}

			return tiles;
		}

		/// <summary>
		/// Assigns null to all tiles.
		/// </summary>
		public void ClearTiles()
		{
			foreach (Tile[,] tileArray in Tiles)
			{
				for (int y = 0; y < tileArray.GetLength(1); y++) for (int x = 0; x < tileArray.GetLength(0); x++) tileArray[x, y] = null;
			}
		}
	}
}