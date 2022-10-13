  # syntax=docker/dockerfile:1
  FROM nginx:latest
  COPY tools/docker/backend_url.sh /docker-entrypoint.d/99-backend_url.sh
  COPY build /www
  COPY conf/nginx.conf /etc/nginx/nginx.conf
  # CMD [ "nginx" ]