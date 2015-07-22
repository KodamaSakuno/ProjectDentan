@echo off
if "%1" == "" goto noargument

if not exist Dentan.Browser\bin\%1\Dentan.Browser.exe goto noexec
if not exist bin\%1\Dentan.Browser.exe copy Dentan.Browser\bin\%1\Dentan.Browser.exe bin\%1\
goto done

:noexec
echo Please compile...
goto done

:noargument
echo No argument...
goto done

:done