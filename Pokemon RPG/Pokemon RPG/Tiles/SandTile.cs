using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokemon_RPG.Tiles
{
	using Microsoft.Xna.Framework;

	class SandTile : Tile
	{
		public SandTile(Color drawColor, TileSide tileSide = TileSide.Middle)
			: base(TileId.Sand, TileType.NonSolid, Rectangle.Empty, drawColor)
		{
			this.Side = tileSide;

			switch(Side)
			{
				case TileSide.Middle:
					SourceRectangle = new Rectangle(16, 48, 16, 16);
					break;
				case TileSide.Right:
					SourceRectangle = new Rectangle(32, 48, 16, 16);
					break;
				case TileSide.Top:
					SourceRectangle = new Rectangle(16, 32, 16, 16);
					break;
				case TileSide.Bottom:
					SourceRectangle = new Rectangle(16, 64, 16, 16);
					break;
				case TileSide.Left:
					SourceRectangle = new Rectangle(0, 48, 16, 16);
					break;
				case TileSide.TopLeft:
					SourceRectangle = new Rectangle(64, 176, 16, 16);
					break;
				case TileSide.TopRight:
					SourceRectangle = new Rectangle(48, 176, 16, 16);
					break;
				case TileSide.BottomRight:
					SourceRectangle = new Rectangle(48, 160, 16, 16);
					break;
				case TileSide.BottomLeft:
					SourceRectangle = new Rectangle(64, 160, 16, 16);
					break;
			}
		}
	}
}