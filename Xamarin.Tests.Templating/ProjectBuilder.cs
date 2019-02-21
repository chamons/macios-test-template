using System;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Utils;

namespace Xamarin.Tests.Templating
{
	public static class ProjectBuilder
	{
		static Func<string> CreateMoreInfoFunction (string csprojTarget)
		{
			return () => {
				string csprojText = "\n\n\n\tCSProj: \n" + File.ReadAllText (csprojTarget);
				string csprojLocation = Path.GetDirectoryName (csprojTarget);
				string fileList = "\n\n\tFiles: " + String.Join (" ", Directory.GetFiles (csprojLocation).Select (x => x.Replace (csprojLocation + "/", "")));
				return csprojText + fileList;
			};
		}

		static void SetEnvironment (string rootDirectory)
		{
			Environment.SetEnvironmentVariable ("TargetFrameworkFallbackSearchPaths", rootDirectory + "/Library/Frameworks/Mono.framework/External/xbuild-frameworks");
			Environment.SetEnvironmentVariable ("MSBuildExtensionsPathFallbackPathsOverride", rootDirectory + "/Library/Frameworks/Mono.framework/External/xbuild");
			Environment.SetEnvironmentVariable ("XAMMAC_FRAMEWORK_PATH", rootDirectory + "/Library/Frameworks/Xamarin.Mac.framework/Versions/Current");
			Environment.SetEnvironmentVariable ("XamarinMacFrameworkRoot", rootDirectory + "/Library/Frameworks/Xamarin.Mac.framework/Versions/Current");
		}

		public static string BuildProject (string csprojTarget, string rootDirectory, bool shouldFail = false, bool release = false, string[] environment = null)
		{
			SetEnvironment (rootDirectory);

			StringBuilder buildArgs = new StringBuilder ();
			buildArgs.Append (" /verbosity:diagnostic ");
			buildArgs.Append (" /property:XamarinMacFrameworkRoot=" + rootDirectory + "/Library/Frameworks/Xamarin.Mac.framework/Versions/Current ");

			buildArgs.Append ($"/property:Configuration={(release ? "Release" : "Debug")} ");

			buildArgs.Append (StringUtils.Quote (csprojTarget));

			return ProcessInvoker.RunAndAssert ("/Library/Frameworks/Mono.framework/Commands/msbuild", buildArgs, "Compile", shouldFail, CreateMoreInfoFunction (csprojTarget), environment);
		}

		public static string BuildClassicProject (string csprojTarget, string rootDirectory, bool shouldFail = false, string[] environment = null)
		{
			SetEnvironment (rootDirectory);

			StringBuilder buildArgs = new StringBuilder ();
			buildArgs.Append (" build ");
			buildArgs.Append (StringUtils.Quote (csprojTarget));
			
			return ProcessInvoker.RunAndAssert ("/Applications/Visual Studio.app/Contents/MacOS/vstool", buildArgs, "Compile", shouldFail, CreateMoreInfoFunction (csprojTarget), environment);
		}
	}
}
