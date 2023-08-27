#!/bin/bash

# Check if directory path argument is provided
if [ -z "$1" ]; then
  echo "Please provide an output directory path as the first argument."
  exit 1
fi

dotnet publish -r linux-x64 -c Release
dotnet publish -r linux-musl-x64 -c Release
dotnet publish -r linux-arm64 -c Release
dotnet publish -r win-x64 -c Release
dotnet publish -r osx.13-arm64 -c Release
dotnet publish -r osx-x64 -c Release

# Switch to the directory where the script exists
cd "$(dirname "$0")" || exit 1

# Create the directory
mkdir -p "$1"

# Check if the directory creation was successful
if [ $? -eq 0 ]; then
  echo "Directory created successfully: $1"
else
  echo "Failed to create directory: $1"
fi

cp bin/Release/net8.0/linux-arm64/publish/tankman $1/tankman-linux-arm64
cp bin/Release/net8.0/linux-x64/publish/tankman $1/tankman-linux-x64
cp bin/Release/net8.0/linux-musl-x64/publish/tankman $1/tankman-linux-musl-x64
cp bin/Release/net8.0/win-x64/publish/tankman.exe $1/tankman-win-x64.exe
cp bin/Release/net8.0/osx.13-arm64/publish/tankman $1/tankman-osx13-arm64
cp bin/Release/net8.0/osx-x64/publish/tankman $1/tankman-osx-x64

# Switch back to the original directory
cd - >/dev/null 2>&1

echo Release files copied to $1