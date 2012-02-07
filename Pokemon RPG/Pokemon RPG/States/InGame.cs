namespace Pokemon_RPG.States
{
	using System;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;

	using Utilities;

	class InGame : State
	{
		public float CameraSpeed = 1000f;

		public World World { get; private set; }

		public InGame(GraphicsDevice graphicsDevice)
			: base(graphicsDevice)
		{
			World = new World(graphicsDevice);
		}

		public override void Update(GameTime gt)
		{
			base.Update(gt);

			var elapsed = (float)gt.ElapsedGameTime.TotalSeconds;

			if (InputManager.KeyPressed(Keys.A))
			{
				World.Camera.Move(new Vector2(-CameraSpeed * elapsed, 0));
			}

			if (InputManager.KeyPressed(Keys.D))
			{
				World.Camera.Move(new Vector2(CameraSpeed * elapsed, 0));
			}

			if (InputManager.KeyPressed(Keys.W))
			{
				World.Camera.Move(new Vector2(0, -CameraSpeed * elapsed));
			}

			if (InputManager.KeyPressed(Keys.S))
			{
				World.Camera.Move(new Vector2(0, CameraSpeed * elapsed));
			}

			if (InputManager.LmbJustPressed())
			{
				if (World.GetMouseTile() != null) Console.WriteLine(World.GetMouseTile().GetType().Name + ", Side: " + World.GetMouseTile().Side);
			}

			World.Update(gt);
		}

		public override void Draw(GameTime gt)
		{
			base.Draw(gt);
			World.Draw(gt); // Uses its own spritebatch and render target.

			SpriteBatch.Begin();

			SpriteBatch.Draw(World.RenderTarget, Vector2.Zero, Color.White);

			SpriteBatch.End();
		}

		public override void ResetRenderTarget(GraphicsDevice graphicsDevice)
		{
			base.ResetRenderTarget(graphicsDevice);

			World.RenderTarget = new RenderTarget2D(graphicsDevice, (int)Helper.GetWindowSize().X, (int)Helper.GetWindowSize().Y);
		}
	}
}