
rm -rf ../Build
rm -rf Publish
rm -rf Output

export UNITY_BUILD=1
dotnet publish ../CompactBuffer.Editor/CompactBuffer.Editor.csproj -o Publish
export UNITY_BUILD=0

mkdir Output
cp Publish/CompactBuffer.*.dll Output
cp Metas/*.meta Output

rm -rf Publish

