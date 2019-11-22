#!/bin/bash

#cd ../../..

echo "-------------------------------------------------------------------------------"
printf "\n"
echo "---------- BUILD INITIATED ----------"

dotnet build --verbosity normal --configuration Release --no-restore

echo "---------- BUILD FINISHED ----------"
printf "\n"
echo "-------------------------------------------------------------------------------"