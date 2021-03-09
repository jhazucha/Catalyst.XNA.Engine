#if !XBOX360 && !WINDOWS_PHONE

using System;
using System.Collections;
using System.ComponentModel;
using Catalyst3D.XNA.Engine.EntityClasses.Sprites;

namespace Catalyst3D.XNA.Engine.CollectionClasses
{
	public class SpriteCollection : CollectionBase, ICustomTypeDescriptor
	{
		public SpriteCollection(SpriteCollection spriteValue)
		{
			AddRange(spriteValue);
		}

		public void AddRange(SpriteCollection spriteValue)
		{
			for (int Counter = 0; Counter < spriteValue.Count; Counter++)
			{
				Add(spriteValue[Counter]);
			}
		}

		public SpriteCollection(Sprite[] spriteValue)
		{
			AddRange(spriteValue);
		}

		public void AddRange(Sprite[] spriteValue)
		{
			for (int Counter = 0; Counter < spriteValue.Length; Counter++)
			{
				Add(spriteValue[Counter]);
			}
		}

		public Sprite this[int intIndex]
		{
			get { return ((Sprite)List[intIndex]); }
			set { List[intIndex] = value; }
		}

		public int Add(Sprite spriteValue)
		{
			return List.Add(spriteValue);
		}

		public bool Contains(Sprite spriteValue)
		{
			return List.Contains(spriteValue);
		}

		public void CopyTo(SpriteCollection[] spriteValue, int intIndex)
		{
			List.CopyTo(spriteValue, intIndex);
		}

		public int IndexOf(SpriteCollection spriteValue)
		{
			return List.IndexOf(spriteValue);
		}

		public void Insert(int intIndex, SpriteCollection spriteValue)
		{
			List.Insert(intIndex, spriteValue);
		}

		public void Remove(SpriteCollection spriteValue)
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

			public SpriteCollection Current
			{
				get { return ((SpriteCollection)(BaseEnumerator.Current)); }
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
				CTBaseSpriteCollectionPropertyDescriptor pd = new CTBaseSpriteCollectionPropertyDescriptor(this, i);
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

	public class CTBaseSpriteCollectionPropertyDescriptor : PropertyDescriptor
	{
		private readonly SpriteCollection collection;
		private readonly int index = -1;

		public CTBaseSpriteCollectionPropertyDescriptor(SpriteCollection coll, int idx)
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
				Sprite sprite = collection[index];
				if (sprite.Texture.Name == null)
				{
					return sprite.AssetName;
				}

				return sprite.Texture.Name;
			}
		}

		public override string Description
		{
			get
			{
				Sprite sprite = collection[index];
				if (sprite.Texture.Name == null)
				{
					return sprite.AssetName;
				}

				return sprite.Texture.Name;
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