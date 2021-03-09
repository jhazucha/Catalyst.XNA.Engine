using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Serialization;

namespace Catalyst3D.XNA.Engine.UtilityClasses
{
	public class Serializer
	{
#if !WINDOWS_PHONE
		public static T Deserialize<T>(string file) where T : class
		{
			T obj;
			XmlReader reader = null;
			FileStream fs = null;

			try
			{
				// get a serializer to serialize the passed in object type
				XmlSerializer serializer = new XmlSerializer(typeof (T));

				// open the file stream
			  fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);

				// create an xml reader from the file stream
				reader = new XmlTextReader(fs);

				// use the Deserialize method to restore the object's state.
				obj = (T) serializer.Deserialize(reader);
			}
			finally
			{
				// close any open streams
				if (fs != null)
					fs.Close();

				if (reader != null)
					reader.Close();
			}

			return obj;
		}

		public static void Serialize(string file, object obj)
		{
			TextWriter writer = null;

			try
			{
				// get a serializer to serialize the passed in object type
				XmlSerializer serializer = new XmlSerializer(obj.GetType());

				writer = new StreamWriter(file);
				serializer.Serialize(writer, obj);
			}
			finally
			{
				// close any open streams
				if (writer != null)
					writer.Close();
			}
		}
#endif

		/// <summary>
		/// Isolated Storage Object De-Serialization
		/// </summary>
		/// <typeparam name="T">Object Type</typeparam>
		/// <param name="store">Isolated Storage File Store</param>
		/// <param name="file">Filename</param>
		/// <returns></returns>
		public static T IsoDeserialize<T>(IsolatedStorageFile store, string file) where T : class
		{
			try
			{
				using (var stream = new IsolatedStorageFileStream(file, FileMode.Open, FileAccess.Read, store))
				{
					var serializer = new XmlSerializer(typeof (T));
					TextReader reader = new StreamReader(stream);

					return (T) serializer.Deserialize(reader);
				}
			}
			catch(Exception er)
			{
				throw new Exception(er.Message);
			}
		}

		/// <summary>
		/// Isolated Storage Object Serialization
		/// </summary>
		/// <param name="store">Isolated Storage File Store</param>
		/// <param name="filename">Filename</param>
		/// <param name="obj">Object to Serialize</param>
		public static void IsoSerialize(IsolatedStorageFile store, string filename, object obj)
		{
			using (var stream = new IsolatedStorageFileStream(filename, FileMode.Create, FileAccess.Write, store))
			{
				try
				{
					var serializer = new XmlSerializer(obj.GetType());
					TextWriter writer = new StreamWriter(stream);

					serializer.Serialize(writer, obj);
				}
				catch(Exception er)
				{
					throw new Exception(er.Message);
				}
			}
		}
	}
}

