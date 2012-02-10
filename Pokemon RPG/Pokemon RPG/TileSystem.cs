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

		/// <summary>
		/// Sets the tile at XY to the target tile if all conditions are met.
		/// </summary>
		/// <param name="x">The X coord</param>
		/// <param name="y">The Y coord</param>
		/// <param name="side">The side to check</param>
		/// <param name="tileType">The tile type we're searching for</param>
		/// <param name="tileEdgeType">The tile type the neighbour should be.</param>
		/// <param name="tileEdgeSide">The tile side the neighbour should have. Pass null if it doesn't matter.</param>
		/// <param name="setToTile">The tile at XY to set to if conditions are met.</param>
		public void PolishNeighbour(int x, int y, Tile.TileSide side, Tile.TileId tileType, Tile.TileId tileEdgeType, Tile setToTile)
		{
			if (this.GetTile(x, y).Id != tileType) return;

			switch (side)
			{
				case Tile.TileSide.BottomRight:
					{
						if (this.GetNeighbourTile(x, y, Tile.TileSide.Bottom).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Right).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.BottomRight).Id == tileEdgeType)
						{
							Tiles[0][x, y] = setToTile;
						}

						break;
					}
				case Tile.TileSide.BottomLeft:
					{
						if (this.GetNeighbourTile(x, y, Tile.TileSide.Bottom).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Left).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.BottomLeft).Id == tileEdgeType)
						{
							Tiles[0][x, y] = setToTile;
						}

						break;
					}
				case Tile.TileSide.TopRight:
					{
						if (this.GetNeighbourTile(x, y, Tile.TileSide.Top).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Right).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.TopRight).Id == tileEdgeType)
						{
							Tiles[0][x, y] = setToTile;
						}

						break;
					}
				case Tile.TileSide.TopLeft:
					{
						if (this.GetNeighbourTile(x, y, Tile.TileSide.Top).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Left).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.TopLeft).Id == tileEdgeType)
						{
							Tiles[0][x, y] = setToTile;
						}

						break;
					}
				case Tile.TileSide.Top:
					{
						if (this.GetNeighbourTile(x, y, Tile.TileSide.Top).Id == tileEdgeType)
						{
							Tiles[0][x, y] = setToTile;
						}

						break;
					}
				case Tile.TileSide.Bottom:
					{
						if (this.GetNeighbourTile(x, y, Tile.TileSide.Bottom).Id == tileEdgeType)
						{
							Tiles[0][x, y] = setToTile;
						}

						break;
					}
				case Tile.TileSide.Left:
					{
						if (this.GetNeighbourTile(x, y, Tile.TileSide.Left).Id == tileEdgeType)
						{
							Tiles[0][x, y] = setToTile;
						}

						break;
					}
				case Tile.TileSide.Right:
					{
						if (this.GetNeighbourTile(x, y, Tile.TileSide.Right).Id == tileEdgeType)
						{
							Tiles[0][x, y] = setToTile;
						}

						break;
					}
				case Tile.TileSide.UpperHalf:
					{
						if (this.GetNeighbourTile(x, y, Tile.TileSide.Left).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.TopLeft).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Top).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.TopRight).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Right).Id == tileEdgeType)
						{
							Tiles[0][x, y] = setToTile;
						}

						break;
					}
				case Tile.TileSide.LowerHalf:
					{
						if (this.GetNeighbourTile(x, y, Tile.TileSide.Left).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.BottomLeft).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Bottom).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.BottomRight).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Right).Id == tileEdgeType)
						{
							Tiles[0][x, y] = setToTile;
						}

						break;
					}
				case Tile.TileSide.LeftHalf:
					{
						if (this.GetNeighbourTile(x, y, Tile.TileSide.Top).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.TopLeft).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Left).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.BottomLeft).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Bottom).Id == tileEdgeType)
						{
							Tiles[0][x, y] = setToTile;
						}

						break;
					}
				case Tile.TileSide.RightHalf:
					{
						if (this.GetNeighbourTile(x, y, Tile.TileSide.Top).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.TopRight).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Left).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.BottomRight).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Bottom).Id == tileEdgeType)
						{
							Tiles[0][x, y] = setToTile;
						}

						break;
					}
				case Tile.TileSide.All:
					{
						if (this.GetNeighbourTile(x, y, Tile.TileSide.Left).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.TopLeft).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Top).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.TopRight).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Right).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.BottomRight).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.Bottom).Id == tileEdgeType
							&& this.GetNeighbourTile(x, y, Tile.TileSide.BottomLeft).Id == tileEdgeType)
						{
							Tiles[0][x, y] = setToTile;
						}

						break;
					}
			}
		}

		/// <summary>
		/// Sets the tile at XY to the target tile if all conditions are met. The only tile being checked will be the one directly to the given side.
		/// </summary>
		/// <param name="x">The X coord</param>
		/// <param name="y">The Y coord</param>
		/// <param name="side">The side to check</param>
		/// <param name="tileType">The tile type we're searching for</param>
		/// <param name="tileEdgeType">The tile type the neighbour should be.</param>
		/// <param name="setToTile">The tile at XY to set to if conditions are met.</param>
		public void PolishNeighbourExclusive(int x, int y, Tile.TileSide side, Tile.TileId tileType, Tile.TileId tileEdgeType, Tile setToTile, Tile.TileSide? tileEdgeSide)
		{
			if (this.GetTile(x, y).Id != tileType) return;

			if (this.GetNeighbourTile(x, y, side).Id == tileEdgeType)
			{
				if (tileEdgeSide != null)
				{
					if ((Tile.TileSide)tileEdgeSide == this.GetNeighbourTile(x, y, side).Side) Tiles[0][x, y] = setToTile;

					return;
				}

				Tiles[0][x, y] = setToTile;
			}
		}

		/// <summary>
		/// Smooths the terrain when these two tiles are next to eachother.
		/// </summary>
		/// <param name="x">The X coord.</param>
		/// <param name="y">The Y coord.</param>
		/// <param name="tileA">The first tile. This is the outcome of the new <c>Tile</c>.</param>
		/// <param name="tileB">The comparison tile. This is the tile that "surrounds" tileA.</param>
		/// <param name="top">Source rectangle for top part of sprite.</param>
		/// <param name="bottom">Source rectangle for bottom part of sprite.</param>
		/// <param name="left">Source rectangle for left part of sprite.</param>
		/// <param name="right">Source rectangle for right part of sprite.</param>
		/// <param name="topLeft">Source rectangle for top left part of sprite.</param>
		/// <param name="topRight">Source rectangle for top right part of sprite.</param>
		/// <param name="bottomLeft">Source rectangle for bottom left part of sprite.</param>
		/// <param name="bottomRight">Source rectangle for bottom right part of sprite.</param>
		/// <param name="upperHalf">Source rectangle for upper half of sprite. This is when tileA is surrounded by tileB on the upper half.</param>
		/// <param name="lowerHalf">Source rectangle for the lower half of sprite. This is when tileA is surrounded by tileB on the lower half.</param>
		/// <param name="circle">Source rectangle for one lonely tileA. This is when tileA is completely surrounded by tileB.</param>
		public void PolishEdgeCouple(int x, int y, Tile.TileId tileA, Tile.TileId tileB, Rectangle top, Rectangle bottom, Rectangle left, Rectangle right, Rectangle topLeft, Rectangle topRight, Rectangle bottomLeft, Rectangle bottomRight, Rectangle upperHalf, Rectangle lowerHalf, Rectangle circle)
		{
			if (Tiles[0][x, y] == null) return;

			#region Edges

			PolishNeighbour(
				x,
				y,
				Tile.TileSide.Top,
				tileA,
				tileB,
				new Tile(tileA, Tile.Solidity.NonSolid, top, Color.White, Tile.TileSide.Top));

			PolishNeighbour(
				x,
				y,
				Tile.TileSide.Bottom,
				tileA,
				tileB,
				new Tile(tileA, Tile.Solidity.NonSolid, bottom, Color.White, Tile.TileSide.Bottom));

			PolishNeighbour(
				x,
				y,
				Tile.TileSide.Left,
				tileA,
				tileB,
				new Tile(tileA, Tile.Solidity.NonSolid, left, Color.White, Tile.TileSide.Left));

			PolishNeighbour(
				x,
				y,
				Tile.TileSide.Right,
				tileA,
				tileB,
				new Tile(tileA, Tile.Solidity.NonSolid, right, Color.White, Tile.TileSide.Right));

			#endregion

			#region Corners

			PolishNeighbour(
				x,
				y,
				Tile.TileSide.BottomRight,
				tileA,
				tileB,
				new Tile(tileA, Tile.Solidity.NonSolid, bottomRight, Color.White, Tile.TileSide.BottomRight));

			PolishNeighbour(
				x,
				y,
				Tile.TileSide.BottomLeft,
				tileA,
				tileB,
				new Tile(tileA, Tile.Solidity.NonSolid, bottomLeft, Color.White, Tile.TileSide.BottomLeft));

			PolishNeighbour(
				x,
				y,
				Tile.TileSide.TopRight,
				tileA,
				tileB,
				new Tile(tileA, Tile.Solidity.NonSolid, topRight, Color.White, Tile.TileSide.TopRight));

			PolishNeighbour(
				x,
				y,
				Tile.TileSide.TopLeft,
				tileA,
				tileB,
				new Tile(tileA, Tile.Solidity.NonSolid, topLeft, Color.White, Tile.TileSide.TopLeft));

			#endregion

			#region One wide/long tile cases

			PolishNeighbour(
				x,
				y,
				Tile.TileSide.UpperHalf,
				tileA,
				tileB,
				new Tile(tileA, Tile.Solidity.NonSolid, upperHalf, Color.White, Tile.TileSide.UpperHalf));

			PolishNeighbour(
				x,
				y,
				Tile.TileSide.LowerHalf,
				tileA,
				tileB,
				new Tile(tileA, Tile.Solidity.NonSolid, lowerHalf, Color.White, Tile.TileSide.LowerHalf));

			#endregion
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">The X coord.</param>
		/// <param name="y">The Y coord.</param>
		/// <param name="topLeft">Source rectangle for top left part of sprite.</param>
		/// <param name="topRight">Source rectangle for top right part of sprite.</param>
		/// <param name="bottomLeft">Source rectangle for bottom left part of sprite.</param>
		/// <param name="bottomRight">Source rectangle for bottom right part of sprite.</param>
		public void PolishInnerGrass(int x, int y, Rectangle topLeft, Rectangle topRight, Rectangle bottomLeft, Rectangle bottomRight)
		{
			if (this.GetTile(x, y) == null) return;
			if (this.GetTile(x, y).Id != Tile.TileId.Grass) return;

			Tile tTopLeft = this.GetNeighbourTile(x, y, Tile.TileSide.TopLeft);
			Tile tTopRight = this.GetNeighbourTile(x, y, Tile.TileSide.TopRight);
			Tile tBottomRight = this.GetNeighbourTile(x, y, Tile.TileSide.BottomRight);
			Tile tBottomLeft = this.GetNeighbourTile(x, y, Tile.TileSide.BottomLeft);

			if (this.GetTile(x, y).Side == Tile.TileSide.Middle)
			{
				if (tTopRight.Id != Tile.TileId.Grass && tTopRight != Tile.InvalidTile)
				{
					Tiles[0][x, y] = new Tile(
						Tile.TileId.Grass, Tile.Solidity.NonSolid, SourceRects.SandToGrassInnerTopRight, Color.White, Tile.TileSide.TopRight);
				}
				else if (tTopLeft.Id != Tile.TileId.Grass && tTopLeft != Tile.InvalidTile)
				{
					Tiles[0][x, y] = new Tile(
						Tile.TileId.Grass, Tile.Solidity.NonSolid, SourceRects.SandToGrassInnerTopLeft, Color.White, Tile.TileSide.TopLeft);
				}
				else if (tBottomRight.Id != Tile.TileId.Grass && tBottomRight != Tile.InvalidTile)
				{
					Tiles[0][x, y] = new Tile(
						Tile.TileId.Grass, Tile.Solidity.NonSolid, SourceRects.SandToGrassInnerBottomRight, Color.White, Tile.TileSide.BottomRight);
				}
				else if (tBottomLeft.Id != Tile.TileId.Grass && tBottomLeft != Tile.InvalidTile)
				{
					Tiles[0][x, y] = new Tile(
						Tile.TileId.Grass, Tile.Solidity.NonSolid, SourceRects.SandToGrassInnerBottomLeft, Color.White, Tile.TileSide.BottomLeft);
				}
			}
		}

		public void PolishTile(int y, int x)
		{
			PolishEdgeCouple(
				x,
				y,
				Tile.TileId.Grass,
				Tile.TileId.Sand,
				SourceRects.SandToGrassTop,
				SourceRects.SandToGrassBottom,
				SourceRects.SandToGrassLeft,
				SourceRects.SandToGrassRight,
				SourceRects.SandToGrassTopLeft,
				SourceRects.SandToGrassTopRight,
				SourceRects.SandToGrassBottomLeft,
				SourceRects.SandToGrassBottomRight,
				SourceRects.SandToGrassUpperHalf,
				SourceRects.SandToGrassLowerHalf,
				SourceRects.SandToGrassCircle);

			PolishEdgeCouple(
				x,
				y,
				Tile.TileId.ShallowWater,
				Tile.TileId.Water,
				SourceRects.ShallowToWaterTop,
				SourceRects.ShallowToWaterBottom,
				SourceRects.ShallowToWaterLeft,
				SourceRects.ShallowToWaterRight,
				SourceRects.ShallowtoWaterTopLeft,
				SourceRects.ShallowToWaterTopRight,
				SourceRects.ShallowToWaterBottomLeft,
				SourceRects.ShallowToWaterBottomRight,
				Rectangle.Empty,
				// TODO: Finish graphics.
				Rectangle.Empty,
				Rectangle.Empty);

			PolishEdgeCouple(
				x,
				y,
				Tile.TileId.Sand,
				Tile.TileId.ShallowWater,
				SourceRects.SandToShallowTop,
				SourceRects.SandToShallowBottom,
				SourceRects.SandToShallowLeft,
				SourceRects.SandToShallowRight,
				SourceRects.SandToShallowTopLeft,
				SourceRects.SandToShallowTopRight,
				SourceRects.SandToShallowBottomLeft,
				SourceRects.SandToShallowBottomRight,
				SourceRects.SandToShallowUpperHalf,
				SourceRects.SandToShallowLowerHalf,
				SourceRects.SandToShallowCircle);
		}

		public Tile GetNeighbourTile(int x, int y, Tile.TileSide side)
		{
			var invalidTile = new Tile(Tile.TileId.Invalid, Tile.Solidity.NonSolid, Rectangle.Empty, Color.White, Tile.TileSide.InvalidSide);

			switch (side)
			{
				case Tile.TileSide.Top:
					return y - 1 >= 0 ? Tiles[0][x, y - 1] : Tile.InvalidTile;
				case Tile.TileSide.Bottom:
					return y + 1 < Height ? Tiles[0][x, y + 1] : Tile.InvalidTile;
				case Tile.TileSide.Left:
					return x - 1 >= 0 ? Tiles[0][x - 1, y] : Tile.InvalidTile;
				case Tile.TileSide.Right:
					return x + 1 < Width ? Tiles[0][x + 1, y] : Tile.InvalidTile;
				case Tile.TileSide.TopLeft:
					return x - 1 >= 0 && y - 1 >= 0 ? Tiles[0][x - 1, y - 1] : Tile.InvalidTile;
				case Tile.TileSide.TopRight:
					return x + 1 < Width && y - 1 >= 0 ? Tiles[0][x + 1, y - 1] : Tile.InvalidTile;
				case Tile.TileSide.BottomLeft:
					return x - 1 >= 0 && y + 1 < Height ? Tiles[0][x - 1, y + 1] : Tile.InvalidTile;
				case Tile.TileSide.BottomRight:
					return x + 1 < Width && y + 1 < Height ? Tiles[0][x + 1, y + 1] : Tile.InvalidTile;
				default:
					return Tile.InvalidTile;
			}
		}

		public Tile GetTile(int x, int y)
		{
			if (x >= 0 && y >= 0 && x < Width - 1 && y < Height - 1)
				return Tiles[0][x, y];

			return Tile.InvalidTile;
		}
	}
}