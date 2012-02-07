using System;
using LuaInterface;
using Pokemon_RPG.lua.Methods;

namespace Pokemon_RPG.lua
{
	public static class LuaManager
	{
		public static Lua LuaVm { get; private set; }

		private static BaseMethods baseMethods;

		private static GuiMethods guiMethods;

		private static GameMethods gameMethods;

		private static IoMethods ioMethods;

		public static void Initialize()
		{
			LuaVm = new Lua();

			RegisterMethods();

			GuiManager.Initialize();
			ScriptManager.CreateFolders();
		}

		private static void RegisterMethods()
		{
			baseMethods = new BaseMethods();
			guiMethods = new GuiMethods();
			gameMethods = new GameMethods();
			ioMethods = new IoMethods();

			LuaVm.NewTable("game");
			LuaVm.NewTable("gui");
			LuaVm.NewTable("io");
			LuaVm.NewTable("states");
			baseMethods.Register(LuaVm);
			guiMethods.Register(LuaVm);
			gameMethods.Register(LuaVm);
			ioMethods.Register(LuaVm);
		}
	}
}