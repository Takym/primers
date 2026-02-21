@REM Kernel Nuclear Core and Shell/核核核殻 - Serve Script
@REM Copyright (C) 2026 Takym.
@echo off

pushd "%~dp0"

call build.cmd
wsl -d Ubuntu-24.04 -- qemu-system-x86_64 -drive if=floppy,file=disk.vfd,index=0,media=disk
@REM x86_64 の代わりに amd64 も使用できる。

popd
