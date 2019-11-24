#!/bin/bash

parent_path=$(cd "$(dirname "${BASH_SOURCE[0]}")";pwd -P)
common="$parent_path"/../../Scripts
source $common/header.sh

header 'PUBLISH STARTED'

dotnet publish --verbosity normal --configuration Debug --no-build --no-restore --output Published --nologo

header 'PUBLISH COMPLETED'