
if "%%1" == "" (
    echo "invalid argument"
    goto END
)

if exist "%%1" (
    del /s /f "%%1\CompactBuffer.Runtime"
    del /s /f "%%1\CompactBuffer.Runtime\Internal"
    del /s /f "%%1\CompactBuffer.Generator"
    del /s /f "%%1\CompactBuffer.Editor"
    del /s /f "%%1\CompactBuffer.Tests"

) else (
    mkdir "%%1"
)

md "%%1\CompactBuffer.Runtime"
md "%%1\CompactBuffer.Runtime\Internal"
md "%%1\CompactBuffer.Generator"
md "%%1\CompactBuffer.Editor"
md "%%1\CompactBuffer.Tests"

copy ../CompactBuffer.Runtime/*.cs "${TARGET_DIR}/CompactBuffer.Runtime"
copy ../CompactBuffer.Runtime/Internal/*.cs "${TARGET_DIR}/CompactBuffer.Runtime/Internal"
copy ../CompactBuffer.Generator/*.cs "${TARGET_DIR}/CompactBuffer.Generator"
copy ../CompactBuffer.Editor/*.cs "${TARGET_DIR}/CompactBuffer.Editor"
copy ../CompactBuffer.Tests/Types*.cs "${TARGET_DIR}/CompactBuffer.Tests"

copy Files/CompactBuffer.Runtime.asmdef "${TARGET_DIR}/CompactBuffer.Runtime"
copy Files/CompactBuffer.Generator.asmdef "${TARGET_DIR}/CompactBuffer.Generator"
copy Files/CompactBuffer.Editor.asmdef "${TARGET_DIR}/CompactBuffer.Editor"
copy Files/CompactBuffer.Tests.asmdef "${TARGET_DIR}/CompactBuffer.Tests"
