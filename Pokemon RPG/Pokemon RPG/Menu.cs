namespace Pokemon_RPG
{
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	using Utilities;

	using XNAGui;

	public class Menu
	{
		public Dictionary<string, Canvas> Canvases { get; private set; }

		private string currentCanvasIdentifier;
		public Canvas CurrentCanvas
		{
			get
			{
				return Canvases[currentCanvasIdentifier];
			}
		}

		public Menu()
		{
			Canvases = new Dictionary<string, Canvas>();
		}

		public Canvas AddCanvas(string identifier, Canvas canvas = null)
		{
			if(canvas == null) canvas = new Canvas(Helper.GetWindowRectangle());

			Canvases.Add(identifier, canvas);

			return Canvases[identifier];
		}

		public void RemoveCanvas(string identifier)
		{
			Canvases.Remove(identifier);
		}

		public void SetCanvas(string identifier)
		{
			currentCanvasIdentifier = identifier;
		}

		public void Update(GameTime gameTime)
		{
			CurrentCanvas.Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			CurrentCanvas.Draw(spriteBatch);
		}
	}
}