#!/bin/bash
#
# Find recursively all the .csproj files on given path
# and extract the package reference lines to packages.txt file.
#
set -euo pipefail
IFS=$'\n\t'

PATHS_TO_FIND="root/project/src/*/*.csproj" # change it by depend on your project folder structure.

addPackages () {
    local search="$1"

    for path in $search; do
        while read -r line; do
            if [[ $line =~ "<PackageReference" ]]; then
                echo "${line// /}" >> packages.txt
            fi  
        done < "$path"
    done
}

# projects
addPackages $PATHS_TO_FIND
