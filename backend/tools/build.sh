#! /bin/bash

dotnet build -c release
docker build -t ghcr.io/oliverfida/checkit_backend .
docker save ghcr.io/oliverfida/checkit_backend > tools/release_files/checkit_backend.docker
cd tools/release_files
tar -cv * -f ../../out/checkit_backend_release.tgz