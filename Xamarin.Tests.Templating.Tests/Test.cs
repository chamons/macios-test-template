using NUnit.Framework;
using System;
using System.IO;

namespace Xamarin.Tests.Templating.Tests
{
	[TestFixture ()]
	public class SmokeTests
	{
		// This is a poor sub for https://github.com/xamarin/xamarin-macios/blob/master/tests/mtouch/Cache.cs#L29
		public string GetTemporaryDirectory ()
		{
			string tempDirectory = Path.Combine (Path.GetTempPath (), Path.GetRandomFileName ());
			Directory.CreateDirectory (tempDirectory);
			return tempDirectory;
		}

		[Test ()]
		public void MacTestLibrary ()
		{
			var tmpDir = GetTemporaryDirectory ();

			try {
				var engine = new MacLibraryTemplateEngine (ProjectFlavor.FullXM, ProjectLanguage.CSharp);
				var fileSubstitutions = new FileSubstitutions { TestCode = "System.Console.WriteLine (typeof (int));" };
				var projectSubstitutions = new ProjectSubstitutions {
					References = "<Reference Include=\"System.Net.Http\" />",
				};
				string projectPath = engine.Generate (tmpDir, projectSubstitutions, fileSubstitutions);


			}
			finally {
			//	Directory.Delete (tmpDir, true);
			}
		}
	}
}
