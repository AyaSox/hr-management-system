# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY HRManagementSystem.csproj ./
RUN dotnet restore HRManagementSystem.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish HRManagementSystem.csproj -c Release -o /app/out --no-restore -p:PublishReadyToRun=true -p:UseAppHost=false

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# .NET 8 container default binding is 8080; expose it explicitly
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "HRManagementSystem.dll"]

# Optional healthcheck example:
# HEALTHCHECK --interval=30s --timeout=30s --start-period=5s --retries=3 CMD curl --fail http://localhost:8080/health || exit 1
