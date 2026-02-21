# Kernel Nuclear Core and Shell/核核核殻 - ブートローダー
Copyright (C) 2026 Takym.

## メモ
* かなり汎用的に実装できたと思う。
* [`pl2_playground.asm`](./pl2_playground.asm) を介してデバッグを行っている。
* MBR でシリンダ番号が 1 以上の時に読み込みに失敗してしまう。
	* セグメントレジスタの計算ミスはしていないと思われる。
	* 原因・修正方法は依然として不明。
