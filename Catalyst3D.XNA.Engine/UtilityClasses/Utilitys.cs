using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Plane = Microsoft.Xna.Framework.Plane;

namespace Catalyst3D.XNA.Engine.UtilityClasses
{
	public static class Utilitys
	{
		private static readonly Random rand = new Random();

		public static Color GetRandomColor()
		{
			int index = rand.Next(1, 10);

			switch (index)
			{
				case 1:
					return Color.Blue;
				case 2:
					return Color.Green;
				case 3:
					return Color.Yellow;
				case 4:
					return Color.Purple;
				case 5:
					return Color.Red;
				case 6:
					return Color.Orange;
				case 7:
					return Color.Pink;
				case 8:
					return Color.Tan;
				case 9:
					return Color.Indigo;
				case 10:
					return Color.Aqua;

				default:
					return Color.Pink;
			}
		}

		public static float GetRandomFloat(float fMin, float fMax)
		{
			return (float)rand.NextDouble() * (fMax - fMin) + fMin;
		}

		public static double GetRandomDouble(double dMin, double dMax)
		{
			return rand.NextDouble() * (dMax - dMin) + dMin;
		}

		public static Vector2 GetRandomVector2(float xMin, float xMax, float yMin, float yMax)
		{
			return new Vector2(GetRandomFloat(xMin, xMax), GetRandomFloat(yMin, yMax));
		}

		public static int GetRandomInt(int iMin, int iMax)
		{
			return rand.Next(iMin, iMax);
		}


		//public static void SaveScreenShot(string filename, Game game)
		//{

		//  try
		//  {
		//    if (game.GraphicsDevice != null)
		//    {

		//      int index = 0;
		//      while (File.Exists(game.Content.RootDirectory + @"\" + filename + index + ".jpg"))
		//        index++;

		//      ResolveTexture2D dest = new ResolveTexture2D(game.GraphicsDevice, game.GraphicsDevice.Viewport.Width,
		//                                                   game.GraphicsDevice.Viewport.Height, 1, SurfaceFormat.Color);
		//      game.GraphicsDevice.ResolveBackBuffer(dest);
		//      dest.Save(game.Content.RootDirectory + @"\" + filename + index + ".jpg", ImageFileFormat.Jpg);
		//      dest.Dispose();
		//    }
		//  }
		//  catch (Exception er)
		//  {
		//    throw new Exception(er.Message);
		//  }

		//}

		public static Plane CreatePlane(float height, Vector3 normalDirection, Matrix view, Matrix projection, bool isClipSide)
		{
			// Make sure our normals are in unity lengths
			normalDirection.Normalize();

			// Setup our coefficeients
			Vector4 coefficeients = new Vector4(normalDirection, height);

			if (isClipSide)
				coefficeients *= -1;

			Matrix wvp = view * projection;
			Matrix inverseWvp = Matrix.Invert(wvp);
			inverseWvp = Matrix.Transpose(inverseWvp);

			coefficeients = Vector4.Transform(coefficeients, inverseWvp);

			return new Plane(coefficeients);
		}

		public static Vector3 CalcPosOnPlane(GraphicsDevice Device, BasicCamera Camera, float X, float Y)
		{
			Vector3 nearsource = new Vector3(X, Y, 0f);
			Vector3 farsource = new Vector3(X, Y, 1f);

			Vector3 nearPoint = Device.Viewport.Unproject(nearsource, Camera.Projection, Camera.View, Matrix.Identity);
			Vector3 farPoint = Device.Viewport.Unproject(farsource, Camera.Projection, Camera.View, Matrix.Identity);

			Vector3 direction = farPoint - nearPoint;

			// Normalize our result
			direction.Normalize();

			Ray ray = new Ray(nearPoint, direction);

			float? result = ray.Intersects(new Plane(Vector3.Up, 0));

			if (result != null)
			{
				Vector3 temp = direction * result.Value;

				Vector3 final = Camera.Position + temp;

				return final;
			}

			return Vector3.Zero;
		}

