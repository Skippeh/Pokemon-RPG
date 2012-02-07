using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokemon_RPG.Tiles
{
	using Microsoft.Xna.Framework;

	class GravelTile : Tile
	{
		public GravelTile(Color drawColor) : base(TileId.Gravel, TileType.NonSolid, Rectangle.Empty, drawColor)
		{
			SourceRectangle = new Rectangle(160, 1240, 16, 16);
		}
	}
}