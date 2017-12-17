@echo off

rem                 Please Note:
rem                   - Should be inside the src/bin folder for it to work

rem Getting the version from the user
set /p VERSION="Version: "

rem Setting constants
set BASE_FILES=..\serverfiles
set CLIENT_FILES=Client\Release
set SERVER_FILES=Server\Release
set DUMP_FILES=DumpUnloader\Release
set MAIN_FILES=important

set DEL_FILES=*.pdb *.config *.zip CitizenFX.Core.dll

rem Creating sub folders
IF NOT EXIST %VERSION% mkdir %VERSION%
cd %VERSION%
IF NOT EXIST "Terminal" mkdir "Terminal"
IF NOT EXIST "Dump Unloader" mkdir "Dump Unloader"
IF NOT EXIST "FiveM Resource" mkdir "FiveM Resource"
cd "FiveM Resource"
echo . > "^ Resource Folder"
echo . > "Now works with any resource name ;)"
IF NOT EXIST "dispatchsystem" mkdir "dispatchsystem"

rem Going back to the main folder
cd ..\..

rem Copying files (I think)
echo Press any key to continue to copy the files
pause > nul
xcopy /e /v /c /q /y %BASE_FILES% "%VERSION%\FiveM Resource\dispatchsystem" > nul
xcopy /e /v /c /q /y %SERVER_FILES% "%VERSION%\FiveM Resource\dispatchsystem" > nul
xcopy /e /v /c /q /y %DUMP_FILES% "%VERSION%\Dump Unloader" > nul
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
