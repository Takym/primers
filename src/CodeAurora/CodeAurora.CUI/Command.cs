using System;
using System.Collections.Generic;

namespace CodeAurora.CUI
{
	// 抽象コマンドクラス
	public abstract class Command
	{
		public abstract string Name { get; }

		public virtual string Description => "コマンドの説明はありません。";

		public abstract void Execute(string[] args);

		public virtual void ShowHelp(string[] args)
		{
			// 既定の説明文を表示する。コマンド名を含まなければならない。
			Console.WriteLine($"コマンド「{this.Name}」の使い方");
			Console.WriteLine(this.Description);
		}
	}

	// コマンドの説明書
	public class HelpCommand : Command
	{
		public override string Name => "help";

		public override string Description => "このコマンドは、使用可能なコマンドの一覧を表示します。";

		public List<Command> Commands { get; }

		public HelpCommand()
		{
			this.Commands = [];
		}

		public override void Execute(string[] args)
		{
			if (args.Length > 0) {
				string cmd = args[0].ToLower();

				int count = this.Commands.Count;
				for (int i = 0; i < count; ++i) {
					if (this.Commands[i].Name == cmd) {
						this.Commands[i].ShowHelp(args[1..]);
						return;
					}
				}

				if (cmd == "all") {
					Console.WriteLine("使用可能なコマンド：");

					for (int i = 0; i < count; ++i) {
						this.Commands[i].ShowHelp([]);
						Console.WriteLine();
					}

					Console.WriteLine("使用不能なコマンド：");
					Console.WriteLine("コマンド「all」の使い方");
					Console.WriteLine("「all」はコマンドではありません。");
					Console.WriteLine("このコマンドは、help コマンドで全てのコマンドを表示する為に予約されています。");
					Console.WriteLine();
				} else {
					Console.WriteLine($"指定されたコマンド「{cmd}」は見つかりませんでした。");
				}
			} else {
				Console.WriteLine("使用可能なコマンド：");

				int count = this.Commands.Count;
				for (int i = 0; i < count; ++i) {
					var cmd = this.Commands[i];
					Console.WriteLine($"- {cmd.Name}: {cmd.Description}");
				}

				Console.WriteLine("使用不能なコマンド：");
				Console.WriteLine("- all: このコマンドは、help コマンドで全てのコマンドを表示する為に予約されています。");
			}
		}

		public override void ShowHelp(string[] args)
		{
			base.ShowHelp(args);

			// 引数が指定されていない場合の動作説明を表示する。
			Console.WriteLine("引数が指定されていない場合、使用可能なコマンドの一覧を表示します。");

			// コマンド名が引数に指定された場合の動作説明を表示する。
			Console.WriteLine("コマンド名が指定された場合、そのコマンドの使い方を表示します。");

			// 追加の引数が指定された場合の動作説明を表示する。
			Console.WriteLine("更に引数を追加する事で、詳細な使い方を表示できる場合があります。");

			// 「all」が引数に指定された場合の動作説明を表示する。
			Console.WriteLine("「all」が指定された場合、全てのコマンドの使い方を表示します。");
		}
	}

	// サンプルコマンド: GreetCommand
	public class GreetCommand : Command
	{
		public override string Name => "greet";

		public override string Description => "このコマンドは、挨拶を表示します。";

		public override void Execute(string[] args)
			=> Console.WriteLine("こんにちは！CodeAuroraへようこそ！");
	}

	// サンプルコマンド: TimeCommand
	public class TimeCommand : Command
	{
		public override string Name => "time";

		public override string Description => "このコマンドは、現在の時刻を表示します。";

		public override void Execute(string[] args)
			=> Console.WriteLine($"現在の時刻は {DateTime.Now} です。");
	}

	// サンプルコマンド: ArgsCommand
	public class ArgsCommand : Command
	{
		public override string Name => "args";

		public override string Description => "このコマンドは、コマンドライン引数を表示します。";

		public override void Execute(string[] args)
		{
			if (args.Length > 0) {
				string mode = args[0].ToLower();

				switch (mode) {
				case "reverse":
					Reverse(args[1..]);
					break;
				case "normal":
					Normal(args[1..]);
					break;
				default:
					Normal(args);
					break;
				}
			} else {
				Console.WriteLine("コマンドライン引数が指定されていません。");
			}

			void Normal(string[] args)
			{
				Console.WriteLine("コマンドライン引数（正順）：");
				for (int i = 0; i < args.Length; ++i) {
					Console.WriteLine($"- [{i}]: {args[i]}");
				}
			}

			void Reverse(string[] args)
			{
				Console.WriteLine("コマンドライン引数（逆順）：");
				for (int i = args.Length - 1; i >= 0; --i) {
					Console.WriteLine($"- [{i}]: {args[i]}");
				}
			}
		}

		public override void ShowHelp(string[] args)
		{
			base.ShowHelp(args);

			// 逆順で表示する場合の引数の指定方法を表示する。
			Console.WriteLine("「args」の次の引数に「reverse」を指定すると、逆順で表示します。");

			// 正順で表示する場合の引数の指定方法を表示する。
			Console.WriteLine("「reverse」の指定が無い場合、通常の動作として正順で表示します。");
			Console.WriteLine("明示的に正順として表示する場合、「args」の次の引数に「normal」を指定します。");
		}
	}
}
