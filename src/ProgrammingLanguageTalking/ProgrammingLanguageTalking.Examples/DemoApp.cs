/****
 * プログラミング言語会話
 * Copyright (C) 2024 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgrammingLanguageTalking.Examples
{
	public static class DemoApp
	{
		public static void Start(string[] args)
		{
			"Hello, World".Print();
			00000123456789.Print();

			GodExtensions.Default.PrayFor(GodExtensions.GratefulText);

			for (int i = 0; i < args.Length; ++i) {
				$"{nameof(args)}[{i:D10}]: {args[i]}".Print();
			}

			var ctx = new RootContext("根幹");
			var mca = new MyCustomAgent();

			mca.MakeDecision(NullAgent.Instance, ctx, NullDecision.Instance)?.ToString().Print();
		}

		public static void Start()
		{
			Start(InputArgs().ToArray());

			static IEnumerable<string> InputArgs()
			{
				Console.WriteLine("コマンド行引数を一つずつ入力してください。");
				Console.WriteLine("引数の入力を終了し、デモアプリを起動するには、空行を入力してください。");

				int i = 0;
				while (true) {
					Console.Write("{0,10}番目の引数：", i++);
					string? line = Console.ReadLine();

					if (string.IsNullOrEmpty(line)) {
						Console.WriteLine("空行が入力された為、デモアプリを起動します。");
						yield break;
					} else {
						yield return line;
					}
				}
			}
		}
	}
}
