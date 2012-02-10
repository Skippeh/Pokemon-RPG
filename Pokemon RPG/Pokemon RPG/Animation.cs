using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon_RPG
{
	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class Animation
	{
		public readonly Texture2D SpriteAtlas;
		public readonly Rectangle AnimationArea;
		public float Fps { get; set; }
		public readonly int FrameWidth;
		public readonly int FrameHeight;
		public readonly int FrameCount;

		public Rectangle CurrentFrame
		{
			get { return new Rectangle(this.currentFrame*FrameWidth, 0, SpriteAtlas.Height, FrameWidth); }
		}

		private float elapsedTime;
		private int currentFrame;

		public Animation(Texture2D spriteAtlas, Rectangle animationArea, float fps, int frameWidth, int frameHeight)
		{
			AnimationArea = animationArea;
			SpriteAtlas = spriteAtlas;
			Fps = 1000/fps;
			FrameCount = animationArea.Width/frameWidth;
			FrameWidth = frameWidth;
			FrameHeight = frameHeight;
		}

		public void Update(GameTime gt)
		{
			this.elapsedTime += (float) gt.ElapsedGameTime.TotalMilliseconds;

			if (this.elapsedTime >= Fps)
			{
				this.elapsedTime = 0f;

				IncrementFrame();
			}
		}

		public Rectangle GetSourceRectangle()
		{
			return new Rectangle(AnimationArea.X + (this.currentFrame * FrameWidth), AnimationArea.Y, FrameWidth, FrameHeight);
		}

		private void IncrementFrame()
		{
			this.currentFrame++;

			if (this.currentFrame >= FrameCount)
			{
				this.currentFrame = 0;
			}
		}
	}
}