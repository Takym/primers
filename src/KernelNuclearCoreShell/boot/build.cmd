@REM Kernel Nuclear Core and Shell - Build Script
@REM Copyright (C) 2026 Takym.
@echo off
pushd "%~dp0"

nasm disk.asm -o disk.vfd -l disk.lst -f bin

popd
