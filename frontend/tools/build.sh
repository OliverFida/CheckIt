#! /bin/bash

npm run build
docker build -t ghcr.io/oliverfida/checkit_frontend .
docker save ghcr.io/oliverfida/checkit_frontend > tools/release_files/checkit_frontend.docker
cd tools/release_files
tar -cv * -f ../../out/checkit_frontend_release.tgz
docker push ghcr.io/oliverfida/checkit_frontend:latest