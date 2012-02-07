namespace Pokemon_RPG.States
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;
	using Microsoft.Xna.Framework.Media;

	using Utilities;

	class Intro : State
	{
		/// <summary>
		/// Amount of alpha to add/subtract per second.
		/// </summary>
		public const float FadeSpeed = 255 * 2f;

		/// <summary>
		/// Amount of time in seconds to wait before fading out.
		/// </summary>
		public const float WaitTime = 1f;

		public List<Texture2D> TextureSplashList { get; private set; }

		public List<Video> VideoSplashList { get; private set; }

		private VideoPlayer videoPlayer;

		private float currentAlpha = 1f;
		private float currentWaitTime;
		private int currentTextureSplashIndex;
		private int currentVideoSplashIndex;
		private float direction = 1f; // 1 to add alpha, -1 to subtract alpha.
		private readonly string directoryPath = Directory.GetCurrentDirectory() + "\\Content\\img\\splashes";
		private bool skipped;

		public Intro(GraphicsDevice graphicsDevice) : base(graphicsDevice)
		{
			if (Program.Arguments.Contains("-nosplash"))
			{
				Finished = true;
				return;
			}

			this.TextureSplashList = new List<Texture2D>();
			VideoSplashList = new List<Video>();

			Console.WriteLine("Loading texture splashes...");
			this.LoadTextureSplashes();
			Console.WriteLine("Loading video splashes...");

			videoPlayer = new VideoPlayer();
			this.videoPlayer.Play(this.VideoSplashList[this.currentVideoSplashIndex]);
		}

		public override void Update(GameTime gt)
		{
			base.Update(gt);
			if (Finished) return;

			if (this.currentVideoSplashIndex < this.VideoSplashList.Count)
			{
				if (this.videoPlayer.State == MediaState.Stopped)
				{
					if (this.currentVideoSplashIndex + 1 < this.VideoSplashList.Count)
					{
						Console.WriteLine("Playing next video...");
						++this.currentVideoSplashIndex;

						this.videoPlayer.Play(this.VideoSplashList[this.currentVideoSplashIndex]);
					}
					else
					{
						++currentVideoSplashIndex; // Increment so the first if statement above will be false, so we can continue to showing the texture splashes.
						return;
					}
				}
				else
				{
					if (InputManager.LmbJustPressed() || InputManager.KeyJustPressed(Keys.Enter))
					{
						videoPlayer.Stop();
						++currentVideoSplashIndex;
					}
					else if (InputManager.KeyJustPressed(Keys.Escape))
					{
						videoPlayer.Stop();
						currentVideoSplashIndex = VideoSplashList.Count;
						currentTextureSplashIndex = TextureSplashList.Count;

						Finished = true;
					}

					return;
				}
			}

			currentAlpha += FadeSpeed * (float)gt.ElapsedGameTime.TotalSeconds * direction;
			currentAlpha = MathHelper.Clamp(currentAlpha, 0, 255);

			if (InputManager.LmbJustPressed() || InputManager.KeyJustPressed(Keys.Enter) && !skipped)
			{
				skipped = true;
				currentAlpha = 0f;
				currentWaitTime = WaitTime;
			}
			else if (InputManager.KeyJustPressed(Keys.Escape))
			{
				skipped = true;
				currentAlpha = 0f;
				currentWaitTime = WaitTime;
				this.currentTextureSplashIndex = this.TextureSplashList.Count;
				this.currentVideoSplashIndex = this.VideoSplashList.Count;
			}

			if (currentAlpha <= 0f) // If done fading out...
			{
				if (this.currentTextureSplashIndex + 1 < this.TextureSplashList.Count) // If we haven't reached the final splash image proceed to the next one.
				{
					this.ResetCounters();
					++this.currentTextureSplashIndex;
				}
				else
				{
					// The intro is finished.
					Finished = true;
				}
			}
			else if (currentAlpha >= 255f) // If fully visible...
			{
				currentWaitTime += (float)gt.ElapsedGameTime.TotalSeconds;

				if (currentWaitTime >= WaitTime) // If wait time is finished...
				{
					direction *= -1;
				}
			}
		}

		public override void Draw(GameTime gt)
		{
			base.Draw(gt);

			GraphicsDevice.Clear(Color.Black);

			if (Finished) return;

			SpriteBatch.Begin();

			if (videoPlayer.State == MediaState.Playing)
			{
				SpriteBatch.Draw(videoPlayer.GetTexture(), Helper.GetWindowRectangle(), Color.White);
			}
			else
			{
				Texture2D texture = this.TextureSplashList[this.currentTextureSplashIndex];
				var size = new Vector2(texture.Width, texture.Height);
				var color = new Color((byte)currentAlpha, (byte)currentAlpha, (byte)currentAlpha, (byte)currentAlpha);

				SpriteBatch.Draw(texture, Helper.GetWindowCenter(size), color);
			}

			SpriteBatch.End();
		}

		private void ResetCounters()
		{
			currentAlpha = 1f;
			currentWaitTime = 0f;
			direction = 1f;
			skipped = false;
		}

		private void LoadTextureSplashes()
		{
			foreach (string fileName in Directory.GetFiles(this.directoryPath))
			{
				if (!fileName.EndsWith(".xnb", true, CultureInfo.InvariantCulture)) continue;

				string filePath =
					fileName.Substring(fileName.LastIndexOf("Content\\", StringComparison.InvariantCulture) + "Content\\".Length).Replace
						(".xnb", "");
				try
				{
					this.TextureSplashList.Add(ResourceHelper.LoadTexture(filePath));
				}
				catch(ContentLoadException)
				{
					// Is of Video type.
					this.VideoSplashList.Add(ResourceHelper.LoadVideo(filePath));
				}
			}
		}
	}
}