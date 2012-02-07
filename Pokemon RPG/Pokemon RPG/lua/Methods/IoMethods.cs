using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokemon_RPG.lua.Methods
{
	using LuaInterface;

	using Microsoft.Xna.Framework.Graphics;

	using Utilities;

	class IoMethods
	{
		public void Register(Lua lua)
		{
			lua.RegisterFunction("io.loadFont", this, this.GetType().GetMethod("LoadFont"));
			lua.RegisterFunction("io.loadTexture", this, this.GetType().GetMethod("LoadTexture"));
		}

		public SpriteFont LoadFont(string fontName)
		{
			return ResourceHelper.LoadFont(fontName);
		}

		public Texture2D LoadTexture(string imageName)
		{
			return ResourceHelper.LoadTexture(imageName);
		}
	}
}