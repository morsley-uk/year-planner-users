#!/bin/bash

cd ../../..

echo "-------------------------------------------------------------------------------"
printf "\n"
echo "---------- BUILD INITIATED ----------"

dotnet build --verbosity quiet --configuration Release --force

echo "---------- BUILD FINISHED ----------"
printf "\n"
echo "-------------------------------------------------------------------------------"