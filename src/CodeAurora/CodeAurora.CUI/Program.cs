using System;
using System.Collections.Generic;
using CodeAurora.CUI;

var helpCmd = new HelpCommand();
var cmds    = helpCmd.Commands;

cmds.Add(helpCmd);
cmds.Add(new GreetCommand());
cmds.Add(new TimeCommand ());
cmds.Add(new ArgsCommand ());

if (args.Length > 0) {
	string cmd = args[0].ToLower();

	// help コマンドによって all が予約されているので、all コマンドを実行しようとしたらエラーとする。
	if (cmd == "all") {
		Console.WriteLine("「all」はコマンドではありません。");
		return;
	}

	int count = cmds.Count;
	for (int i = 0; i < count; ++i) {
		if (cmds[i].Name == cmd) {
			cmds[i].Execute(args[1..]);
			return;
		}
	}

	Console.WriteLine($"指定されたコマンド「{cmd}」は見つかりませんでした。");
} else {
	Console.WriteLine("引数が指定されていません。");
	helpCmd.Execute([]);
}

/* // 古いコード

// コマンドライン引数の解析を追加
if (args.Length > 0) {
	// 引数に基づいて面白い処理を追加
	string arg0 = args[0].ToLowerInvariant();
	switch (arg0) {
	case "help":
		Console.WriteLine("使用可能なコマンド: greet, time, help");
		break;
	case "greet":
		Console.WriteLine("こんにちは！CodeAuroraへようこそ！");
		break;
	case "time":
		Console.WriteLine($"現在の時刻は {DateTime.Now} です。");
		break;
	case "args":
		Console.WriteLine("コマンドライン引数:");
		for (int i = 1; i < args.Length; ++i){
			Console.WriteLine($"- {args[i]}");
		}
		break;
	default:
		Console.WriteLine($"コマンド '{arg0}' は認識されませんでした。");
		break;
	}
} else {
	Console.WriteLine("引数が指定されていません。");
}

//*/
