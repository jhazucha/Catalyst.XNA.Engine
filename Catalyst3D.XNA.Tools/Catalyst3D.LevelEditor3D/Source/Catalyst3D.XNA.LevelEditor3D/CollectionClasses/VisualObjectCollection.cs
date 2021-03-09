using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Catalyst3D.XNA.Engine.AbstractClasses;

namespace LevelEditor3D.CollectionClasses
{
  public class BaseVisualObjectEnumerator : IEnumerator
  {
    private readonly IEnumerator BaseEnumerator;

    public BaseVisualObjectEnumerator(IEnumerable spriteMappings)
    {
      BaseEnumerator = (spriteMappings).GetEnumerator();
    }

    public VisualObjectCollection Current
    {
      get { return ((VisualObjectCollection)(BaseEnumerator.Current)); }
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

  public class VisualObjectCollection : CollectionBase, ICustomTypeDescriptor
  {
    public void AddRange(List<VisualObject> sprites)
    {
      for (int Counter = 0; Counter < sprites.Count; Counter++)
      {
        Add(sprites[Counter]);
      }
    }

    public VisualObjectCollection(VisualObject[] value)
    {
      AddRange(value);
    }

    public VisualObjectCollection()
    {
    }

    public void AddRange(VisualObject[] value)
    {
      for (int Counter = 0; Counter < value.Length; Counter++)
      {
        Add(value[Counter]);
      }
    }

    public void AddRange(VisualObjectCollection vo)
    {
      for (int i = 0; i < vo.Count; i++)
        Add(vo[i]);
    }

    public VisualObject this[int intIndex]
    {
      get { return ((VisualObject)List[intIndex]); }
      set { List[intIndex] = value; }
    }

    public int Add(VisualObject value)
    {
      return List.Add(value);
    }

    public bool Contains(VisualObject value)
    {
      return List.Contains(value);
    }

    public void CopyTo(VisualObjectCollection[] spriteArray, int intIndex)
    {
      List.CopyTo(spriteArray, intIndex);
    }

    public int IndexOf(VisualObjectCollection value)
    {
      return List.IndexOf(value);
    }

    public void Insert(int intIndex, VisualObjectCollection value)
    {
      List.Insert(intIndex, value);
    }

    public void Remove(VisualObject sprite)
    {
      List.Remove(sprite);
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
        EmitterObjectCollectionPropertyDescriptor pd = new EmitterObjectCollectionPropertyDescriptor(this, i);
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

  public class EmitterObjectCollectionPropertyDescriptor : PropertyDescriptor
  {
    private readonly VisualObjectCollection collection;
    private readonly int index = -1;

    public EmitterObjectCollectionPropertyDescriptor(VisualObjectCollection coll, int idx)
      : base("#" + idx, null)
    {
      collection = coll;
      index = idx;
    }

    public override AttributeCollection Attributes
    {
      get { return new AttributeCollection(null); }
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
        VisualObject sprite = collection[index];
        if(string.IsNullOrEmpty(sprite.Name))
        {
          return sprite.GetType().Name;
        }

        return sprite.Name;
      }
    }

    public override string Description
    {
      get
      {
        VisualObject sprite = collection[index];
        StringBuilder builder = new StringBuilder();

        builder.Append(sprite.AssetName);
        builder.Append(" [");
        builder.Append(sprite.GetType());
        builder.Append("]");

        return builder.ToString();
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
      get { return DisplayName + index; }
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