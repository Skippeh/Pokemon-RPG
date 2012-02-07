using System;
using LuaInterface;

namespace Pokemon_RPG.lua.Methods
{
	public class BaseMethods
	{
		public void Register(Lua lua)
		{
			lua.RegisterFunction("write", this, this.GetType().GetMethod("Write"));
			lua.RegisterFunction("writeLine", this, this.GetType().GetMethod("WriteLine"));
		}

		public void Write(string text)
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write(text);
			Console.ResetColor();
		}

		public void WriteLine(string text)
		{
			Write(text + "\n");
		}
	}
}