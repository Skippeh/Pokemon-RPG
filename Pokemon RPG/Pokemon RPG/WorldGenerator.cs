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
			Stopwatch sw = new Stopwatch();
			sw.Start();

			tileSystem.ClearTiles();

			var perlin = new Perlin { Seed = this.Seed,  };

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
						tileSystem.Tiles[x, y] = new GrassTile(Color.White);
					}
					else if (pVal <= 0.00 && pVal >= -0.10)
					{
						tileSystem.Tiles[x, y] = new SandTile(Color.White);
					}
					else if(pVal >= -0.15)
					{
						tileSystem.Tiles[x, y] = new ShallowWaterTile(Color.White);
					}
					else
					{
						color = new Color((byte)(color.R / 1.5f), (byte)(color.G / 1.5f), (byte)(color.B / 1.5f), 255);
						tileSystem.Tiles[x, y] = new WaterTile(color);
					}
				}
			}

			// Second part: Polish up terrain a bit
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					if (tileSystem.Tiles[x, y] == null) continue;

					this.PolishEdge(x, y);
					this.PolishCorner(x, y);
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
			foreach (Tile t in tileSystem.Tiles)
			{
				if (t != null) ++initializedCount;
			}

			return (float)initializedCount / tileSystem.Tiles.Length;
		}

		private void PolishCorner(int x, int y)
		{
			// Top left
			if (x - 1 >= 0 && y - 1 >= 0)
			{
				
			}

			// Top right
			if (x + 1 < Width - 1 && y - 1 >= 0)
			{
				
			}

			// Bottom right
			if (x + 1 < Width - 1 && y + 1 < Height - 1)
			{
				
			}

			// Bottom left
			if (x - 1 >= 0 && y + 1 < Height - 1)
			{
				
			}
		}

		private void PolishEdge(int x, int y)
		{
			if (x < this.Width - 1)
			{
				// Right

				if (this.tileSystem.Tiles[x, y].Id == Tile.TileId.Sand)
				{
					if (this.tileSystem.Tiles[x + 1, y].Id == Tile.TileId.Grass)
					{
						var color = this.tileSystem.Tiles[x, y].DrawColor;
						this.tileSystem.Tiles[x, y] = new SandTile(color, Tile.TileSide.Right);
					}
				}
			}

			if (x > 0)
			{
				// Left

				if (this.tileSystem.Tiles[x, y].Id == Tile.TileId.Sand)
				{
					if (this.tileSystem.Tiles[x - 1, y].Id == Tile.TileId.Grass)
					{
						var color = this.tileSystem.Tiles[x, y].DrawColor;
						this.tileSystem.Tiles[x, y] = new SandTile(color, Tile.TileSide.Left);
					}
				}
			}

			if (y < this.Height - 1)
			{
				// Bottom

				if (this.tileSystem.Tiles[x, y].Id == Tile.TileId.Sand)
				{
					if (this.tileSystem.Tiles[x, y + 1].Id == Tile.TileId.Grass)
					{
						var color = this.tileSystem.Tiles[x, y].DrawColor;
						this.tileSystem.Tiles[x, y] = new SandTile(color, Tile.TileSide.Bottom);
					}
				}
			}

			if (y > 0)
			{
				// Top

				if (this.tileSystem.Tiles[x, y].Id == Tile.TileId.Sand)
				{
					if (this.tileSystem.Tiles[x, y - 1].Id == Tile.TileId.Grass)
					{
						var color = this.tileSystem.Tiles[x, y].DrawColor;
						this.tileSystem.Tiles[x, y] = new SandTile(color, Tile.TileSide.Top);
					}
				}
			}
		}

		private void SetSeed(int? seed)
		{
			if (seed == null)
			{
				this.Seed = Helper.Rand.Next(int.MinValue, int.MaxValue);
			}
			else
			{
				this.Seed = (int)seed;
			}
		}
	}
}