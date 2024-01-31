
if "%%1" == "" (
    echo "invalid argument"
    goto END
)

if exist "%%1" (
    del /s /q "%%1"
)

md "%%1"
md "%%1\CompactBuffer"

xcopy /y Output\*.* "%%1"
xcopy /y ..\CompactBuffer.Tests\Types*.cs "%%1\CompactBuffer.Tests"
xcopy /y Files\CompactBuffer.Tests.asmdef "%%1\CompactBuffer.Tests"

:END
