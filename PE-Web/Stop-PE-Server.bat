@echo off
title Stop PE Web Server
echo Closing Node.js PE Server...
for /f "tokens=5" %%a in ('netstat -aon ^| findstr ":3000" ^| findstr "LISTENING"') do taskkill /F /PID %%a
echo Server Stopped.
timeout /t 2 >nul
