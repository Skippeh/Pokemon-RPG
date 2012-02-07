namespace Pokemon_RPG.Tiles
{
	using Microsoft.Xna.Framework;

	class ShallowWaterTile : Tile
	{
		public ShallowWaterTile(Color drawColor)
			: base(TileId.ShallowWater, TileType.NonSolid, new Rectangle(80, 2968, 16, 16), drawColor)
		{
		}
	}
}