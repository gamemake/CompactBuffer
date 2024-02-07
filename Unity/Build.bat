
del /s /f ..\Build
del /s /f Publish
del /s /f Output

dotnet publish ..\CompactBuffer.Editor\CompactBuffer.Editor.csproj -p:UNITY_BUILD=true -o Publish
IF %ERRORLEVEL% NEQ 0 goto ERROR

md Output
xcopy /y Publish\CompactBuffer.*.dll Output
xcopy /y Files\*.meta Output
del /s /f Publish

:END
set UNITY_BUILD=0
