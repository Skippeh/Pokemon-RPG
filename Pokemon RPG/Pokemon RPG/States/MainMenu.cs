namespace Pokemon_RPG.States
{
	using System;
	using System.Collections.Generic;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Media;

	using Utilities;

	using XNAGui;

	class MainMenu : State
	{
		private readonly Menu menu;

		private float currentVolumeTarget;

		private float currentVolumeLerp;

		public MainMenu(GraphicsDevice graphicsDevice) : base(graphicsDevice)
		{
			this.menu = GuiManager.LoadedMenus["mainMenu"];
		}

		public override void Update(GameTime gt)
		{
			base.Update(gt);

			this.menu.Update(gt);

			currentVolumeLerp = MathHelper.Lerp(currentVolumeLerp, currentVolumeTarget, 5 * (float)gt.ElapsedGameTime.TotalSeconds);
			MediaPlayer.Volume = currentVolumeLerp;
		}

		public override void Draw(GameTime gt)
		{
			base.Draw(gt);

			GraphicsDevice.Clear(new Color(126, 127, 129));

			SpriteBatch.Begin();

			this.menu.Draw(SpriteBatch);

			SpriteBatch.End();
		}

		public override void OnFocus()
		{
			base.OnFocus();

			if (!HasFocusedOnce)
			{
				// Apply mouse button click sounds to all buttons and clickable labels
				foreach (Canvas canvas in this.menu.Canvases.Values)
				{
					foreach (Control control in canvas.Controls)
					{
						if (control.GetType() != typeof(Button) && control.GetType() != typeof(ClickableLabel)) continue;

						control.MouseDown += (sender, args) => ResourceHelper.LoadSound("sound\\gui\\btnClick").Play();
					}
				}
			}

			// Start music
			//MediaPlayer.Play(ResourceHelper.LoadSong("sound\\music\\mainMenu"));
			MediaPlayer.Volume = 0f;

			this.FadeMusicTo(0.5f);
		}

		public override void OnUnFocus()
		{
			base.OnUnFocus();

			// Stop/Fade music
			MediaPlayer.Stop();
		}

		private void FadeMusicTo(float amount)
		{
			currentVolumeTarget = amount;
		}
	}
}