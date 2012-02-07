namespace Pokemon_RPG.Tiles
{
	using Microsoft.Xna.Framework;

	class ShallowWaterTile : Tile
	{
		public ShallowWaterTile(Color drawColor)
			: base(TileId.ShallowWater, TileType.NonSolid, new Rectangle(32, 3000, 16, 16), drawColor)
		{
		}
	}
}