@echo off
rem Change to current directory
cd /d "%~dp0"
cd /d "%cd%\"

MySqlBackuper.exe uninstall
MySqlBackuper.exe install
MySqlBackuper.exe start
pause