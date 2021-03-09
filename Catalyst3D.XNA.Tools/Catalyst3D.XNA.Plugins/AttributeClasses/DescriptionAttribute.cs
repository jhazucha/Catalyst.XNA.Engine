using System;

namespace Catalyst3D.PluginSDK.AttributeClasses
{
  [AttributeUsage(AttributeTargets.All)]
  public class DescriptionAttribute : Attribute
  {
    public readonly string Description;

    public DescriptionAttribute(string description)
    {
      Description = description;
    }
  }
}
