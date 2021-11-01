#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM registry.mwc.local/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000

FROM registry.mwc.local/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY . .
# COPY ["../Rep33.Domain/Rep33.Domain.csproj", "../Rep33.Domain/"]
# COPY ["../Rep33.Data/Rep33.Data.csproj", "../Rep33.Data/"]
# COPY ["../Rep33wapi/Rep33.WEB.csproj", "../Rep33wapi/"]

RUN dotnet restore "./Rep33.Domain/Rep33.Domain.csproj"
RUN dotnet restore "./Rep33.Data/Rep33.Data.csproj"
RUN dotnet restore "./Rep33wapi/Rep33.WEB.csproj"


WORKDIR "/src/Rep33.Domain"
RUN dotnet build "Rep33.Domain.csproj" -c Release -o /app/build

WORKDIR "/src/Rep33.Data"
RUN dotnet build "Rep33.Data.csproj" -c Release -o /app/build

WORKDIR "/src/Rep33wapi"
RUN dotnet build "Rep33.WEB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rep33.WEB.csproj" -c Release -o /app/

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Rep33.WEB.dll"]
