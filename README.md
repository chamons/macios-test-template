# Test template prototype for xamarin-macios

Today testing msbuild projects in macios is divided into two seperate systems:

- Xamarin.iOS uses in-process msbuild tests that are checked into source control.
- Xamarin.Mac uses out-of-process msbuild tests that are generated from the tests.

I prefer the Xamarin.Mac solution, however the infrastructure used to generate 
those tests was organically grown (overgrew) over time and needs a serious cleanup pass.

In addition, it was hard coded strongly towards Xamarin.Mac'isms. 

This project is a prototype for an alternative that could be shared and more easily extended.

## How to use the API

- You instance a TemplateEngine subclass either with a TemplateInfo or your flavor\language
    * MacAppTemplateEngine
    * MacBindingTemplateEngine
    * MacClassicAppTemplateEngine
    * MacLibraryTemplateEngine
    * MacSystemMonoTemplateEngine
    * NetStandardTemplateEngine

- Create a number of Substitutions classes (FileSubstitutions, PListSubstitutions, ProjectSubstitutions) which describe how you want to fill in the "holes" in the templates.

- You call a Generate method passing in those substitutions and any additional options.

### Example

```csharp
var tmpDir = Cache.CreateTemporaryDirectory ();

var engine = new MacAppTemplateEngine (ProjectFlavor.FullXM, ProjectLanguage.CSharp);
var fileSubstitutions = new FileSubstitutions { TestCode = "System.Console.WriteLine (typeof (int));" };
var projectSubstitutions = new ProjectSubstitutions {
	References = "<Reference Include=\"System.Net.Http\" />",
};

string projectPath = engine.Generate (tmpDir, projectSubstitutions, fileSubstitutions);
ProjectBuilder.BuildProject (projectPath, "/"); /* Use system XM instead of _mac-build today */
```

