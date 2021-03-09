#if !XBOX360 && !WINDOWS_PHONE

#region Using Statements

using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;

#endregion

namespace Catalyst3D.XNA.Engine.UtilityClasses
{
	public class ContentBuilder : IDisposable
	{
		const string xnaVersion = ", Version=4.0.0.0, PublicKeyToken=842cf8be1de50553";

		private static readonly string[] pipelineAssemblies =
			{
				"Microsoft.Xna.Framework.Content.Pipeline.FBXImporter" + xnaVersion,
				"Microsoft.Xna.Framework.Content.Pipeline.XImporter" + xnaVersion,
				"Microsoft.Xna.Framework.Content.Pipeline.TextureImporter" + xnaVersion,
				"Microsoft.Xna.Framework.Content.Pipeline.EffectImporter" + xnaVersion,

				// If you want to use custom importers or processors from
				// a Content Pipeline Extension Library, add them here.
				//
				// If your extension DLL is installed in the GAC, you should refer to it by assembly
				// name, eg. "MyPipelineExtension, Version=1.0.0.0, PublicKeyToken=1234567812345678".
				//
				// If the extension DLL is not in the GAC, you should refer to it by
				// file path, eg. "c:/MyProject/bin/MyPipelineExtension.dll".
			};

		// MSBuild objects used to dynamically build content.
		Project buildProject;
		ProjectRootElement projectRootElement;
		BuildParameters buildParameters;
		readonly List<ProjectItem> projectItems = new List<ProjectItem>();
		ErrorLogger errorLogger;

		// Temporary directories used by the content build.
		public string BuildDirectory;
		
		// Have we been disposed?
		bool isDisposed;

		public string OutputDirectory;

		public ContentBuilder(string outputDirectory)
		{
			BuildDirectory = @"C:\Catalyst3D\";
			OutputDirectory = outputDirectory;

			// Create the build project.
			projectRootElement = ProjectRootElement.Create(@"C:\Catalyst3D\");

			// Include the standard targets file that defines how to build XNA Framework content.
			projectRootElement.AddImport("$(MSBuildExtensionsPath)\\Microsoft\\XNA Game Studio\\" +
																	 "v4.0\\Microsoft.Xna.GameStudio.ContentPipeline.targets");

			buildProject = new Project(projectRootElement);

			buildProject.SetProperty("XnaPlatform", "Windows");
			buildProject.SetProperty("XnaProfile", "Reach");
			buildProject.SetProperty("XnaFrameworkVersion", "v4.0");
			buildProject.SetProperty("Configuration", "Release");
			buildProject.SetProperty("OutputPath", outputDirectory);

			// Register any custom importers or processors.
			foreach(string pipelineAssembly in pipelineAssemblies)
			{
				buildProject.AddItem("Reference", pipelineAssembly);
			}

			// Hook up our custom error logger.
			errorLogger = new ErrorLogger();

			buildParameters = new BuildParameters(ProjectCollection.GlobalProjectCollection);
			buildParameters.Loggers = new ILogger[] { errorLogger };
		}

		public void Add(string filename, string name, string importer, string processor)
		{
			ProjectItem item = buildProject.AddItem("Compile", filename)[0];

			item.SetMetadataValue("Link", Path.GetFileName(filename));
			item.SetMetadataValue("Name", name);

			if(!string.IsNullOrEmpty(importer))
				item.SetMetadataValue("Importer", importer);

			if(!string.IsNullOrEmpty(processor))
				item.SetMetadataValue("Processor", processor);

			projectItems.Add(item);
		}

		public void Clear()
		{
			buildProject.RemoveItems(projectItems);
			projectItems.Clear();
		}

		public string Build()
		{
			// Clear any previous errors.
			errorLogger.Errors.Clear();

			// Create and submit a new asynchronous build request.
			BuildManager.DefaultBuildManager.BeginBuild(buildParameters);

			BuildRequestData request = new BuildRequestData(buildProject.CreateProjectInstance(), new string[0]);
			BuildSubmission submission = BuildManager.DefaultBuildManager.PendBuildRequest(request);

			submission.ExecuteAsync(null, null);

			// Wait for the build to finish.
			submission.WaitHandle.WaitOne();

			BuildManager.DefaultBuildManager.EndBuild();

			// If the build failed, return an error string.
			if (submission.BuildResult.OverallResult == BuildResultCode.Failure)
			{
				return string.Join("\n", errorLogger.Errors.ToArray());
			}

			// TODO: Clean up temporary/build folders!!

			Directory.Delete(@"C:\Catalyst3D\", true);

			return null;
		}

		~ContentBuilder()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(!isDisposed)
			{
				isDisposed = true;
			}
		}
	}
}

#endif