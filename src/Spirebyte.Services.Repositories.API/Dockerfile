﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY . .
ENV ASPNETCORE_URLS http://*:80
ENTRYPOINT dotnet Spirebyte.Services.Repositories.API.dll