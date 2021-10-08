#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Rep33wapi/Rep33wapi.csproj", "Rep33wapi/"]
RUN dotnet restore "Rep33wapi/Rep33wapi.csproj"
COPY . .
WORKDIR "/src/Rep33wapi"
RUN dotnet build "Rep33wapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rep33wapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rep33wapi.dll"]