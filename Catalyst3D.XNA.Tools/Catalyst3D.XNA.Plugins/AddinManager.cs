using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting;
using Catalyst3D.PluginSDK.AbstractClasses;
using Catalyst3D.PluginSDK.AttributeClasses;
using Catalyst3D.PluginSDK.Interfaces;
using Catalyst3D.XNA.Engine.UtilityClasses;

namespace Catalyst3D.PluginSDK
{
	public static class AddinManager
	{
		public static IAddinHost Host { get; set; }
		public static List<Addin> Addins = new List<Addin>();

		public static List<Addin> ExamineAddin(string file)
		{
			List<Addin> addins = new List<Addin>();

			Assembly asm = Assembly.LoadFrom(file);
			if(asm != null)
			{
				// get the types
				Type[] types = asm.GetExportedTypes();
				foreach(Type type in types)
				{
					// check for interface
					if(typeof(Addin).IsAssignableFrom(type))
					{
						// populate a new addin
						Addin addin = new Addin();
						addin.File = file;
						addin.Path = type.ToString();
						addin.Name = GetNameAttribute(type);
						addin.Description = GetDescriptionAttribute(type);
						addin.Guid = type.GUID;

						// add to collection
						addins.Add(addin);
					}
				}
			}

			// return
			return addins;
		}

		public static void LoadAddins(List<Addin> addins, IAddinHost host)
		{
			// create instance of each addin
			foreach (Addin addin in addins)
			{
				// create instance
				ObjectHandle handle = Activator.CreateComInstanceFrom(addin.File, addin.Path);
				Addin instance = (Addin) handle.Unwrap();
				addin.Instance = instance;

				// add the addin to the global collection
				Addins.Add(addin);

				// call the register method on the addin
				instance.Register(host);
			}
		}

		public static void Register(Addin a)
		{
			ObjectHandle handle = Activator.CreateComInstanceFrom(a.File, a.Path);
			Addin instance = (Addin) handle.Unwrap();
			a.Instance = instance;
			instance.Register(Host);
		}

		private static string GetNameAttribute(Type type)
		{
			string name = "";
			object[] attributes = type.GetCustomAttributes(typeof(AddinNameAttribute), false);
			if(attributes.Length == 1)
			{
				AddinNameAttribute na = (AddinNameAttribute)attributes[0];
				name = na.Name;
			}
			return name;
		}
		private static string GetDescriptionAttribute(Type type)
		{
			string desc = "";
			object[] attributes = type.GetCustomAttributes(typeof(AddinDescriptionAttribute), false);
			if(attributes.Length == 1)
			{
				AddinDescriptionAttribute da = (AddinDescriptionAttribute)attributes[0];
				desc = da.AddinDescription;
			}
			return desc;
		}

		public static void SavePlugins(string filename)
		{
			Serializer.Serialize(filename, Addins);
		}

		public static List<Addin> LoadPlugins(string filename)
		{
			return Serializer.Deserialize<List<Addin>>(filename);
		}
	}
}