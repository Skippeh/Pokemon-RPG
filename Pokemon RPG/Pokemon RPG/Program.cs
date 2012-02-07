namespace Pokemon_RPG
{
	using System.Collections.Generic;

#if WINDOWS || XBOX
	static class Program
	{
		public static Game1 Game { get; private set; }

		public static List<string> Arguments { get; private set; }

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			Arguments = new List<string>();
			foreach(string arg in args) Arguments.Add(arg.ToLower());

			Game = new Game1();

			Game.Run();
		}
	}
#endif
}