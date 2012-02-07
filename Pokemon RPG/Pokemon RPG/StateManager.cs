using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokemon_RPG
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	using Pokemon_RPG.States;

	using Utilities;

	static class StateManager
	{
		public static State CurrentState
		{
			get
			{
				return stateDict[currentStateIdentifier];
			}
		}

		private static string currentStateIdentifier;

		private static Dictionary<string, State> stateDict;

		public static void Initialize()
		{
			stateDict = new Dictionary<string, State>();
		}

		public static void Update(GameTime gameTime)
		{
			CurrentState.Update(gameTime);
		}

		public static void Draw(GameTime gameTime)
		{
			CurrentState.Draw(gameTime);
		}

		public static void AddState(string stateIdentifier, State state)
		{
			stateDict[stateIdentifier] = state;
		}

		public static void SetState(string stateIdentifier, bool resetStateTime)
		{
			if(currentStateIdentifier != null)
				CurrentState.OnUnFocus();

			currentStateIdentifier = stateIdentifier;
			CurrentState.OnFocus();
			CurrentState.HasFocusedOnce = true;

			if(resetStateTime) CurrentState.ResetTime();
		}

		public static void ResetRenderTargets(GraphicsDevice graphicsDevice)
		{
			foreach (var t in stateDict.Values)
			{
				t.ResetRenderTarget(graphicsDevice);
			}
		}
	}
}