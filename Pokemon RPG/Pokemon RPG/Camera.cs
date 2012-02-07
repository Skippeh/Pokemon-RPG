using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokemon_RPG
{
	using Microsoft.Xna.Framework;

	using Utilities;

	public class Camera
	{
		public Vector2 Position { get; set; }

		private Vector2 targetPosition;

		public float Zoom { get; set; }

		public float Rotation { get; set; }

		public Camera()
		{
			Position = Vector2.Zero;
			Zoom = 1.5f;
			Rotation = 0f;
		}

		public Matrix GetMatrix()
		{
			return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) * Matrix.CreateRotationZ(Rotation)
			       * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1))
			       * Matrix.CreateTranslation(new Vector3(Helper.GetWindowSize().X * 0.5f, Helper.GetWindowSize().Y * 0.5f, 0));
		}

		public Rectangle GetRectangle()
		{
			return new Rectangle(
				(int)((Position.X - Helper.GetWindowSize().X / 2f) / Zoom),
				(int)((Position.Y - Helper.GetWindowSize().Y / 2f) / Zoom),
				(int)((Position.X + Helper.GetWindowSize().X / 2f) / Zoom),
				(int)((Position.Y + Helper.GetWindowSize().Y / 2f) / Zoom));
		}

		public void Move(Vector2 add)
		{
			Position += add;
		}

		public Vector2 GetPosition()
		{
			return Position * Zoom;
		}
	}
}