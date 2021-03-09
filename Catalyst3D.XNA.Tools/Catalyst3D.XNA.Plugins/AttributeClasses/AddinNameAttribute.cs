using System;

namespace Catalyst3D.PluginSDK.AttributeClasses
{
  [AttributeUsage(AttributeTargets.All)]
  public class AddinNameAttribute : Attribute
  {
    public readonly string Name;
    public AddinNameAttribute(string name)
    {
      Name = name;
    }
  }
}
