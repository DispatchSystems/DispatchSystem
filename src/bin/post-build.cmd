@echo off

rem                 Please Note:
rem                   - Should be inside the src/bin folder for it to work

rem Getting the version from the user
set /p VERSION="Version: "

rem Setting constants
set BASE_FILES=..\serverfiles
set CLIENT_FILES=Terminal\Release
set SERVER_FILES=FiveM.Server\Release
set DUMP_FILES=Dump.Client\Release
set MAIN_FILES=important

set DEL_FILES=*.pdb *.config *.zip CitizenFX.Core.dll

rem Creating sub folders
IF NOT EXIST %VERSION% mkdir %VERSION%
cd %VERSION%
IF NOT EXIST "Terminal" mkdir "Terminal"
IF NOT EXIST "Dump Client" mkdir "Dump Client"
IF NOT EXIST "FiveM Resources" mkdir "FiveM Resources"
cd "FiveM Resources"
echo . > "^ Resource Folders"

rem Going back to the main folder
cd ..\..

rem Copying files (I think)
echo Press any key to continue to copy the files

xcopy /e /v /c /q /y "%BASE_FILES%" "%VERSION%\FiveM Resources" > nul

pause > nul
xcopy /e /v /c /q /y %SERVER_FILES% "%VERSION%\FiveM Resources\dispatchsystem" > nul
xcopy /e /v /c /q /y %DUMP_FILES% "%VERSION%\Dump Client" > nul
xcopy /e /v /c /q /y %CLIENT_FILES% "%VERSION%\Terminal" > nul

rem Clean up
cd %VERSION%
del /s /q %DEL_FILES% > nul

rem Copying final bits after the clearing
cd ..
xcopy /e /v /c /q /y "%MAIN_FILES%" "%VERSION%" > nul

echo DONE!
pause
EXIT
