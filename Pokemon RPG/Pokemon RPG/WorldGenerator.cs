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

			Console.WriteLine("[WorldGeneration]: Generating terrain...");

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
						tileSystem.Tiles[0][x, y] = new Tile(Tile.TileId.Grass, Tile.Solidity.NonSolid, SourceRects.Grass, Color.White, TileSystem.TileSide.Middle);
					}
					else if (pVal <= 0.00 && pVal >= -0.10)
					{
						tileSystem.Tiles[0][x, y] = new Tile(
							Tile.TileId.Sand, Tile.Solidity.NonSolid, SourceRects.SandMiddle, Color.White, TileSystem.TileSide.Middle);
					}
					else if(pVal >= -0.15)
					{
						tileSystem.Tiles[0][x, y] = new Tile(
							Tile.TileId.ShallowWater, Tile.Solidity.NonSolid, SourceRects.ShallowWaterMiddle, Color.White, TileSystem.TileSide.Middle);
					}
					else
					{
						color = new Color((byte)(color.R / 1.5f), (byte)(color.G / 1.5f), (byte)(color.B / 1.5f), 255);
						tileSystem.Tiles[0][x, y] = new Tile(Tile.TileId.Water, Tile.Solidity.NonSolid, SourceRects.Water, color, TileSystem.TileSide.Middle);
					}
				}
			}

			Console.WriteLine("[WorldGeneration]: Finished terrain, smoothing terrain...");

			// Second part: Polish up terrain a bit
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					tileSystem.PolishEdgeCouple(
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

					tileSystem.PolishEdgeCouple(
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
						Rectangle.Empty, // TODO: Finish graphics.
						Rectangle.Empty,
						Rectangle.Empty);

					tileSystem.PolishEdgeCouple(
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
			}

			Console.WriteLine("[WorldGeneration]: Finished edges. Doing inner edges...");

			// Has to be done after doing edges, else it won't be correct in some cases.
			for (int y = 0; y < Height; y++)
				for (int x = 0; x < Width; x++)
				{
					tileSystem.PolishInnerGrass(
						x,
						y,
						SourceRects.SandToGrassInnerTopLeft,
						SourceRects.SandToGrassInnerTopRight,
						SourceRects.SandToGrassInnerBottomLeft,
						SourceRects.SandToGrassInnerBottomRight);
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