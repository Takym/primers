
// 画面に文字列を表示する。
Console.WriteLine("こんにちは！ゲームへようこそ！");

// 変数 userName を宣言する。
string? userName;

// 利用者の名前を入力する。
Console.Write("あなたの名前を入力してください：");
userName = Console.ReadLine();

// もし利用者名が空であれば、そのまま終了する。
if (string.IsNullOrEmpty(userName)) {
	Console.WriteLine("何も入力されなかった為、ゲームを終了します。");

	// 異常終了なので -1 を返す。
	return -1;
}

// 変数 userHP を定義する。
int userHP = 20;

// 敵に関する情報を定義する。
string enemyName = "敵A";
int enemyHP = 15;

// キャラクターの情報を表示する関数を定義する。
static void PrintStatus(string name, int HP)
	=> Console.WriteLine($"現在の{name}さんの体力は{HP}です。");

// 現在の状態を表示する。
PrintStatus(userName,  userHP );
PrintStatus(enemyName, enemyHP);

// キー入力を待機する。
Console.Write($"何かキーを押すと{userName}さんと{enemyName}さんが相互に攻撃し合います。");
Console.ReadKey(true);
Console.WriteLine();

// 相互に攻撃する。
const int power = 10;

userHP  -= power;
enemyHP -= power;

Console.WriteLine($"両者とも体力が{power}だけ減少しました。");

// 現在の状態を表示する。
PrintStatus(userName,  userHP );
PrintStatus(enemyName, enemyHP);

// 正常時は常に 0 を返す。
return 0;
