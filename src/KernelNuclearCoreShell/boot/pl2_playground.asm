; Kernel Nuclear Core and Shell/核核核殻 - Secondary Program Loader (Playground)
; Copyright (C) 2026 Takym.

PL2:
	BITS	16

	MOV		SI, .MSG_HELLO
	CALL	MBR.PRINT

	MOV		BP, ADDR_MBR

	; ドライブ番号表示
	MOV		SI, .MSG_DRIVE_NUM
	CALL	MBR.PRINT
	MOV		AL, [BP + IDX_DRIVE_NUM]
	CALL	.PRINT_NUM
	CALL	.PRINT_LN

	; シリンダ数表示
	MOV		SI, .MSG_CYLN_CT
	CALL	MBR.PRINT
	MOV		CX, [BP + IDX_CYLN_CT]
	MOV		AL, CH
	CALL	.PRINT_NUM
	MOV		AL, CL
	CALL	.PRINT_NUM
	CALL	.PRINT_LN

	; ヘッド数表示
	MOV		SI, .MSG_HEAD_CT
	CALL	MBR.PRINT
	MOV		AL, [BP + IDX_HEAD_CT]
	CALL	.PRINT_NUM
	CALL	.PRINT_LN

	; セクタ数表示
	MOV		SI, .MSG_SECT_CT
	CALL	MBR.PRINT
	MOV		AL, [BP + IDX_SECT_CT]
	CALL	.PRINT_NUM
	CALL	.PRINT_LN

	JMP		MBR.END

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
	CALL	MBR.PRINT           ; 文字列表示
	POP		SI                  ; SI の値をスタックから復元
	POP		CX                  ; CX の値をスタックから復元
	RET                         ; 制御を呼び出し元へ返す

.PRINT_LN:
	PUSH	SI            ; SI の値をスタックへ退避
	MOV		SI, .MSG_CRLF ; 改行文字列取得
	CALL	MBR.PRINT     ; 文字列表示
	POP		SI            ; SI の値をスタックから復元
	RET                   ; 制御を呼び出し元へ返す

.MSG:
.MSG_BYTE      DB 0x7F, 0x7F, 0x00
.MSG_CRLF      DB 0x0D, 0x0A, 0x00
.MSG_HELLO     DB "hello world", 0x0D, 0x0A, "this is kncs demo mode", 0x0D, 0x0A, 0x0D, 0x0A, 0x0D, 0x0A, 0x00
.MSG_DRIVE_NUM DB "DRIVE: 0x__", 0x00
.MSG_CYLN_CT   DB "CYLN : 0x", 0x00
.MSG_HEAD_CT   DB "HEAD : 0x__", 0x00
.MSG_SECT_CT   DB "SECT : 0x__", 0x00
