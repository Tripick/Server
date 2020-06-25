FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TripickServer.csproj", "TripickServer/"]
RUN dotnet restore "TripickServer.csproj"
COPY . .
WORKDIR "/src/TripickServer"
RUN dotnet build "TripickServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TripickServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TripickServer.dll"]