		public static Ray CalculateRayFromCursor(GraphicsDevice device, BasicCamera cam, float x, float y)
		{
			// Setup near and far vectors
			Vector3 near = new Vector3(x, y, device.Viewport.MinDepth);
			Vector3 far = new Vector3(x, y, device.Viewport.MaxDepth);

			// Get the near point
			Vector3 nearPoint = device.Viewport.Unproject(near, cam.Projection, cam.View, Matrix.Identity);

			// Get the far point
			Vector3 farPoint = device.Viewport.Unproject(far, cam.Projection, cam.View, Matrix.Identity);

			// find the direction vector that goes from the near to the far points and normalize that
			Vector3 dir = farPoint - nearPoint;
			dir.Normalize();

			// and then create a new ray using the nearest point as the source
			return new Ray(nearPoint, dir);
		}

		public static Ray CalculateRayFromCursor(GraphicsDevice device, BasicCamera cam, Matrix world, float x, float y)
		{
			// Setup near and far vectors
			Vector3 near = new Vector3(x, y, 0);
			Vector3 far = new Vector3(x, y, 1);

			// Get the near point
			Vector3 nearPoint = device.Viewport.Unproject(near, cam.Projection, cam.View, world);

			// Get the far point
			Vector3 farPoint = device.Viewport.Unproject(far, cam.Projection, cam.View, world);

			// find the direction vector that goes from the near to the far points and normalize that
			Vector3 dir = farPoint - nearPoint;
			dir.Normalize();

			// and then create a new ray using the nearest point as the source
			return new Ray(nearPoint, dir);
		}

		// http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=1144568&SiteID=1
		public static bool Intersects(Ray ray, Vector3 a, Vector3 b, Vector3 c, Vector3 normal, bool positiveSide, bool negativeSide, out float t)
		{
			t = 0;
			{
				float denom = Vector3.Dot(normal, ray.Direction);

				if (denom > float.Epsilon)
				{
					if (!negativeSide)
						return false;
				}
				else if (denom < -float.Epsilon)
				{
					if (!positiveSide)
						return false;
				}
				else
				{
					return false;
				}

				t = Vector3.Dot(normal, a - ray.Position) / denom;

				if (t < 0)
				{
					// Interersection is behind origin
					return false;
				}
			}

			// Calculate the largest area projection plane in X, Y or Z.
			int i0, i1;
			{
				float n0 = Math.Abs(normal.X);
				float n1 = Math.Abs(normal.Y);
				float n2 = Math.Abs(normal.Z);

				i0 = 1;
				i1 = 2;

				if (n1 > n2)
				{
					if (n1 > n0)
						i0 = 0;
				}
				else
				{
					if (n2 > n0)
						i1 = 0;
				}
			}

			float[] A = { a.X, a.Y, a.Z };
			float[] B = { b.X, b.Y, b.Z };
			float[] C = { c.X, c.Y, c.Z };
			float[] R = { ray.Direction.X, ray.Direction.Y, ray.Direction.Z };
			float[] RO = { ray.Position.X, ray.Position.Y, ray.Position.Z };

			// Check the intersection point is inside the triangle.
			{
				float u1 = B[i0] - A[i0];
				float v1 = B[i1] - A[i1];
				float u2 = C[i0] - A[i0];
				float v2 = C[i1] - A[i1];
				float u0 = t * R[i0] + RO[i0] - A[i0];
				float v0 = t * R[i1] + RO[i1] - A[i1];

				float alpha = u0 * v2 - u2 * v0;
				float beta = u1 * v0 - u0 * v1;
				float area = u1 * v2 - u2 * v1;

				const float EPSILON = 1e-3f;

				float tolerance = EPSILON * area;

				if (area > 0)
				{
					if (alpha < tolerance || beta < tolerance || alpha + beta > area - tolerance)
						return false;
				}
				else
				{
					if (alpha > tolerance || beta > tolerance || alpha + beta < area - tolerance)
						return false;
				}
			}

			return true;
		}

