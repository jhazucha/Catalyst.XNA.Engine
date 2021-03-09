#if !XBOX360 && !WINDOWS_PHONE

using System;
using System.Diagnostics;
using System.IO;

namespace Catalyst3D.XNA.Engine.UtilityClasses
{
	public class Debug
	{
		//Note: this is probably not available on the 360 so will need to look into that if ported

		public static void EnabledTracing()
		{
			EnabledTracing(AppDomain.CurrentDomain.BaseDirectory + @"\debug.txt");
		}

		public static void EnabledTracing(string file)
		{
			// create file stream
			FileStream fs = new FileStream(file, FileMode.Create);

			// create trace listener
			TextWriterTraceListener tracelistener = new TextWriterTraceListener(fs);
			Trace.Listeners.Add(tracelistener);
			Trace.AutoFlush = true;
		}

		public static void Print(string s)
		{
			System.Diagnostics.Debug.WriteLine(s);
		}
	}
}
#endif