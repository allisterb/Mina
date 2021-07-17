@echo off
@setlocal
set ERROR_CODE=0

cd src\Mina.CLI\bin\Debug\netcoreapp3.1\
"Mina.CLI.exe" %*
goto end

:end
cd ..\..\..\..\..\
exit /B %ERROR_CODE%