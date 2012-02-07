namespace Pokemon_RPG.Tiles
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	using Utilities;

	public class WaterTile : Tile
	{
		public WaterTile(Color drawColor)
			: base(TileId.Water, TileType.NonSolid, Rectangle.Empty, drawColor)
		{
			this.SourceRectangle = new Rectangle(16, 528, 16, 16);
		}
	}
}