namespace Pokemon_RPG.States
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	using Utilities;

	using XNAGui;

	class WorldEditor : State
	{
		private Canvas canvas = GuiManager.LoadedMenus["worldEditor"].Canvases["mainCanvas"];

		public WorldEditor(GraphicsDevice graphicsDevice)
			: base(graphicsDevice)
		{

		}

		public override void Update(GameTime gt)
		{
			base.Update(gt);

			canvas.Update(gt);
		}

		public override void Draw(GameTime gt)
		{
			base.Draw(gt);

			GraphicsDevice.Clear(new Color(156, 148, 152));

			SpriteBatch.Begin();

			canvas.Draw(SpriteBatch);

			SpriteBatch.End();
		}
	}
}