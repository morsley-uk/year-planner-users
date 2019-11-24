#!/bin/bash

parent_path=$(cd "$(dirname "${BASH_SOURCE[0]}")";pwd -P)
common="$parent_path"/../../Scripts
source $common/header.sh

header 'BUILD STARTED'

dotnet build --verbosity normal --configuration Debug --no-restore

header 'BUILD COMPLETED'