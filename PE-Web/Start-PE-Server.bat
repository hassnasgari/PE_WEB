@echo off
chcp 65001 >nul
title PE Web Server
cd /d "%~dp0"
echo.
echo ======================================================
echo    PE Web Server (Ver 0009)
echo ======================================================
echo.
echo  Website: http://localhost:3000
echo.
echo ======================================================
echo.
start "" cmd /c "timeout /t 2 /nobreak >nul & start http://localhost:3000"
node server.js
pause
