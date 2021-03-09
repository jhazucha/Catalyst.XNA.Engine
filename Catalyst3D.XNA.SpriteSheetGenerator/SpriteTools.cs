using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Catalyst3D.XNA.Engine.EntityClasses;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;

namespace Catalyst3D.XNA.Tools
{
	public static class SpriteTools
	{
		public static void GenerateSpriteSheet(Actor actor, string projectPath, string filename)
		{
			const int padding = 10;

			const int MaxWidth = 2048;
			const int MaxHeight = 2048;

			int currentWidth = 0;
			int currentHeight = 0;

			int currentX = padding;
			int currentY = padding;

			var tempImage = new Bitmap(MaxWidth, MaxHeight, PixelFormat.Format32bppArgb);

			foreach (Sequence2D s in actor.ClipPlayer.Sequences)
			{
				foreach (KeyFrame2D k in s.Frames)
				{
					var bitmap = Image.FromFile(projectPath + @"/Temp/" + k.AssetFilename) as Bitmap;

					if (bitmap != null)
					{
						if ((currentX + bitmap.Width + padding) > (MaxWidth - padding))
						{
							if (currentY + bitmap.Height > MaxHeight - padding)
								throw new Exception("Image would roll off the sprite sheet!");

							currentWidth = MaxWidth;

							// Needs to go down a row
							currentY += bitmap.Height + padding;
							currentX = padding;
						}

						for (int x = 0; x < bitmap.Width; x++)
						{
							for (int y = 0; y < bitmap.Height; y++)
							{
								currentHeight = bitmap.Height;
								tempImage.SetPixel(x + currentX, y + currentY, bitmap.GetPixel(x, y));
							}
						}

						// Store the bounding Recangle coords
						k.SourceRectangle = new Microsoft.Xna.Framework.Rectangle(currentX, currentY, bitmap.Width, bitmap.Height);

						if (currentWidth < MaxWidth)
							currentWidth += (padding + bitmap.Width + padding);
						else
							currentWidth = MaxWidth;

						currentX += (padding + bitmap.Width + padding);
					}
				}
			}

			// Now lets clamp this image to the height of our last row of frames
			var final = new Bitmap(currentWidth, currentY + currentHeight + padding, PixelFormat.Format32bppArgb);

			for (int x = 0; x < currentWidth; x++)
			{
				for (int y = 0; y < currentY + currentHeight; y++)
				{
					final.SetPixel(x, y, tempImage.GetPixel(x, y));
				}
			}

			string fn = filename.Replace(" ", "");

			final.Save(projectPath + @"\" + fn + "-SpriteSheet.png", ImageFormat.Png);

			actor.AssetName = Path.GetFileNameWithoutExtension(fn);
			actor.SpriteSheetFileName = fn + "-SpriteSheet.png";
		}
	}
}