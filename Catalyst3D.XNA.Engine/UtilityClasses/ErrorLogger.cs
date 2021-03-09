#if !WINDOWS_PHONE

#region Using Statements
using System.Collections.Generic;
using Microsoft.Build.Framework;
#endregion

namespace Catalyst3D.XNA.Engine.UtilityClasses
{
	public class ErrorLogger : ILogger
	{
		public void Initialize(IEventSource eventSource)
		{
			if (eventSource != null)
			{
				eventSource.ErrorRaised += ErrorRaised;
			}
		}

		public void Shutdown()
		{
		}

		void ErrorRaised(object sender, BuildErrorEventArgs e)
		{
			errors.Add(e.Message);
		}

		public List<string> Errors
		{
			get { return errors; }
		}

		readonly List<string> errors = new List<string>();

		#region ILogger Members

		string ILogger.Parameters
		{
			get { return parameters; }
			set { parameters = value; }
		}

		string parameters;

		LoggerVerbosity ILogger.Verbosity
		{
			get { return verbosity; }
			set { verbosity = value; }
		}

		LoggerVerbosity verbosity = LoggerVerbosity.Normal;

		#endregion
	}
}

#endif