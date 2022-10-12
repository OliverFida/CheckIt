  # syntax=docker/dockerfile:1
  FROM mcr.microsoft.com/dotnet/aspnet:6.0
  COPY bin/release/net6.0/ App/
  WORKDIR /App
  ENV ASPNETCORE_URLS="http://0.0.0.0:5000"
  ENTRYPOINT ["dotnet", "awl-raumreservierung.dll"]
  CMD [ "App/awl-raumreservierung" ]