#!/bin/bash

#cd ../../..

echo "-------------------------------------------------------------------------------"
printf "\n"
echo "---------- RESTORE INITIATED ----------"

dotnet restore --verbosity normal --force --no-cache

echo "---------- RESTORE FINISHED ----------"
printf "\n"
echo "-------------------------------------------------------------------------------"