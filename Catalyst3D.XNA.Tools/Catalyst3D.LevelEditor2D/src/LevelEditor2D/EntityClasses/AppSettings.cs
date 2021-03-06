using System;
using Microsoft.Xna.Framework;

namespace LevelEditor2D.EntityClasses
{
  [Serializable]
  public class AppSettings
  {
    public string ContentPath;
    public string ProjectPath;
    public string RenderSurfaceColor;
    public string ProjectFilename;
  	public string ObjectTypesPath;

    public int Resolution;

    public Color GetRenderSurfaceColor()
    {
      switch (RenderSurfaceColor)
      {
        case "Black":
          return Color.Black;
        case "Cornflower Blue":
          return Color.CornflowerBlue;
        case "Dark Gray":
          return Color.DarkGray;
        case "White":
          return Color.White;
      }

      // Default
      return Color.CornflowerBlue;
    }
  }
}