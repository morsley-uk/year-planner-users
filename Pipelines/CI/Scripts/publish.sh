#!/bin/bash

#cd ../../..

echo "-------------------------------------------------------------------------------"
printf "\n"
echo "---------- PUBLISH INITIATED ----------"

dotnet publish --verbosity normal --configuration Release --no-build --no-restore --output Output

echo "---------- PUBLISH FINISHED ----------"
printf "\n"
echo "-------------------------------------------------------------------------------"