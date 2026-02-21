; Kernel Nuclear Core and Shell/核核核殻 - Master Boot Record
; Copyright (C) 2026 Takym.

ADDR_MBR         EQU 0x7C00 ; MBR の番地
BYTES_PER_SECT   EQU 0x0200 ; 1 セクタ当たりのバイト数は 512
IDX_DRIVE_NUM    EQU     -1 ; ドライブ番号
IDX_CYLN_CT      EQU     -3 ; シリンダ数
IDX_HEAD_CT      EQU     -4 ; ヘッド数
IDX_SECT_CT      EQU     -5 ; セクタ数
ATTEMPTION_LIMIT EQU     10 ; 試行回数上限

MBR:
	BITS	16
	ORG		ADDR_MBR
	JMP		.IPL
	NOP

.BPB: ; BIOS Parameter Block (https://elm-chan.org/docs/fat.html)
	; 取り敢えず空に設定する。
	TIMES	0x005A - ($ - $$) DB 0x00

.INVOKE_PL2:
	JMP		PL2
	NOP

.MSG:
.MSG_FAIL     DB "DISK SYSTEM ERROR:"
.MSG_FAIL_NUM DB "0", 0x0D, 0x0A, 0x00

.IPL: ; Initial Program Loader (https://takym.github.io/blog/general/2025/08/01/bootloader.html)
	CLI                                          ; 割り込み禁止
	XOR		AX, AX                               ; AX = 0
	MOV		ES, AX                               ; ES = 0
	MOV		SS, AX                               ; SS = 0
	MOV		DS, AX                               ; DS = 0
	MOV		SP, ADDR_MBR                         ; スタック位置を ADDR_MBR に再設定
	STI                                          ; 割り込み許可
	CLD                                          ; アドレスの増減方向の設定（常に加算モード）
	MOV		BP, SP                               ; BP = SP
	SUB		SP, -IDX_SECT_CT                     ; ドライブ情報用のスタックを確保（処理的には加算命令でも良いが、減算に確保、加算に破棄の意図を持たせている）
	MOV		[BP + IDX_DRIVE_NUM], DL             ; ドライブ番号保存
	MOV		DI, AX                               ; DI = 0（不必要？）
	MOV		AH, 0x08                             ; ドライブ情報を取得する
	INT		0x13                                 ; BIOS 関数呼び出し
	JC		.FAIL1                               ; 失敗した時
	MOV		AL, CL                               ; セクタ数取得処理
	AND		AL, 0x3F                             ; セクタ数取得処理
	SHR		CL, 6                                ; シリンダ数取得処理
	ROR		CX, 8                                ; シリンダ数取得処理
	INC		CX                                   ; シリンダ数取得処理
	INC		DH                                   ; ヘッド数取得処理
	MOV		[BP + IDX_CYLN_CT], CX               ; シリンダ数保存
	MOV		[BP + IDX_HEAD_CT], DH               ; ヘッド数保存
	MOV		[BP + IDX_SECT_CT], AL               ; セクタ数保存
	MOV		AX, (ADDR_MBR + BYTES_PER_SECT) >> 4 ; AX に読み込み先のセグメントを計算
	MOV		ES, AX                               ; ES に読み込み先のセグメントを設定
	MOV		CX, 0x02                             ; 二番目のセクタ＆最初のシリンダ
	MOV		DH, 0x00                             ; 最初のヘッド
	CALL	.READ_DISK                           ; ディスク読み込み
	MOV		AX, BYTES_PER_SECT >> 4              ; AX は 1 セクタ当たりのバイト数
	MOV		BH, 0                                ; BX の上位バイトはゼロ
	MOV		BL, [BP + IDX_SECT_CT]               ; BX の下位バイトはセクタ数
	MUL		BX                                   ; DX:AX = AX * BX
	CMP		DX, 0                                ; DX と 0 を比較
	JNE		.FAIL2                               ; 巨大なアドレスはエラー
	MOV		BX, AX                               ; 1 ループ毎にセグメントに加算する値を BX にコピー
	ADD		AX, ADDR_MBR >> 4                    ; AX に読み込み先のセグメントを計算
	MOV		ES, AX                               ; ES に読み込み先のセグメントを設定
	MOV		CX, 0                                ; 最初のシリンダ
	MOV		DH, 1                                ; ヘッド番号を 1 に設定
;	MOV		SI, [BP + IDX_CYLN_CT]               ; シリンダ数を SI にキャッシュ
	MOV		SI, 2                                ; SI に 2 以上を設定すると必ず失敗する
	MOV		DL, [BP + IDX_HEAD_CT]               ; ヘッド数を DL にキャッシュ
.READ_NEXT:
	CMP		DH, DL                               ; ヘッド番号の比較
	JB		.CALL_READ_DISK                      ; 最大ヘッド数未満なら読み込み処理開始
	MOV		DH, 0                                ; 最初のヘッドに戻る
	INC		CX                                   ; シリンダ番号加算
	CMP		CX, SI                               ; シリンダ番号の比較
	JAE		.INVOKE_PL2                          ; 二次ローダー起動
.CALL_READ_DISK:
	ROL		CX, 8                                ; シリンダ番号設定位置を調整
	SHL		CL, 6                                ; シリンダ番号を上位へ移動
	AND		CL, 0xC0                             ; 最初のセクタ（セクタ番号の部分を 0 に初期化）
	OR		CL, 0x01                             ; 最初のセクタ（セクタ番号を 1 に再設定）
	CALL	.READ_DISK                           ; ディスク読み込み
	SHR		CL, 6                                ; セクタ番号を除去し、シリンダ番号を下位へ移動
	ROR		CX, 8                                ; シリンダ番号設定位置を調整
	INC		DH                                   ; ヘッド番号加算
	CALL	.PRINT_AX                            ; デバッグ用の表示
	ADD		AX, BX                               ; AX に読み込み先のセグメントを計算
	MOV		ES, AX                               ; ES に読み込み先のセグメントを設定
	CALL	.PRINT_AX                            ; デバッグ用の表示
	CALL	.PRINT_LN                            ; デバッグ用の表示
	JMP		.READ_NEXT                           ; 次の読み込みへ移行する

.FAIL1:
	MOV		[.MSG_FAIL_NUM], "1"                 ; エラー番号を 1 に設定
	JMP		.FAIL_CORE                           ; エラーメッセージ表示処理へ
.FAIL2:
	MOV		[.MSG_FAIL_NUM], "2"                 ; エラー番号を 2 に設定
	JMP		.FAIL_CORE                           ; エラーメッセージ表示処理へ
.FAIL3:
	MOV		[.MSG_FAIL_NUM], "3"                 ; エラー番号を 3 に設定
.FAIL_CORE:
	MOV		SI, .MSG_FAIL                        ; エラーメッセージ取得
	CALL	.PRINT                               ; エラーメッセージ表示
.END:
	HLT                                          ; CPU 停止
	JMP		.END                                 ; 無限ループ

.PRINT:
	PUSH	AX                                   ; AX の値をスタックへ退避
	PUSH	BX                                   ; BX の値をスタックへ退避
	MOV		AH, 0x0E                             ; 文字を一つだけ表示する
	XOR		BX, BX                               ; 文字色とページ番号に 0 を指定
.PRINT_PUT:
	LODSB                                        ; 次の文字を読み込む
	CMP		AL, 0x00                             ; NULL 文字判定
	JE		.PRINT_END                           ; 終端を検出したら終了
	INT		0x10                                 ; BIOS 関数呼び出し
	JMP		.PRINT_PUT                           ; 次の文字へ
.PRINT_END:
	POP		BX                                   ; BX の値をスタックから復元
	POP		AX                                   ; AX の値をスタックから復元
	RET                                          ; 制御を呼び出し元へ返す

.READ_DISK:
	PUSH	AX                                   ; AX の値をスタックへ退避
	PUSH	BX                                   ; BX の値をスタックへ退避
	PUSH	DX                                   ; DX の値をスタックへ退避
	PUSH	SI                                   ; SI の値をスタックへ退避
	PUSH	BP                                   ; BP の値をスタックへ退避
	MOV		BP, ADDR_MBR                         ; BP = ADDR_MBR
	MOV		AL, [BP + IDX_SECT_CT]               ; セクタ数取得
	MOV		BH, CL                               ; セクタ番号を BH にコピー
	AND		BH, 0x3F                             ; 上位 2 ビットにあるシリンダ番号を除去
	DEC		BH                                   ; セクタ番号は 1 始まりなので減算して調整
	SUB		AL, BH                               ; 読み込むセクタ数からセクタ番号を引く
	XOR		BX, BX                               ; BX に読み込み先のアドレスを設定
	XOR		SI, SI                               ; 試行回数はゼロ
.READ_DISK_RETRY:
	MOV		AH, 0x02                             ; ディスクからデータを読み込む
	MOV		DL, [BP + IDX_DRIVE_NUM]             ; ドライブ番号再設定
	INT		0x13                                 ; BIOS 関数呼び出し
	JNC		.READ_DISK_END                       ; 成功したら終了
	INC		SI                                   ; 試行回数加算
	CMP		SI, ATTEMPTION_LIMIT                 ; 試行回数上限と比較
;	JAE		.FAIL3                               ; 失敗した時
	JAE		.READ_DISK_SKIP_ERROR                ; エラーを無視する
	MOV		AH, 0x00                             ; ドライブ再設定
	MOV		DL, [BP + IDX_DRIVE_NUM]             ; ドライブ番号再設定
	INT		0x13                                 ; BIOS 関数呼び出し
	JMP		.READ_DISK_RETRY                     ; 再試行
.READ_DISK_SKIP_ERROR:
	MOV		[.MSG_FAIL_NUM], "3"                 ; エラー番号を 3 に設定
	MOV		SI, .MSG_FAIL                        ; エラーメッセージ取得
	CALL	.PRINT                               ; エラーメッセージ表示
.READ_DISK_END:
	POP		BP                                   ; BP の値をスタックから復元
	POP		SI                                   ; SI の値をスタックから復元
	POP		DX                                   ; DX の値をスタックから復元
	POP		BX                                   ; BX の値をスタックから復元
	POP		AX                                   ; AX の値をスタックから復元
	RET                                          ; 制御を呼び出し元へ返す


; DEBUG CODE BEGIN

.PRINT_AX:
	ROL		AX, 8
	CALL	.PRINT_NUM
	ROR		AX, 8
	CALL	.PRINT_NUM
	CALL	.PRINT_LN
	RET

.PRINT_NUM:
	PUSH	CX                  ; CX の値をスタックへ退避
	PUSH	SI                  ; SI の値をスタックへ退避
	MOV		CL, AL              ; 数値取得
	SHR		CL, 4               ; 上位の数値を取得
	OR		CL, 0x30            ; 文字コードに変換
	CMP		CL, 0x3A            ; もし CL < 0x3A ならば
	JB		.PRINT_NUM_SKIP1    ; 次の処理を飛ばす
	ADD		CL, 7               ; CL += 7
.PRINT_NUM_SKIP1:
	MOV		[.MSG_BYTE + 0], CL ; 上位の数字を設定
	MOV		CL, AL              ; 数値取得
	AND		CL, 0x0F            ; 下位の数値を取得
	OR		CL, 0x30            ; 文字コードに変換
	CMP		CL, 0x3A            ; もし CL < 0x3A ならば
	JB		.PRINT_NUM_SKIP2    ; 次の処理を飛ばす
	ADD		CL, 7               ; CL += 7
.PRINT_NUM_SKIP2:
	MOV		[.MSG_BYTE + 1], CL ; 下位の数字を設定
	MOV		SI, .MSG_BYTE       ; 文字列取得
	CALL	.PRINT              ; 文字列表示
	POP		SI                  ; SI の値をスタックから復元
	POP		CX                  ; CX の値をスタックから復元
	RET                         ; 制御を呼び出し元へ返す

.PRINT_LN:
	PUSH	SI            ; SI の値をスタックへ退避
	MOV		SI, .MSG_CRLF ; 改行文字列取得
	CALL	.PRINT        ; 文字列表示
	POP		SI            ; SI の値をスタックから復元
	RET                   ; 制御を呼び出し元へ返す

.MSG_BYTE      DB 0x7F, 0x7F, 0x00
.MSG_CRLF      DB 0x0D, 0x0A, 0x00

; DEBUG CODE ENDED


;	TIMES	0x01BE - ($ - $$) DB 0x00

.PT: ; Partition Table (https://wiki.osdev.org/Partition_Table)
	; 取り敢えず空に設定する。
	TIMES	0x01FE - ($ - $$) DB 0x00

.BOOT_SIGN:
	DW		0xAA55
