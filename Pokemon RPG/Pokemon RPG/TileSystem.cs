namespace Pokemon_RPG
{
	using System;
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;

	using Pokemon_RPG.Tiles;

	using Utilities;

	public class TileSystem
	{
		private int tileSize;

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
		/// The array that contains all tiles.
		/// </summary>
		public Tile[,] Tiles { get; private set; }

		public TileSystem(int width, int height, int tileSize)
		{
			Width = width;
			Height = height;
			TileSize = tileSize;

			Tiles = new Tile[Width,Height];
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
			else _area = new Rectangle(0, 0, Tiles.GetLength(0), Tiles.GetLength(1));

			for (int y = _area.Top; y < _area.Bottom; y++)
			{
				for (int x = _area.Left; x < _area.Right; x++)
				{
					if (Tiles[x, y] == null) continue;

					Tiles[x, y].Update(gt);
				}
			}
		}

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

		public Rectangle GetArea(Vector2 cameraPosition)
		{
			var retVal = new Rectangle(
				(int)(cameraPosition.X - Helper.GetWindowSize().X / 2f),
				(int)(cameraPosition.Y - Helper.GetWindowSize().Y / 2f),
				(int)Helper.GetWindowSize().X,
				(int)Helper.GetWindowSize().Y);

			return GetArea(retVal);
		}

		public Tile GetTileAt(Vector2 position)
		{
			int x = (int)Math.Floor(position.X / TileSize);
			int y = (int)Math.Floor(position.Y / TileSize);

			if (x >= 0 && x < Tiles.GetLength(0) && y >= 0 && y < Tiles.GetLength(1))
				return Tiles[x, y];

			return null;
		}

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
					if (_x < 0 || _x >= this.Tiles.GetLength(0) || _y < 0 || _y >= this.Tiles.GetLength(1)) continue;

					if (!addNullItems && Tiles[_x, _y] == null) continue;

					tiles.Add(Tiles[_x, _y]);
				}
			}

			return tiles;
		}

		public void ClearTiles()
		{
			for (int y = 0; y < Tiles.GetLength(1); y++)
				for (int x = 0; x < Tiles.GetLength(0); x++) Tiles[x, y] = null;
		}
	}
}