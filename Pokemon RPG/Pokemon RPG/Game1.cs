namespace Pokemon_RPG
{
	using System;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;

	using Pokemon_RPG.States;
	using Pokemon_RPG.Tiles;
	using Pokemon_RPG.lua;

	using Utilities;

	using XNAGui;

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		#region Constants and Fields

		private readonly GraphicsDeviceManager graphics;

		private SpriteBatch spriteBatch;

		#endregion

		#region Constructors and Destructors

		public Game1()
		{
			this.graphics = new GraphicsDeviceManager(this);
			this.Content.RootDirectory = "Content";
		}

		#endregion

		#region Methods

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			this.GraphicsDevice.Clear(Color.Black);

			StateManager.Draw(gameTime);
			this.GraphicsDevice.SetRenderTarget(null); // Drawing a state will set the render target to its own,
			// so make sure to reset it to default when the state has been drawn.

			this.spriteBatch.Begin();
			this.spriteBatch.Draw(StateManager.CurrentState.RenderTarget, Vector2.Zero, Color.White);
			this.spriteBatch.End();

			base.Draw(gameTime);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			Console.Title = "Pokemon RPG";

			this.IsMouseVisible = true;
			UtilityManager.Initialize(this);
			XnaGui.Initialize(this);
			LuaManager.Initialize();
			StateManager.Initialize();
			GuiManager.LoadGuis();

			StateManager.AddState("intro", new Intro(this.GraphicsDevice));
			StateManager.AddState("mainMenu", new MainMenu(this.GraphicsDevice));
			StateManager.AddState("worldEditor", new WorldEditor(this.GraphicsDevice));
			StateManager.SetState("intro", true);
			StateManager.CurrentState.StateFinished += (sender, args) => StateManager.SetState("mainMenu", true); // CurrentState is in this case the intro state.

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			this.Window.ClientSizeChanged += this.WindowClientSizeChanged;
			this.Window.AllowUserResizing = true;

			this.graphics.PreferMultiSampling = true;
			//this.graphics.PreferredBackBufferWidth = 1280;
			//this.graphics.PreferredBackBufferHeight = 720;
			this.graphics.ApplyChanges();
			this.WindowClientSizeChanged(this, new EventArgs());

			// Create a new SpriteBatch, which can be used to draw textures.
			this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

			Tile.TextureAtlas = ResourceHelper.LoadTexture("img\\tiles\\world_tileset");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				this.Exit();
			}

			UtilityManager.Update(this.IsActive, gameTime);
			XnaGui.Update(this.IsActive, gameTime);

			StateManager.Update(gameTime);

			base.Update(gameTime);
		}

		private void WindowClientSizeChanged(object sender, EventArgs e)
		{
			Console.WriteLine("Window size changed to: " + Helper.GetWindowSize());
			StateManager.ResetRenderTargets(this.GraphicsDevice);
		}

		#endregion
	}
}