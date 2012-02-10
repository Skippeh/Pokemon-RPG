using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokemon_RPG.GeneratorSeeds
{
	public class Seed
	{
		/// <summary>
		/// The tile system that this seed will grow inside.
		/// </summary>
		protected TileSystem TileSystem;

		/// <summary>
		/// The x coordinate in the tile system.
		/// </summary>
		protected int X;

		/// <summary>
		/// The y coordinate in the tile system.
		/// </summary>
		protected int Y;

		public Seed(int x, int y, ref TileSystem tileSystem)
		{
			this.TileSystem = tileSystem;

			X = x;
			Y = y;
		}

		/// <summary>
		/// Grow the seed (Place tiles at XY in the tile system).
		/// </summary>
		public virtual void Grow()
		{
			
		}
	}
}