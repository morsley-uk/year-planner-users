#!/bin/bash

parent_path=$(cd "$(dirname "${BASH_SOURCE[0]}")";pwd -P)
common="$parent_path"/../../Scripts
source $common/header.sh

header 'RESTORE STARTED'

dotnet restore --verbosity normal

header 'RESTORE COMPLETED'