using System;
namespace Xamarin.Tests.Templating
{
	public class MacSystemMonoTemplateEngine : TemplateEngineBase
	{
		public MacSystemMonoTemplateEngine () : base (TemplateInfo.FromFiles ("SystemMonoExample.csproj", "Main.cs"))
		{
		}

		public MacSystemMonoTemplateEngine (TemplateInfo info) : base (info)
		{
		}

		public string Generate (string outputDirectory, ProjectSubstitutions projectSubstitutions, FileSubstitutions fileSubstitutions)
		{
			FileCopier templateEngine = CreateEngine (outputDirectory);

			ReplacementGroup replacements = ReplacementGroup.Create (Replacement.Create ("%CODE%", fileSubstitutions.TestCode), Replacement.Create ("%DECL%", fileSubstitutions.TestDecl));
			templateEngine.CopyTextWithSubstitutions (MacAppTemplateEngine.GetAppMainSourceText (ProjectLanguage.CSharp), TemplateInfo.SourceName, replacements);

			templateEngine.CopyFile ("Info-Unified.plist", "Info.plist");

			return templateEngine.CopyFileWithSubstitutions (TemplateInfo.ProjectName, GetStandardProjectReplacement (projectSubstitutions));
		}
	}
}
