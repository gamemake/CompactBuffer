
if [ $# -ne 1 ]; then
    echo "invalid argument"
    exit 1
fi

TARGET_DIR="$1"

if [ ! -d "${TARGET_DIR}" ]; then mkdir "${TARGET_DIR}"; fi
rm -rf "${TARGET_DIR}/CompactBuffer.Runtime"
rm -rf "${TARGET_DIR}/CompactBuffer.Runtime/Internal"
rm -rf "${TARGET_DIR}/CompactBuffer.Generator"
rm -rf "${TARGET_DIR}/CompactBuffer.Editor"
rm -rf "${TARGET_DIR}/CompactBuffer.Tests"

mkdir "${TARGET_DIR}/CompactBuffer.Runtime"
mkdir "${TARGET_DIR}/CompactBuffer.Runtime/Internal"
mkdir "${TARGET_DIR}/CompactBuffer.Generator"
mkdir "${TARGET_DIR}/CompactBuffer.Editor"
mkdir "${TARGET_DIR}/CompactBuffer.Tests"

cp ../CompactBuffer.Runtime/*.cs "${TARGET_DIR}/CompactBuffer.Runtime"
cp ../CompactBuffer.Runtime/Internal/*.cs "${TARGET_DIR}/CompactBuffer.Runtime/Internal"
cp ../CompactBuffer.Generator/*.cs "${TARGET_DIR}/CompactBuffer.Generator"
cp ../CompactBuffer.Editor/*.cs "${TARGET_DIR}/CompactBuffer.Editor"
cp ../CompactBuffer.Tests/Types*.cs "${TARGET_DIR}/CompactBuffer.Tests"

cp Files/CompactBuffer.Runtime.asmdef "${TARGET_DIR}/CompactBuffer.Runtime"
cp Files/CompactBuffer.Generator.asmdef "${TARGET_DIR}/CompactBuffer.Generator"
cp Files/CompactBuffer.Editor.asmdef "${TARGET_DIR}/CompactBuffer.Editor"
cp Files/CompactBuffer.Tests.asmdef "${TARGET_DIR}/CompactBuffer.Tests"
