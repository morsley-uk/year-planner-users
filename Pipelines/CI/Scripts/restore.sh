#!/bin/bash

#cd ../../..

echo "-------------------------------------------------------------------------------"
printf "\n"
echo "---------- RESTORE INITIATED ----------"

dotnet restore --verbosity normal

echo "---------- RESTORE FINISHED ----------"
printf "\n"
echo "-------------------------------------------------------------------------------"