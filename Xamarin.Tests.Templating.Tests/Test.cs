using NUnit.Framework;
using System;
using System.IO;

namespace Xamarin.Tests.Templating.Tests
{
	[TestFixture ()]
	public class SmokeTests
	{
		[Test ()]
		public void MacTestApp ()
		{
			var tmpDir = Cache.CreateTemporaryDirectory ();

			var engine = new MacAppTemplateEngine (ProjectFlavor.FullXM, ProjectLanguage.CSharp);
			var fileSubstitutions = new FileSubstitutions { TestCode = "System.Console.WriteLine (typeof (int));" };
			var projectSubstitutions = new ProjectSubstitutions {
				References = "<Reference Include=\"System.Net.Http\" />",
			};
			string projectPath = engine.Generate (tmpDir, projectSubstitutions, fileSubstitutions);
			ProjectBuilder.BuildProject (projectPath, "/"); /* Use system XM instead of _mac-build today */
		}
	}
}