		// Fast Computation of Terrain Shadow Maps
		// http://www.gamedev.net/reference/articles/article1817.asp
		/*
		private static int Intersects(Vector3 pos, Ray ray)
		{
			int w, hits;
			float d, h, D;
			Vector3 v, dir;

			v = pos + ray.Direction;
			w = size.X;

			hits = 0;

			for (int x = 0; x < size.X - 1; x++)
			{
				for (int y = 0; y < size.Y - 1; y++)
				{
					D = Vector3.Distance(new Vector3(v.X, 0, v.Z), new Vector3(ray.Position.X, 0, ray.Position.Z));
					d = Vector3.Distance(pos, v);            // light direction
					h = pos.Y + (d * ray.Position.Y) / D;  // X(P) point

					// check if height in point P is bigger than point X's height
					float pixelHeight = bits[x + y * size.X].ToVector4().X;
					if (pixelHeight > h)
					{
						hits++;   // if so, mark as hit, and skip this work point.
						//z = (h - pixelHeight) / Vector3.Distance(new Vector3(x * size.X, pixelHeight, z * size.Y), ray.Position);
						break;
					};

					dir = ray.Direction;
					dir.Y = 0;
					dir.Normalize();
					v += dir;   // fetch new working point
				}
			}

			while (!((v.X >= w - 1) || (v.X <= 0) || (v.Z >= w - 1) || (v.Z <= 0)))
			{
					// length of lightdir's projection
					D = Vector3.Distance(new Vector3(v.X, 0, v.Z), new Vector3(ray.Position.X, 0, ray.Position.Z));
					d = Vector3.Distance(pos, v);            // light direction
					h = pos.Y + (d * ray.Position.Y) / D;  // X(P) point

					// check if height in point P is bigger than point X's height
					if (bits[(int)Math.Floor(v.Z) * w + (int)Math.Floor(v.X)].ToVector3().X * maxHeight > h)
					{
							hits++;   // if so, mark as hit, and skip this work point.
							break;
					};

					dir = ray.Direction;
					dir.Y = 0;
					v += dir.Normalize();   // fetch new working point
			};
		 
			return hits;
		}
		*/

		/// <summary>
		/// Perlin Noise Generation
		/// http://www.gamedev.net/reference/articles/article2085.asp
		/// </summary>
		public static float PerlinNoise(int x, int y, int maxValue)
		{
			Random r = new Random();

			int n = x + y * 57 + r.Next(0, maxValue) * 131;
			n = (n << 13) ^ n;

			return (1.0f - ((n * (n * n * 0x3d73 + 0xc0ae5) + 0x5208dd0d) & 0x7fffffff) * 0.000000000931322574615478515625f);
		}

		public static Vector2 GetCenterScreen(Viewport viewport)
		{
			// Center the text in the viewport.
		  return new Vector2((float) viewport.Width/2, (float) viewport.Height/2);
		}

		public static Vector3 GetRandomVolume(float particleSize, Vector3 position)
		{
			//get value between 0 and radius
			float r = (float)(rand.NextDouble() * particleSize / 2);

			//get value between 0 and 2Pi (horizontal angle)
			float theta = (float)(rand.NextDouble() * (float)Math.PI * 2);
			float phi = (float)(rand.NextDouble() * (float)Math.PI - (float)Math.PI / 2f);

			//get value between -Pi/2 and Pi/2 (vertical angle)
			float dx = (float)(r * Math.Cos(theta) * Math.Cos(phi));
			float dy = (float)(r * Math.Sin(theta) * Math.Cos(phi));
			float dz = (float)(r * Math.Sin(phi));

			return new Vector3(dx + position.X, dy + position.Y, dz + position.Z);
		}

		public static float RandomBetween(float min, float max)
		{
			return min + (float)rand.NextDouble() * (max - min);
		}

		public static Vector2 PickRandomDirection()
		{
			float angle = RandomBetween(0, MathHelper.TwoPi);
			return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		}

		public static Texture2D GenerateTexture(GraphicsDevice device, Color color, int width, int height)
		{
			Color[] data = new Color[width * height];

			// 1 pixels wide
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					data[x + y * width] = color;
				}
			}

			// Set the texture up with this pixel data
			Texture2D texture = new Texture2D(device, width, height);
			texture.SetData(data);

