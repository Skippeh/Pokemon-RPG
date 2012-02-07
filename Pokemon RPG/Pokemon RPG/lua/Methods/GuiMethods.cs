using LuaInterface;

namespace Pokemon_RPG.lua.Methods
{
	using Microsoft.Xna.Framework;

	using Utilities;
	using XNAGui;

	class GuiMethods
	{
		public void Register(Lua lua)
		{
			lua.RegisterFunction("game.getWidth", this, this.GetType().GetMethod("GetWindowWidth"));
			lua.RegisterFunction("game.getHeight", this, this.GetType().GetMethod("GetWindowHeight"));
			lua.RegisterFunction("game.getCenter", this, this.GetType().GetMethod("GetWindowCenter"));
			lua.RegisterFunction("gui.registerMenu", this, this.GetType().GetMethod("RegisterMenu"));
		}

		public int GetWindowWidth()
		{
			return (int)Helper.GetWindowSize().X;
		}

		public int GetWindowHeight()
		{
			return (int)Helper.GetWindowSize().Y;
		}

		public Vector2 GetWindowCenter(Vector2 size)
		{
			return Helper.GetWindowCenter(size);
		}

		public Menu RegisterMenu(string identifier)
		{
			GuiManager.LoadedMenus[identifier] = new Menu();

			return GuiManager.LoadedMenus[identifier];
		}
	}
}