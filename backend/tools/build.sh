#! /bin/bash

dotnet build -c release
docker build -t checkit .
docker save checkit > tools/release_files/checkit.docker
cd tools/release_files
tar -cv * -f ../../out/checkit_release.tgz
