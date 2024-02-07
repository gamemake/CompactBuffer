
rm -rf ../Build
rm -rf Publish
rm -rf Output

dotnet publish ../CompactBuffer.Editor/CompactBuffer.Editor.csproj -p:UNITY_BUILD=true -o Publish

mkdir Output
cp Publish/CompactBuffer.*.dll Output
cp Files/*.meta Output

rm -rf Publish
