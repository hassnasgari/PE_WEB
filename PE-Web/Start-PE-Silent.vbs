Set WshShell = CreateObject("WScript.Shell")
WshShell.CurrentDirectory = "c:\Users\ASG\Desktop\0-Programing\20-PE_WEB\PE-Web"
WshShell.Run "node server.js", 0, False
WScript.Sleep 1000
WshShell.Run "http://localhost:3000"