			return texture;
		}

		public static Texture2D TextureFromFile(GraphicsDevice device, string path)
		{
			if (device != null)
			{
				using (Stream stream = File.OpenRead(path))
				{
					return Texture2D.FromStream(device, stream);
				}
			}
			return null;
		}

		public static Color Lerp(Color c1, Color c2, float f)
		{
			byte a = (byte)(c1.A + (c2.A - c1.A) * f);
			byte r = (byte)(c1.R + (c2.R - c1.R) * f);
			byte g = (byte)(c1.G + (c2.G - c1.G) * f);
			byte b = (byte)(c1.B + (c2.B - c1.B) * f);
			return new Color(r, g, b, a);
		}

		//public static int CompareQuadTreeNodesByDistance(QuadTreeNode x, QuadTreeNode y)
		//{
		//  int retval = x.distanceFromCamera.CompareTo(y.distanceFromCamera);
		//  if(retval < 0)
		//    return -1;
		//  if(retval > 0)
		//    return 1;

		//  return 0;
		//}

		public static Matrix CreateDecalViewProjectionMatrix(BasicCamera camera, float cursorSize)
		{
			Matrix decalView = Matrix.CreateLookAt(camera.Position, new Vector3(camera.Target.X, 0f, camera.Target.Z), Vector3.Forward);
			Matrix decalProj = Matrix.CreateOrthographic(cursorSize, cursorSize, camera.NearPlane, camera.FarPlane);

			return decalView * decalProj;
		}

		/// <summary>
		/// Get a Velocity Vector based on the Angle and Speed
		/// </summary>
		/// <param name="angle">Angle in Degrees</param>
		/// <param name="speed">Speed</param>
		/// <returns></returns>
		public static Vector2 VelocityFromAngleSpeed(int angle, float speed)
		{
			double a = MathHelper.ToRadians(angle);

			float x = (float)Math.Sin(a) * speed;
			float y = (float)Math.Cos(a) * speed;

			return new Vector2(x, y);
		}

		/// <summary>
		/// Takes a Day Color and a Night Color and Lerps them based on the current hour of the day
		/// </summary>
		/// <param name="hour">Current Hour</param>
		/// <param name="dayColor">Day Color</param>
		/// <param name="nightColor">Night Color</param>
		/// <returns></returns>
		public static Color GetColorBasedOnTimeOfDay(int hour, Color dayColor, Color nightColor)
		{
			if (hour > 12)
			{
				float lerpValue = (float)(hour - 12) / 12;
				return Color.Lerp(dayColor, nightColor, lerpValue);
			}
			else
			{
				float lerpValue = (float)hour / 12;
				return Color.Lerp(nightColor, dayColor, lerpValue);
			}
		}

		public static T LoadXML<T>(string filename) where T : class
		{
			using (var store = IsolatedStorageFile.GetUserStoreForApplication())
			{
				return store.FileExists(filename) ? Serializer.IsoDeserialize<T>(store, filename) : null;
			}
		}

		public static void SaveXML(string filename, object obj)
		{
			using (var store = IsolatedStorageFile.GetUserStoreForApplication())
			{
				Serializer.IsoSerialize(store, filename, obj);
			}
		}

		public static void DeleteXML(string filename)
		{
			using (var store = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (store.FileExists(filename))
					store.DeleteFile(filename);
			}
		}

		public static void CopyObject<T>(object sourceObject, ref T destObject) where T : class
		{
			//	If either the source, or destination is null, return
			if (sourceObject == null || destObject == null)
				return;

			//	Get the type of each object
			Type sourceType = sourceObject.GetType();
			Type targetType = destObject.GetType();

			//	Loop through the source properties
			foreach (PropertyInfo p in sourceType.GetProperties())
			{
				//	Get the matching property in the destination object
				PropertyInfo targetObj = targetType.GetProperty(p.Name);

				//	If there is none, skip
				if (targetObj == null)
					continue;

				if (p.Name == "GraphicsDevice" || p.Name == "Width" || p.Name == "Height" || p.Name == "IsInitialized" || p.Name == "Texture")
					continue;

				//	Set the value in the destination
				targetObj.SetValue(destObject, p.GetValue(sourceObject, null), null);
			}
		}

		public static TimeSpan GetRandomTimeSpan(TimeSpan min, TimeSpan max)
		{
			double minTicks = min.TotalSeconds;
			double maxTicks = max.TotalSeconds;

			int seconds = GetRandomInt((int)minTicks, (int)maxTicks);
			return TimeSpan.FromSeconds(seconds);
		}

    public static Vector2 GetCenterScreenPosition(Viewport viewport, int width, int height, Vector2 scale)
    {
      int vw = viewport.Width/2;
      int vh = viewport.Height/2;

      return new Vector2(vw, vh);
    }
	}
}