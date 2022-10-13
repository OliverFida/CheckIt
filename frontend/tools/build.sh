#! /bin/bash

npm run build
docker build -t checkit_frontend .
docker save checkit_fontend > tools/release_files/checkit_frontend.docker
cd tools/release_files
tar -cv * -f ../../out/checkit_frontend_release.tgz
