#!/bin/bash
set -euo pipefail
IFS=$'\n\t'

PROJECT_NAME="./src/SPA.Blazor" # the project name.

# build projects.
dotnet build --no-restore -c Release

# publish artifacts to root folder. 
# You can call only the pulish without --no-build option, if you are not building something not referanced on your project.
dotnet publish $PROJECT_NAME.csproj --no-build --no-restore -c Release -o ../artifacts