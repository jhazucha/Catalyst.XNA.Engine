using System;
using Catalyst3D.XNA.Engine.UtilityClasses;
using NUnit.Framework;

namespace Catalyst3D.XNA.Testing.Graphics.Models
{
	[TestFixture]
	public class LoadModelsAtRunTimeTest
	{
		[Test]
		public void Load()
		{
			// Create our Builder
		  ContentBuilder builder = new ContentBuilder(@"C:\Build\Output");

			// Add our Build Files
			builder.Add(@"C:\Build\Original\Stadium1.fbx", "Stadium1", null, "ModelProcessor");

			// Build our content
			string result = builder.Build();

			// Display Results
			if(string.IsNullOrEmpty(result))
				Console.WriteLine("Success ok to load model!");
			else
				Console.WriteLine(result);
		}
	}
}