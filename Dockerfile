FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TripickServer.csproj", "TripickServer/"]
RUN dotnet restore
COPY . .
WORKDIR "/src/TripickServer"
RUN dotnet build "TripickServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TripickServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TripickServer.dll"]