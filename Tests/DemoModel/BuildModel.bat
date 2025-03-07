::@ECHO off
SETLOCAL

IF "%~1" == "" ( set MODELCOMPILER="..\..\Bin\Debug\mdc.exe"
) ELSE ( 
    set MODELCOMPILER="%~1\mdc.exe" )

::set MODELCOMPILER=%cf%"\mdc"
set MODEL=DemoModel
set VERSION=v104
set EXCLUDE=Draft
set INPUT=.
set OUTPUT=generated

ECHO Building Model %MODEL%
ECHO %MODELCOMPILER% compile --spec %VERSION% --exclude %EXCLUDE% --d2 "%INPUT%\%MODEL%.xml" -c "%INPUT%\%MODEL%.csv" --o2 "%OUTPUT%"
%MODELCOMPILER% compile --spec %VERSION% --exclude %EXCLUDE% --d2 "%INPUT%\%MODEL%.xml" -c "%INPUT%\%MODEL%.csv" --o2 "%OUTPUT%"
IF %ERRORLEVEL% NEQ 0 ( ECHO Failed %MODEL% & EXIT /B 3 )