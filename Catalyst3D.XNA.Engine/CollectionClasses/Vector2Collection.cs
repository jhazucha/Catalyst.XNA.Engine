#if !XBOX360 && !WINDOWS_PHONE

using System;
using System.Collections;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Catalyst3D.XNA.Engine.EntityClasses.CollectionClasses
{
  public class Vector2Collection : CollectionBase, ICustomTypeDescriptor
  {
    public Vector2Collection(Vector2Collection spriteValue)
    {
      AddRange(spriteValue);
    }

    public void AddRange(Vector2Collection spriteValue)
    {
      for (int Counter = 0; Counter < spriteValue.Count; Counter++)
      {
        Add(spriteValue[Counter]);
      }
    }

    public Vector2Collection(List<Vector2> spriteValue)
    {
      AddRange(spriteValue);
    }

    public Vector2Collection()
    {
      
    }

    public void AddRange(List<Vector2> spriteValue)
    {
      for (int Counter = 0; Counter < spriteValue.Count; Counter++)
      {
        Add(spriteValue[Counter]);
      }
    }

    public Vector2 this[int intIndex]
    {
      get
      {
        return ((Vector2) List[intIndex]);
      }
      set { List[intIndex] = value; }
    }

    public int Add(Vector2 spriteValue)
    {
      return List.Add(spriteValue);
    }

    public bool Contains(Vector2 spriteValue)
    {
      return List.Contains(spriteValue);
    }

    public void CopyTo(Vector2Collection[] spriteValue, int intIndex)
    {
      List.CopyTo(spriteValue, intIndex);
    }

    public int IndexOf(Vector2Collection spriteValue)
    {
      return List.IndexOf(spriteValue);
    }

    public void Insert(int intIndex, Vector2Collection spriteValue)
    {
      List.Insert(intIndex, spriteValue);
    }

    public void Remove(Vector2 spriteValue)
    {
      List.Remove(spriteValue);
    }

    public new BaseSpriteEnumerator GetEnumerator()
    {
      return new BaseSpriteEnumerator(this);
    }

    public class BaseSpriteEnumerator : IEnumerator
    {
      private readonly IEnumerator BaseEnumerator;

      public BaseSpriteEnumerator(IEnumerable spriteMappings)
      {
        BaseEnumerator = (spriteMappings).GetEnumerator();
      }

      public Vector2Collection Current
      {
        get { return ((Vector2Collection)(BaseEnumerator.Current)); }
      }

      object IEnumerator.Current
      {
        get { return BaseEnumerator.Current; }
      }

      public bool MoveNext()
      {
        return BaseEnumerator.MoveNext();
      }

      bool IEnumerator.MoveNext()
      {
        return BaseEnumerator.MoveNext();
      }

      public void Reset()
      {
        BaseEnumerator.Reset();
      }

      void IEnumerator.Reset()
      {
        BaseEnumerator.Reset();
      }
    }

    public AttributeCollection GetAttributes()
    {
      return TypeDescriptor.GetAttributes(this, true);
    }

    public string GetClassName()
    {
      return TypeDescriptor.GetClassName(this, true);
    }

    public string GetComponentName()
    {
      return TypeDescriptor.GetComponentName(this, true);
    }

    public TypeConverter GetConverter()
    {
      return TypeDescriptor.GetConverter(this, true);
    }

    public EventDescriptor GetDefaultEvent()
    {
      return TypeDescriptor.GetDefaultEvent(this, true);
    }

    public PropertyDescriptor GetDefaultProperty()
    {
      return TypeDescriptor.GetDefaultProperty(this, true);
    }

    public object GetEditor(Type editorBaseType)
    {
      return TypeDescriptor.GetEditor(this, editorBaseType, true);
    }

    public EventDescriptorCollection GetEvents()
    {
      return TypeDescriptor.GetEvents(this, true);
    }

    public EventDescriptorCollection GetEvents(Attribute[] attributes)
    {
      return TypeDescriptor.GetEvents(this, attributes, true);
    }

    public PropertyDescriptorCollection GetProperties()
    {
      PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);
      for (int i = 0; i < List.Count; i++)
      {
        // PropertyDescriptorCollection instance
        EmitterCollectionPropertyDescriptor pd = new EmitterCollectionPropertyDescriptor(this, i);
        pds.Add(pd);
      }
      return pds;
    }

    public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
    {
      return GetProperties();
    }

    public object GetPropertyOwner(PropertyDescriptor pd)
    {
      return this;
    }
  }

  public class EmitterCollectionPropertyDescriptor : PropertyDescriptor
  {
    private readonly Vector2Collection collection;
    private readonly int index = -1;

    public EmitterCollectionPropertyDescriptor(Vector2Collection coll, int idx)
      : base("#" + idx, null)
    {
      collection = coll;
      index = idx;
    }

    public override AttributeCollection Attributes
    {
      get { return TypeDescriptor.GetAttributes(this, true); }
    }

    public override bool CanResetValue(object component)
    {
      return true;
    }

    public override Type ComponentType
    {
      get { return collection.GetType(); }
    }

    public override string DisplayName
    {
      get
      {
        Vector2 sprite = collection[index];
        return "X:" + sprite.X + "," + "Y:" + sprite.Y;
      }
    }

    public override string Description
    {
      get
      {
        Vector2 sprite = collection[index];
        return "X:" + sprite.X + "," + "Y:" + sprite.Y;
      }
    }

    public override object GetValue(object component)
    {
      return collection[index];
    }

    public override bool IsReadOnly
    {
      get { return true; }
    }

    public override string Name
    {
      get { return "#" + index; }
    }

    public override Type PropertyType
    {
      get { return collection[index].GetType(); }
    }

    public override void ResetValue(object component)
    {
    }

    public override bool ShouldSerializeValue(object component)
    {
      return true;
    }

    public override void SetValue(object component, object value)
    {
    }
  }
}

#endif