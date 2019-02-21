using System;
using System.Text;
using NUnit.Framework;

namespace Xamarin.Tests.Templating
{
	public static class ProcessInvoker
	{
		public static string RunAndAssert (string exe)
		{
			var parts = exe.Split (new char[] { ' ' }, 2);
			if (parts.Length == 1)
				return RunAndAssert (exe, "", "Command: " + exe);
			else
				return RunAndAssert (parts[0], parts[1], "Command: " + exe);
		}

		public static string RunAndAssert (string exe, StringBuilder args, string stepName, bool shouldFail = false, Func<string> getAdditionalFailInfo = null, string[] environment = null)
		{
			return RunAndAssert (exe, args.ToString (), stepName, shouldFail, getAdditionalFailInfo, environment);
		}

		public static string RunAndAssert (string exe, string args, string stepName, bool shouldFail = false, Func<string> getAdditionalFailInfo = null, string[] environment = null)
		{
			StringBuilder output = new StringBuilder ();
			Environment.SetEnvironmentVariable ("MONO_PATH", null);
			int compileResult = Invoker.RunCommand (exe, args != null ? args.ToString () : string.Empty, environment, output, suppressPrintOnErrors: shouldFail);
			if (!shouldFail && compileResult != 0) {
				// Driver.RunCommand won't print failed output unless verbosity > 0, so let's do it ourselves.
				Console.WriteLine ($"Execution failed; exit code: {compileResult}");
				Console.WriteLine (output);
			}
			Func<string> getInfo = () => getAdditionalFailInfo != null ? getAdditionalFailInfo () : "";
			if (!shouldFail)
				Assert.AreEqual (0, compileResult, stepName + " failed:\n\n'" + output + "' " + exe + " " + args + getInfo ());
			else
				Assert.AreNotEqual (0, compileResult, stepName + " did not fail as expected:\n\n'" + output + "' " + exe + " " + args + getInfo ());

			return output.ToString ();
		}
	}
}
