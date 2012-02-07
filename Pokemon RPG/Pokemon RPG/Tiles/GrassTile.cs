namespace Pokemon_RPG.Tiles
{
	using Microsoft.Xna.Framework;

	using Utilities;

	public class GrassTile : Tile
	{
		public GrassTile(Color drawColor)
			: base(TileId.Grass, TileType.NonSolid, Rectangle.Empty, drawColor)
		{
			double randVal = Helper.Rand.NextDouble();

			if (randVal <= 0.25)
			{
				SourceRectangle = new Rectangle(0, 0, 16, 16);
			}
			else if (randVal <= 0.50)
			{
				SourceRectangle = new Rectangle(16, 0, 16, 16);
			}
			else if (randVal <= 0.75)
			{
				SourceRectangle = new Rectangle(32, 0, 16, 16);
			}
			else if (randVal <= 1.00)
			{
				SourceRectangle = new Rectangle(48, 0, 16, 16);
			}
		}
	}
}