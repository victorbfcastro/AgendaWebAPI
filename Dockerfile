FROM mcr.microsoft.com/dotnet/core/sdk:5.0.101 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:5.0.101
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AgendaWebAPI.dll"]