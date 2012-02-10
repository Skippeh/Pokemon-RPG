namespace Pokemon_RPG
{
	using System;
	using System.Threading;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;

	using Pokemon_RPG.Tiles;

	using Utilities;

	public class World
	{
		public Camera Camera { get; private set; }

		public RenderTarget2D RenderTarget { get; set; }

		public readonly TileSystem TileSystem;

		private readonly WorldGenerator worldGen;

		private readonly SpriteBatch spriteBatch;

		private readonly GraphicsDevice graphicsDevice;

		/// <summary>
		/// Initializes a new instance of the World class.
		/// </summary>
		/// <param name="graphicsDevice">The graphics device to use when initializing the <c>SpriteBatch</c>.</param>
		public World(GraphicsDevice graphicsDevice, int seed)
		{
			this.TileSystem = new TileSystem(2048, 2048, 16);
			Camera = new Camera();
			this.graphicsDevice = graphicsDevice;
			spriteBatch = new SpriteBatch(this.graphicsDevice);
			RenderTarget = new RenderTarget2D(graphicsDevice, (int)Helper.GetWindowSize().X, (int)Helper.GetWindowSize().Y);

			worldGen = new WorldGenerator(ref this.TileSystem);
			GenerateWorld(false, seed);
		}

		private void GenerateWorld(bool outputProgress, int seed)
		{
			this.worldGen.GenerateWorld(seed, true);

			if (!outputProgress) return;

			new Thread(
				() =>
					{
						while (true)
						{
							float percentage = this.worldGen.GetTilesPercentage();

							Console.WriteLine("World generation progress: " + percentage);

							if (percentage >= 1f)
							{
								break;
							}
						}

						Console.WriteLine("World generated!");
					}).Start();
		}

		public void Update(GameTime gt)
		{
			this.TileSystem.Update(this.TileSystem.GetArea(Camera.GetPosition()), gt);
		}
		
		public void Draw(GameTime gt)
		{
			var rect = this.TileSystem.GetArea(Camera.Position);

			var temp = (RenderTarget2D)graphicsDevice.GetRenderTargets()[0].RenderTarget;
			graphicsDevice.SetRenderTarget(RenderTarget);

			this.spriteBatch.Begin(
				0, null, SamplerState.PointClamp, null, null, null, Camera.GetMatrix());

			for (int i = 0; i < TileSystem.LayerCount; i++)
			{
				for (int y = rect.Top; y < rect.Bottom; y++)
				{
					for (int x = rect.Left; x < rect.Right; x++)
					{
						if (this.TileSystem.Tiles[i][x, y] == null) continue;

						this.TileSystem.Tiles[i][x, y].Draw(
							spriteBatch,
							new Rectangle(x * this.TileSystem.TileSize, y * this.TileSystem.TileSize, this.TileSystem.TileSize, this.TileSystem.TileSize),
							gt,
							!InputManager.KeyPressed(Keys.L));
					}
				}
			}

			this.spriteBatch.End();

			graphicsDevice.SetRenderTarget(temp);
		}

		/// <summary>
		/// Returns the mouse's world position.
		/// </summary>
		/// <returns></returns>
		public Vector2 GetMouseWorldPosition()
		{
			return ((InputManager.MousePosition - Helper.GetWindowCenter()) + Camera.GetPosition()) / Camera.Zoom;
		}

		/// <summary>
		/// Returns the tile the mouse is over. Returns null if outside the world area.
		/// </summary>
		/// <returns></returns>
		public Tile GetMouseTile()
		{
			Console.WriteLine(this.GetMouseTilePosition());
			return this.TileSystem.GetTileAt(this.GetMouseWorldPosition());
		}

		public Vector2 GetMouseTilePosition()
		{
			return GetMouseWorldPosition() / TileSystem.TileSize;
		}
	}
}