# Kernel Nuclear Core and Shell/核核核殻 - ブートローダー
Copyright (C) 2026 Takym.

## メモ
* かなり汎用的に実装できたと思う。
* [`mbr_debug.asm`](./mbr_debug.asm) と [`pl2_playground.asm`](./pl2_playground.asm) を介してデバッグを行っている。
* MBR でシリンダ番号が 1 以上の時に読み込みに失敗してしまう。
	* 原因・修正方法は次の通り判明した。
		* →64KB の境界を超えて読み込む事はできない。
			* `0xDC00 + 0x2400 = 0x1_0000` 可能
			* `0xE800 + 0x2400 = 0x1_0C00` 不能
		* →64KB の境界を超える場合に分割して読み込む様にした。
