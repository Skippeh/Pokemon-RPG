using System;

namespace Pokemon_RPG.States
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	using Utilities;

	public class State
	{
		/// <summary>
		/// The render target this state draws to.
		/// </summary>
		public RenderTarget2D RenderTarget { get; protected set; }

		/// <summary>
		/// The sprite batch this state draws to.
		/// </summary>
		public SpriteBatch SpriteBatch { get; protected set; }

		public bool HasFocusedOnce { get; set; }

		/// <summary>
		/// Elapsed time since this state was initialized.
		/// </summary>
		public TimeSpan ElapsedTime
		{
			get
			{
				return DateTime.Now - StartTime;
			}
		}

		/// <summary>
		/// Indicates whether the state has finished.
		/// </summary>
		public bool Finished { get; protected set; }

		/// <summary>
		/// The time this state was initialized, or reset.
		/// </summary>
		public DateTime StartTime;

		/// <summary>
		/// The XNA game's graphics device.
		/// </summary>
		protected GraphicsDevice GraphicsDevice;

		/// <summary>
		/// Makes sure we can only invoke the StateFinished event once.
		/// </summary>
		private bool invokedFinishedEvent;

		// Don't allow initializing empty states, unless deriving the class.
		protected State()
		{
		}

		/// <summary>
		/// Initializes a new state.
		/// </summary>
		/// <param name="gd">The XNA game's graphics device.</param>
		protected State(GraphicsDevice gd)
		{
			this.GraphicsDevice = gd;
			SpriteBatch = new SpriteBatch(this.GraphicsDevice);
			RenderTarget = new RenderTarget2D(GraphicsDevice, (int)Helper.GetWindowSize().X, (int)Helper.GetWindowSize().Y);

			StartTime = DateTime.Now;

			HasFocusedOnce = false;
		}

		/// <summary>
		/// Resets the state. This will also set the StartTime to the current time.
		/// </summary>
		public virtual void ResetTime()
		{
			StartTime = DateTime.Now;
		}

		/// <summary>
		/// Updates the state.
		/// </summary>
		/// <param name="gt"></param>
		public virtual void Update(GameTime gt)
		{
			if (Finished)
			{
				this.OnFinished();
			}
		}

		/// <summary>
		/// Draws the state. Place this first in your draw code in a derived state.
		/// </summary>
		/// <param name="gt">The current <c>GameTime</c>.</param>
		public virtual void Draw(GameTime gt)
		{
			GraphicsDevice.SetRenderTarget(RenderTarget);
		}

		public event EventHandler StateFinished;

		protected void OnFinished()
		{
			if (invokedFinishedEvent) return;

			if(StateFinished != null)
			{
				StateFinished(this, new EventArgs());
				invokedFinishedEvent = true;
			}
		}

		public virtual void ResetRenderTarget(GraphicsDevice graphicsDevice)
		{
			RenderTarget = new RenderTarget2D(graphicsDevice, (int)Helper.GetWindowSize().X, (int)Helper.GetWindowSize().Y);
		}

		/// <summary>
		/// Called when this state is switched to.
		/// </summary>
		public virtual void OnFocus()
		{
		}

		/// <summary>
		/// Called when this state is switched from.
		/// </summary>
		public virtual void OnUnFocus()
		{
			
		}
	}
}