using System.IO;

namespace Pokemon_RPG
{
	public static class ScriptManager
	{
		static public string FilePath = Directory.GetCurrentDirectory() + "\\Scripts";

		public static void CreateFolders()
		{
			if (Directory.Exists(FilePath)) return;

			Directory.CreateDirectory(FilePath);

			GuiManager.CreateFolders();
		}
	}
}