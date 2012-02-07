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
		private readonly TileSystem tileSystem;

		private readonly WorldGenerator worldGen;

		public Camera Camera { get; private set; }

		public RenderTarget2D RenderTarget { get; set; }

		private readonly SpriteBatch spriteBatch;

		private readonly GraphicsDevice graphicsDevice;

		/// <summary>
		/// Initializes a new instance of the World class.
		/// </summary>
		/// <param name="graphicsDevice">The graphics device to use when initializing the <c>SpriteBatch</c>.</param>
		public World(GraphicsDevice graphicsDevice)
		{
			tileSystem = new TileSystem(2048, 2048, 16);
			Camera = new Camera();
			this.graphicsDevice = graphicsDevice;
			spriteBatch = new SpriteBatch(this.graphicsDevice);
			RenderTarget = new RenderTarget2D(graphicsDevice, (int)Helper.GetWindowSize().X, (int)Helper.GetWindowSize().Y);

			worldGen = new WorldGenerator(ref tileSystem);
			GenerateWorld(false);
		}

		private void GenerateWorld(bool outputProgress)
		{
			this.worldGen.GenerateWorld(Helper.Rand.Next(int.MinValue, int.MaxValue), true);

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
			if(InputManager.KeyJustPressed(Keys.Enter) && worldGen.GetTilesPercentage() >= 1f) this.GenerateWorld(true);

			foreach (Tile t in tileSystem.GetTilesAt(Camera.GetRectangle(), false))
			{
				t.Update(gt);
			}
		}
		
		public void Draw(GameTime gt)
		{
			var rect = tileSystem.GetArea(Camera.Position);

			var temp = (RenderTarget2D)graphicsDevice.GetRenderTargets()[0].RenderTarget;
			graphicsDevice.SetRenderTarget(RenderTarget);

			this.spriteBatch.Begin(
				SpriteSortMode.BackToFront, null, SamplerState.PointClamp, null, null, null, Camera.GetMatrix());

			for (int i = 0; i < TileSystem.LayerCount; i++ )
				for (int y = rect.Top; y < rect.Bottom; y++)
				{
					for (int x = rect.Left; x < rect.Right; x++)
					{
						if (tileSystem.Tiles[i][x, y] == null) continue;

						tileSystem.Tiles[i][x, y].Draw(
							spriteBatch, new Rectangle(x * tileSystem.TileSize, y * tileSystem.TileSize, tileSystem.TileSize, tileSystem.TileSize), gt, !InputManager.KeyPressed(Keys.L));
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
			Console.WriteLine(this.GetMouseWorldPosition() / tileSystem.TileSize);
			return tileSystem.GetTileAt(this.GetMouseWorldPosition());
		}
	}
}