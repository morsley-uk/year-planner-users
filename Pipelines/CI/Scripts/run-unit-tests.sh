#!/bin/bash

#cd ../../..
cd Tests
cd UnitTests

for d in */; do
	#echo "$d"
    cd "$d"
    #ls -la
    projectFile=$(find . -type f -name "*.csproj")
    #echo $projectFile
    echo "-------------------------------------------------------------------------------"
    printf "\n"
    echo "---------- TESTS INITIATED ----------"

    dotnet test $projectFile --verbosity quiet --no-build --no-restore --results-directory TestResults --blame --logger trx --configuration Release

    echo "---------- TESTS FINISHED ----------"
    printf "\n"
    echo "-------------------------------------------------------------------------------"
    printf "\n"
    printf "\n"
    printf "\n"

    cd ..
done