; Kernel Nuclear Core and Shell - Master Boot Record
; Copyright (C) 2026 Takym.

ADDR_MBR EQU 0x7C00

MBR:
	BITS	16
	ORG		ADDR_MBR
	JMP		.IPL
	NOP

.BPB:
.IPL:
.PT:

.BOOT_SIGN:
	TIMES	0x01FE - ($ - $$) DB 0x00
	DW		0xAA55
