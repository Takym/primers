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

			(GodExtensions.Default                    ).Pray();
			(GodExtensions.Default = KamiSama.Instance).Pray();

			for (int i = 0; i < args.Length; ++i) {
				$"{nameof(args)}[{i:D10}]: {args[i]}".Print();
			}

			var ctx  = new RootContext("根幹");
			var mcaA = new MyCustomAgent("A");
			var mcaB = new MyCustomAgent("B");

			// 注意：Decision.SendMessage は OnMessageReceived の内部で使う事を想定。
			// Agent.MakeDecision は外部で使う事を想定。

			// TODO: 後でループに置き換える。
			var d0 = mcaA.MakeDecision(NullAgent.Instance, ctx,       NullDecision.Instance);
			var d1 = mcaB.MakeDecision(mcaA,               ctx, d0 ?? NullDecision.Instance);
			var d2 = mcaA.MakeDecision(mcaB,               ctx, d1 ?? NullDecision.Instance);
			var d3 = mcaB.MakeDecision(mcaA,               ctx, d2 ?? NullDecision.Instance);
			var d4 = mcaB.MakeDecision(mcaA,               ctx, d3 ?? NullDecision.Instance);
			var d5 = mcaB.MakeDecision(mcaA,               ctx, d4 ?? NullDecision.Instance);

			d0?.ToString().Print();
			d1?.ToString().Print();
			d2?.ToString().Print();
			d3?.ToString().Print();
			d4?.ToString().Print();
			d5?.ToString().Print();

			/* 記事執筆用のメモ
			・プロジェクト構成の説明
			・あくまで人間同士の会話を想定している。
			・コンピュータとの会話でも翻訳システムでもない。
			・コードが日本語の文章に翻訳される訳ではない。
			*/
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
