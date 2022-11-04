  # syntax=docker/dockerfile:1
  FROM nginx:latest
  COPY tools/docker/backend_config.sh /docker-entrypoint.d/99-backend_config.sh
  COPY dist /www
  COPY conf/nginx.conf /etc/nginx/nginx.conf
  # CMD [ "nginx" ]