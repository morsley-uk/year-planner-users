#!/bin/bash

#cd ../../..

echo "-------------------------------------------------------------------------------"
printf "\n"
echo "---------- PUBLISH INITIATED ----------"

dotnet publish --verbosity quiet --configuration Release --force --no-build --no-restore --output Output

echo "---------- PUBLISH FINISHED ----------"
printf "\n"
echo "-------------------------------------------------------------------------------"