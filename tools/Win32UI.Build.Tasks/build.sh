#!/bin/sh

set -e
MY_DIR=$(cd $(dirname $0) && pwd)
cd $MY_DIR

dotnet restore; dotnet build
mkdir -p ../../bin/BuildTasks/netcoreapp1.1
cp -f bin/Debug/netcoreapp1.1/Win32UI.*.dll ../../bin/BuildTasks/netcoreapp1.1
cp -f Sunburst.Win32UI.Build.targets ../../bin/BuildTasks

