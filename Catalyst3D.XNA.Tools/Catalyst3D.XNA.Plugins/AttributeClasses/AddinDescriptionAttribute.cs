using System;

namespace Catalyst3D.PluginSDK.AttributeClasses
{
  [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
  public class AddinDescriptionAttribute : Attribute
  {
    public readonly string AddinDescription;
    public AddinDescriptionAttribute(string addindescription)
    {
      AddinDescription = addindescription;
    }
  }
}
