#if !XBOX360 && !WINDOWS_PHONE
using System;
using System.Collections;
using System.ComponentModel;
using Catalyst3D.XNA.Engine.EntityClasses.Camera;

namespace Catalyst3D.XNA.Engine.CollectionClasses
{
	public class CameraCollection : CollectionBase, ICustomTypeDescriptor
	{
		public CameraCollection(CameraCollection camValue)
		{
			AddRange(camValue);
		}

		public void AddRange(CameraCollection camValue)
		{
			for (int Counter = 0; Counter < camValue.Count; Counter++)
			{
				Add(camValue[Counter]);
			}
		}

		public CameraCollection(BasicCamera[] camValue)
		{
			AddRange(camValue);
		}

		public void AddRange(BasicCamera[] camValue)
		{
			for (int Counter = 0; Counter < camValue.Length; Counter++)
			{
				Add(camValue[Counter]);
			}
		}

		public BasicCamera this[int intIndex]
		{
			get { return ((BasicCamera)List[intIndex]); }
			set { List[intIndex] = value; }
		}

		public int Add(BasicCamera camValue)
		{
			return List.Add(camValue);
		}

		public bool Contains(CameraCollection camValue)
		{
			return List.Contains(camValue);
		}

		public void CopyTo(CameraCollection[] camArray, int intIndex)
		{
			List.CopyTo(camArray, intIndex);
		}

		public int IndexOf(CameraCollection camValue)
		{
			return List.IndexOf(camValue);
		}

		public void Insert(int intIndex, CameraCollection camValue)
		{
			List.Insert(intIndex, camValue);
		}

		public void Remove(CameraCollection camValue)
		{
			List.Remove(camValue);
		}

		public new BaseCameraEnumerator GetEnumerator()
		{
			return new BaseCameraEnumerator(this);
		}

		public class BaseCameraEnumerator : IEnumerator
		{
			private readonly IEnumerator BaseEnumerator;

			public BaseCameraEnumerator(IEnumerable camMappings)
			{
				BaseEnumerator = (camMappings).GetEnumerator();
			}

			public CameraCollection Current
			{
				get { return ((CameraCollection)(BaseEnumerator.Current)); }
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
				CTBaseCameraCollectionPropertyDescriptor pd = new CTBaseCameraCollectionPropertyDescriptor(this, i);
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

	public class CTBaseCameraCollectionPropertyDescriptor : PropertyDescriptor
	{
		private readonly CameraCollection collection = null;
		private readonly int index = -1;

		public CTBaseCameraCollectionPropertyDescriptor(CameraCollection coll, int idx)
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
				BasicCamera camera = collection[index];
				if (camera.Name == null)
				{
					return camera.GetType().ToString();
				}
				else
				{
					return camera.Name;
				}
			}
		}

		public override string Description
		{
			get
			{
				BasicCamera camera = collection[index];
				if (camera.Name == null)
				{
					return camera.GetType().ToString();
				}
				else
				{
					return camera.Name;
				}
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