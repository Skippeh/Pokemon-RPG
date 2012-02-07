using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokemon_RPG.lua.Methods
{
	using LuaInterface;

	using Microsoft.Xna.Framework.Graphics;

	using Pokemon_RPG.States;

	class GameMethods
	{
		public void Register(Lua lua)
		{
			lua.RegisterFunction("game.exit", this, this.GetType().GetMethod("Exit"));
			lua.RegisterFunction("game.setState", this, this.GetType().GetMethod("SetState"));
			lua.RegisterFunction("game.addState", this, this.GetType().GetMethod("AddState"));
			lua.RegisterFunction("game.getXnaGame", this, this.GetType().GetMethod("GetXnaGame"));
			lua.RegisterFunction("game.getGraphicsDevice", this, this.GetType().GetMethod("GetGraphicsDevice"));
			lua.RegisterFunction("game.startGame", this, GetType().GetMethod("StartGame"));
		}

		public void Exit()
		{
			Program.Game.Exit();
		}

		public void SetState(string identifier, bool resetTime = true)
		{
			StateManager.SetState(identifier, resetTime);
		}

		public void AddState(string identifier, State state)
		{
			StateManager.AddState(identifier, state);
		}

		public Game1 GetXnaGame()
		{
			return Program.Game;
		}

		public GraphicsDevice GetGraphicsDevice()
		{
			return Program.Game.GraphicsDevice;
		}

		public void StartGame()
		{
			StateManager.AddState("inGame", new InGame(Program.Game.GraphicsDevice));
			StateManager.SetState("inGame", true);
		}
	}
}