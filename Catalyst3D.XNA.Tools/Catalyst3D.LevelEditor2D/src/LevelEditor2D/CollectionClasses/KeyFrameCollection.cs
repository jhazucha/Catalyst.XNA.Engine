using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;
using Microsoft.Xna.Framework;

namespace LevelEditor2D.CollectionClasses
{
  public class KeyFrameCollection : CollectionBase, ICustomTypeDescriptor
  {
    public KeyFrameCollection(KeyFrameCollection spriteValue)
    {
      AddRange(spriteValue);
    }

    public void AddRange(KeyFrameCollection spriteValue)
    {
      for (int Counter = 0; Counter < spriteValue.Count; Counter++)
      {
        Add(spriteValue[Counter]);
      }
    }

    public KeyFrameCollection(List<KeyFrame2D> spriteValue)
    {
      AddRange(spriteValue);
    }

    public KeyFrameCollection()
    {

    }

		public void AddRange(List<KeyFrame2D> spriteValue)
    {
      for (int Counter = 0; Counter < spriteValue.Count; Counter++)
      {
        Add(spriteValue[Counter]);
      }
    }

		public KeyFrame2D this[int intIndex]
    {
      get
      {
				return ((KeyFrame2D)List[intIndex]);
      }
      set { List[intIndex] = value; }
    }

		public int Add(KeyFrame2D spriteValue)
    {
      return List.Add(spriteValue);
    }

		public bool Contains(KeyFrame2D spriteValue)
    {
      return List.Contains(spriteValue);
    }

    public void CopyTo(KeyFrameCollection[] spriteValue, int intIndex)
    {
      List.CopyTo(spriteValue, intIndex);
    }

    public int IndexOf(KeyFrameCollection spriteValue)
    {
      return List.IndexOf(spriteValue);
    }

    public void Insert(int intIndex, KeyFrameCollection spriteValue)
    {
      List.Insert(intIndex, spriteValue);
    }

    public void Remove(Vector2 spriteValue)
    {
      List.Remove(spriteValue);
    }

    public new KeyFrameEnumerator GetEnumerator()
    {
      return new KeyFrameEnumerator(this);
    }

    public class KeyFrameEnumerator : IEnumerator
    {
      private readonly IEnumerator BaseEnumerator;

      public KeyFrameEnumerator(IEnumerable spriteMappings)
      {
        BaseEnumerator = (spriteMappings).GetEnumerator();
      }

      public KeyFrameCollection Current
      {
        get { return ((KeyFrameCollection)(BaseEnumerator.Current)); }
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
        KeyFrameCollectionPropertyDescriptor pd = new KeyFrameCollectionPropertyDescriptor(this, i);
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

  public class KeyFrameCollectionPropertyDescriptor : PropertyDescriptor
  {
    private readonly KeyFrameCollection collection;
    private readonly int index = -1;

    public KeyFrameCollectionPropertyDescriptor(KeyFrameCollection coll, int idx)
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
				KeyFrame2D sprite = collection[index];
      	return "KeyFrame2D";
      }
    }

    public override string Description
    {
      get
      {
        return "KeyFrame2D";
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
