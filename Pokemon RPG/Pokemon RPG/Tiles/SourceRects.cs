using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokemon_RPG.Tiles
{
	using Microsoft.Xna.Framework;

	using Utilities;

	static class SourceRects
	{
		static public Rectangle Grass
		{
			get
			{
				double randVal = Helper.Rand.NextDouble();

				if (randVal <= 0.25)
				{
					return new Rectangle(0, 0, 16, 16);
				}
				
				if (randVal <= 0.50)
				{
					return new Rectangle(16, 0, 16, 16);
				}
				
				if (randVal <= 0.75)
				{
					return new Rectangle(32, 0, 16, 16);
				}
				
				return new Rectangle(48, 0, 16, 16);
			}
		}

		static public Rectangle Gravel = new Rectangle(160, 1240, 16, 16);

		static public Rectangle SandMiddle = new Rectangle(16, 48, 16, 16);
		static public Rectangle SandRight = new Rectangle(32, 48, 16, 16);
		static public Rectangle SandTop = new Rectangle(16, 32, 16, 16);
		static public Rectangle SandBottom = new Rectangle(16, 64, 16, 16);
		static public Rectangle SandLeft = new Rectangle(0, 48, 16, 16);
		static public Rectangle SandTopLeft = new Rectangle(64, 176, 16, 16);
		static public Rectangle SandTopRight = new Rectangle(48, 176, 16, 16);
		static public Rectangle SandBottomRight = new Rectangle(48, 160, 16, 16);
		static public Rectangle SandBottomLeft = new Rectangle(64, 160, 16, 16);

		static public Rectangle ShallowWater = new Rectangle(80, 2968, 16, 16);
		static public Rectangle Water = new Rectangle(16, 528, 16, 16);
		static public Rectangle TallGrass = new Rectangle(0, 16, 16, 16);
	}
}