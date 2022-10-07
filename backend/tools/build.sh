#! /bin/bash

docker build -t checkit .
docker save checkit > checkit.docker
tar -cv tools/release_files/* -f out/checkit_release.tgz
