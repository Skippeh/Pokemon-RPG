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
		static public Rectangle SandToGrassRight = new Rectangle(128, 2984, 16, 16);
		static public Rectangle SandToGrassTop = new Rectangle(112, 2968, 16, 16);
		static public Rectangle SandToGrassBottom = new Rectangle(112, 3000, 16, 16);
		static public Rectangle SandToGrassLeft = new Rectangle(96, 2984, 16, 16);

		static public Rectangle SandToGrassTopLeft = new Rectangle(96, 2968, 16, 16);
		static public Rectangle SandToGrassTopRight = new Rectangle(128, 2968, 16, 16);
		static public Rectangle SandToGrassBottomLeft = new Rectangle(96, 3000, 16, 16);
		static public Rectangle SandToGrassBottomRight = new Rectangle(128, 3000, 16, 16);
		static public Rectangle SandToGrassUpperHalf = new Rectangle(80, 2984, 16, 16);
		static public Rectangle SandToGrassLowerHalf = new Rectangle(80, 3000, 16, 16);
		static public Rectangle SandToGrassCircle = new Rectangle(64, 3000, 16, 16);
		static public Rectangle SandToGrassInnerTopLeft;
		static public Rectangle SandToGrassInnerTopRight;
		static public Rectangle SandToGrassInnerBottomLeft;
		static public Rectangle SandToGrassInnerBottomRight;

		public static Rectangle SandToShallowTopLeft = new Rectangle(144, 2968, 16, 16);
		public static Rectangle SandToShallowTop = new Rectangle(160, 2968, 16, 16);
		public static Rectangle SandToShallowTopRight = new Rectangle(176, 2968, 16, 16);
		public static Rectangle SandToShallowLeft = new Rectangle(144, 2984, 16, 16);
		public static Rectangle SandToShallowRight = new Rectangle(176, 2984, 16, 16);
		public static Rectangle SandToShallowBottomLeft = new Rectangle(144, 3000, 16, 16);
		public static Rectangle SandToShallowBottom = new Rectangle(160, 3000, 16, 16);
		public static Rectangle SandToShallowBottomRight = new Rectangle(176, 3000, 16, 16);
		public static Rectangle SandToShallowUpperHalf = new Rectangle(64, 2984, 16, 16);
		public static Rectangle SandToShallowLowerHalf = new Rectangle(64, 3000, 16, 16);
		public static Rectangle SandToShallowCircle = new Rectangle(32, 3000, 16, 16);

		static public Rectangle ShallowWaterMiddle = new Rectangle(80, 2968, 16, 16);
		public static Rectangle ShallowtoWaterTopLeft = new Rectangle(192, 2968, 16, 16);
		public static Rectangle ShallowToWaterTop = new Rectangle(208, 2968, 16, 16);
		public static Rectangle ShallowToWaterTopRight = new Rectangle(224, 2968, 16, 16);
		public static Rectangle ShallowToWaterLeft = new Rectangle(192, 2984, 16, 16);
		public static Rectangle ShallowToWaterRight = new Rectangle(224, 2984, 16, 16);
		public static Rectangle ShallowToWaterBottomLeft = new Rectangle(192, 3000, 16, 16);
		public static Rectangle ShallowToWaterBottom = new Rectangle(208, 3000, 16, 16);
		public static Rectangle ShallowToWaterBottomRight = new Rectangle(224, 3000, 16, 16);

		static public Rectangle Water = new Rectangle(16, 528, 16, 16);
		static public Rectangle TallGrass = new Rectangle(0, 16, 16, 16);
	}
}