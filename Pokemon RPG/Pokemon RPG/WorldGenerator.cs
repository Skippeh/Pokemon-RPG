namespace Pokemon_RPG
{
	using System;
	using System.Diagnostics;
	using System.Threading;

	using Microsoft.Xna.Framework;

	using Pokemon_RPG.Tiles;

	using Utilities;
	using LibNoise;

	using Math = System.Math;

	public class WorldGenerator
	{
		/// <summary>
		/// The TileSystem's width.
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		/// The TileSystem's height.
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		/// The world seed.
		/// </summary>
		public int Seed { get; private set; }

		private TileSystem tileSystem;

		private Random rand;

		private enum TileSide
		{
			Top,
			Bottom,
			Left,
			Right,
			TopLeft,
			TopRight,
			BottomLeft,
			BottomRight
		}

		public WorldGenerator(ref TileSystem tileSystem)
		{
			this.tileSystem = tileSystem;

			Width = this.tileSystem.Width;
			Height = this.tileSystem.Height;
		}

		/// <summary>
		/// Generates a new world and then sets the tiles in the TileSystem to it.
		/// </summary>
		/// <param name="seed"></param>
		/// <param name="threaded">If true, the world will generate on another thread.</param>
		public void GenerateWorld(int? seed, bool threaded)
		{
			this.SetSeed(seed);

			if (threaded)
			{
				new Thread(() => this.GenerateWorld(this.Seed, false)).Start();

				return;
			}
			var sw = new Stopwatch();
			sw.Start();

			tileSystem.ClearTiles();

			var perlin = new Perlin { Seed = this.Seed };

			// First part: Generate landscape
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					double pVal = perlin.GetValue(x / 500d, y / 500d, 0);
					double cpVal = (pVal + 1.975) / 2;
					cpVal = MathHelper.Clamp((float)cpVal, 0f, 1f);
					var color = new Color((byte)(cpVal * 255), (byte)(cpVal * 255), (byte)(cpVal * 255), 255);

					if (pVal > 0.00)
					{
						tileSystem.Tiles[0][x, y] = new Tile(Tile.TileId.Grass, Tile.Solidity.NonSolid, SourceRects.Grass, Color.White);
					}
					else if (pVal <= 0.00 && pVal >= -0.10)
					{
						tileSystem.Tiles[0][x, y] = new Tile(
							Tile.TileId.Sand, Tile.Solidity.NonSolid, SourceRects.SandMiddle, Color.White);
					}
					else if(pVal >= -0.15)
					{
						tileSystem.Tiles[0][x, y] = new Tile(
							Tile.TileId.ShallowWater, Tile.Solidity.NonSolid, SourceRects.ShallowWater, Color.White);
					}
					else
					{
						color = new Color((byte)(color.R / 1.5f), (byte)(color.G / 1.5f), (byte)(color.B / 1.5f), 255);
						tileSystem.Tiles[0][x, y] = new Tile(Tile.TileId.Water, Tile.Solidity.NonSolid, SourceRects.Water, color);
					}
				}
			}

			// Second part: Polish up terrain a bit
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					if (tileSystem.Tiles[0][x, y] == null) continue;


				}
			}

			sw.Stop();
			Console.WriteLine("Generation time: " + sw.ElapsedMilliseconds + "ms");
		}

		/// <summary>
		/// Returns a float between 0 and 1, indicating much of the world is generated.
		/// </summary>
		/// <returns></returns>
		public float GetTilesPercentage()
		{
			int initializedCount = 0;
			foreach (Tile[,] tList in tileSystem.Tiles)
			{
				foreach(Tile t in tList)
					if (t != null) ++initializedCount;
			}

			return (float)initializedCount / tileSystem.Tiles.Length;
		}

		private void PolishCorner(int x, int y, TileSide side, Tile.TileId tileType, Tile.TileId tileEdgeType, Tile setToTile)
		{
			switch (side)
			{
				case TileSide.BottomRight:
					{
						if (this.GetTile(x, y).Id == tileType)
						{
							if (this.GetNeighbourTile(x, y, TileSide.Bottom).Id == tileEdgeType
							    && this.GetNeighbourTile(x, y, TileSide.Right).Id == tileEdgeType
							    && this.GetNeighbourTile(x, y, TileSide.BottomRight).Id == tileEdgeType)
							{
								tileSystem.Tiles[0][x, y] = setToTile;
							}
						}

						break;
					}
			}
		}

		private Tile GetNeighbourTile(int x, int y, TileSide side)
		{
			if (x < 0 || y < 0 || x >= this.Width - 1 || y >= this.Height - 1) return null;

			switch(side)
			{
				case TileSide.Top:
					return tileSystem.Tiles[0][x, y - 1];
				case TileSide.Bottom:
					return tileSystem.Tiles[0][x, y + 1];
				case TileSide.Left:
					return tileSystem.Tiles[0][x - 1, y];
				case TileSide.Right:
					return tileSystem.Tiles[0][x + 1, y];
				case TileSide.TopLeft:
					return tileSystem.Tiles[0][x - 1, y - 1];
				case TileSide.TopRight:
					return tileSystem.Tiles[0][x + 1, y - 1];
				case TileSide.BottomLeft:
					return tileSystem.Tiles[0][x - 1, y + 1];
				case TileSide.BottomRight:
					return tileSystem.Tiles[0][x + 1, y + 1];
				default:
					return null;
			}
		}

		private Tile GetTile(int x, int y)
		{
			if(x >= 0 && y >= 0 && x < Width  - 1 && y < Height - 1)
				return tileSystem.Tiles[0][x, y];

			return null;
		}

		private void SetSeed(int? seed)
		{
			if (seed == null)
			{
				rand = new Random();
				this.Seed = rand.Next(int.MinValue, int.MaxValue);
			}
			else
			{
				this.Seed = (int)seed;
				rand = new Random((int)seed);
			}
		}
	}
}