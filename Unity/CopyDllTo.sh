
if [ $# -ne 1 ]; then
    echo "invalid argument"
    exit 1
fi

TARGET_DIR="$1"

if [ -d "${TARGET_DIR}" ]; then
    rm -rf "${TARGET_DIR}"
fi
    mkdir "${TARGET_DIR}"

mkdir "${TARGET_DIR}/CompactBuffer.Tests"

cp Output/* "${TARGET_DIR}"
cp ../CompactBuffer.Tests/Types*.cs "${TARGET_DIR}/CompactBuffer.Tests"
cp Files/CompactBuffer.Tests.asmdef "${TARGET_DIR}/CompactBuffer.Tests"
