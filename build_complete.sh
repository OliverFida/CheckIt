#!/usr/bin/env bash

cd backend
tools/build.sh
cd ../frontend
tools/build.sh

cd ..

mv backend/tools/release_files/checkit_backend.docker release_files/
mv frontend/tools/release_files/checkit_frontend.docker release_files/

cd release_files
tar -cv * -f ../checkit_release.tgz

docker push ghcr.io/oliverfida/checkit_backend:latest
docker push ghcr.io/oliverfida/checkit_frontend:latest
