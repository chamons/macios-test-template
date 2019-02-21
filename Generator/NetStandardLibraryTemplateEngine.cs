using System;
using System.IO;

namespace Xamarin.Tests.Templating
{
	public class NetStandardLibraryTemplateEngine : TemplateEngineBase
	{
		public NetStandardLibraryTemplateEngine () : base (TemplateInfo.FromFiles ("NetStandardLib.csproj", "Class1.cs"))
		{
		}

		const string NetStandardSubDir = "NetStandard";

		public string Generate (string outputDirectory)
		{
			Directory.CreateDirectory (Path.Combine (outputDirectory, NetStandardSubDir));

			FileTemplateEngine templateEngine = CreateEngine (outputDirectory);
			templateEngine.CopyFile (TemplateInfo.SourceName);
			return templateEngine.CopyFile (TemplateInfo.ProjectName);
		}

		protected override FileTemplateEngine CreateEngine (string outputDirectory) => FileTemplateEngine.WithSubdirectory (DirectoryFinder.FindSourceDirectory (), outputDirectory, NetStandardSubDir);
	}
}
