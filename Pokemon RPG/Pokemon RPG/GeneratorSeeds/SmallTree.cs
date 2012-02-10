using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokemon_RPG.GeneratorSeeds
{
	using Microsoft.Xna.Framework;

	using Pokemon_RPG.Tiles;

	class SmallTree : Seed
	{
		public SmallTree(int x, int y, ref TileSystem tileSystem)
			: base(x, y, ref tileSystem)
		{
		}

		public override void Grow()
		{
			TileSystem.Tiles[1][X, Y] = new Tile(
				Tile.TileId.Tree,
				Tile.Solidity.Solid,
				SourceRects.Tree1x3Bottom,
				Color.White,
				Tile.TileSide.Bottom);

			TileSystem.Tiles[1][X, Y - 1] = new Tile(
				Tile.TileId.Tree,
				Tile.Solidity.Solid,
				SourceRects.Tree1x3Middle,
				Color.White,
				Tile.TileSide.Middle);

			TileSystem.Tiles[1][X, Y - 2] = new Tile(
				Tile.TileId.Tree,
				Tile.Solidity.Solid,
				SourceRects.Tree1x3Top,
				Color.White,
				Tile.TileSide.Top);
		}
	}
}