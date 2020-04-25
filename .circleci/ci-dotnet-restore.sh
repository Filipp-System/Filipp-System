#!/bin/bash
#
# .NET Core try to find obj folders to ensure the package required is installed before or not.
# The script collect all the obj folder to zip(tar.gz) file to extract it for the next build.
#
set -euo pipefail
IFS=$'\n\t'

PATHS_TO_FIND="./**/*.csproj" # change it by depend on your project folder structure.

copyObjFolders () {
    local search="$1"

    for path in $search; do
        mkdir -p ./objs/$path/..
        cp -r $path ./objs/$path/..
    done
}

if [ ! -f ./objs.tar.gz ]; then
    #restore
    dotnet restore
    
    #projects
    copyObjFolders $PATHS_TO_FIND

    #zip.
    tar -czf objs.tar.gz -C objs .
else
    #unzip objs
    tar -xzf objs.tar.gz

    echo "Restore skipped."
fi