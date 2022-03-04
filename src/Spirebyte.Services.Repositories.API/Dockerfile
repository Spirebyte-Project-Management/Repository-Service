#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Repository-Service/src/Spirebyte.Services.Repositories.API/Spirebyte.Services.Repositories.API.csproj", "src/Repository-Service/src/Spirebyte.Services.Repositories.API/"]
COPY ["src/Repository-Service/src/Spirebyte.Services.Repositories.Infrastructure/Spirebyte.Services.Repositories.Infrastructure.csproj", "src/Repository-Service/src/Spirebyte.Services.Repositories.Infrastructure/"]
COPY ["src/Repository-Service/src/Spirebyte.Services.Repositories.Application/Spirebyte.Services.Repositories.Application.csproj", "src/Repository-Service/src/Spirebyte.Services.Repositories.Application/"]
COPY ["src/Repository-Service/src/Spirebyte.Services.Repositories.Core/Spirebyte.Services.Repositories.Core.csproj", "src/Repository-Service/src/Spirebyte.Services.Repositories.Core/"]
RUN dotnet restore "src/Repository-Service/src/Spirebyte.Services.Repositories.API/Spirebyte.Services.Repositories.API.csproj"
COPY . .
WORKDIR "/src/src/Repository-Service/src/Spirebyte.Services.Repositories.API"
RUN dotnet build "Spirebyte.Services.Repositories.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spirebyte.Services.Repositories.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spirebyte.Services.Repositories.API.dll"]
