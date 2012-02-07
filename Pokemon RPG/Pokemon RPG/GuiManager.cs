using System;
using System.Collections.Generic;

namespace Pokemon_RPG
{
	using System.Globalization;
	using System.IO;

	using Pokemon_RPG.lua;

	using Utilities;

	using XNAGui;

	public static class GuiManager
	{
		public static Dictionary<string, Menu> LoadedMenus;

		public static readonly string FilePath = ScriptManager.FilePath + "\\Gui";

		public static void Initialize()
		{
			LoadedMenus = new Dictionary<string, Menu>();
		}

		public static void CreateFolders()
		{
			if (Directory.Exists(FilePath)) return;

			Directory.CreateDirectory(FilePath);
		}

		public static void LoadGuis()
		{
			LoadedMenus.Clear();

			try
			{
				Console.WriteLine("Loading gui's...");

				foreach (string fileName in Directory.GetFiles(FilePath))
				{
					if (!fileName.EndsWith(".lua", true, CultureInfo.InvariantCulture)) continue; // Make sure to only run lua files.

					Console.WriteLine(
						"Loading file: " + fileName.Substring(fileName.LastIndexOf("\\", StringComparison.InvariantCultureIgnoreCase) + 1));

					LuaManager.LuaVm.DoString(IoHelper.GetFileContents(fileName));
				}

				Console.WriteLine("Gui's loaded.");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine("Failed to load gui's.");
				Console.ReadKey(true);
				Environment.Exit(1);
			}
		}
	}
}