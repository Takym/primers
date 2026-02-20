; Kernel Nuclear Core and Shell - Floppy Disk
; Copyright (C) 2026 Takym.

%INCLUDE "mbr.asm"
%INCLUDE "pl2.asm"

TIMES 0x00168000 - ($ - $$) DB 0x00
