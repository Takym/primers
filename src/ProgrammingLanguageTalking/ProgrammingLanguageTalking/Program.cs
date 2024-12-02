/****
 * プログラミング言語会話
 * Copyright (C) 2024 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using ProgrammingLanguageTalking.Examples;

namespace ProgrammingLanguageTalking
{
	internal static class Program
	{
		[STAThread()]
		private static int Main(string[] args)
		{
			try {
				DemoApp.Start(args);
				return 0;
			} catch (Exception e) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Error.WriteLine();
				Console.Error.WriteLine(e.ToString());
				Console.ResetColor();
				return e.HResult;
			}
		}

#if DEBUG
		private static class DebugEnvironment
		{
			[Conditional("DEBUG")]
			[STAThread()]
			private static void Main() => DemoApp.Start();
		}
#endif
	}
}
