FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG Nuget_CustomFeedUserName
ARG Nuget_CustomFeedPassword

WORKDIR /src
COPY ["src/Spirebyte.Services.Repositories.API/Spirebyte.Services.Repositories.API.csproj", "src/Spirebyte.Services.Repositories.API/"]
COPY ["src/Spirebyte.Services.Repositories.Infrastructure/Spirebyte.Services.Repositories.Infrastructure.csproj", "src/Spirebyte.Services.Repositories.Infrastructure/"]
COPY ["src/Spirebyte.Services.Repositories.Application/Spirebyte.Services.Repositories.Application.csproj", "src/Spirebyte.Services.Repositories.Application/"]
COPY ["src/Spirebyte.Services.Repositories.Core/Spirebyte.Services.Repositories.Core.csproj", "src/Spirebyte.Services.Repositories.Core/"]
RUN dotnet nuget add source --username ${Nuget_CustomFeedUserName} --password ${Nuget_CustomFeedPassword} --store-password-in-clear-text --name github "https://nuget.pkg.github.com/Spirebyte-Project-Management/index.json"
RUN dotnet restore "src/Spirebyte.Services.Repositories.API/Spirebyte.Services.Repositories.API.csproj"
COPY . .
WORKDIR "/src/src/Spirebyte.Services.Repositories.API"
RUN dotnet build "Spirebyte.Services.Repositories.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spirebyte.Services.Repositories.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT Docker
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED true
ENTRYPOINT ["dotnet", "Spirebyte.Services.Repositories.API.dll"]
