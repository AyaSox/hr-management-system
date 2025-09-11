# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY HRManagementSystem/*.csproj ./HRManagementSystem/
RUN dotnet restore

# Copy everything else and build
COPY HRManagementSystem/. ./HRManagementSystem/
WORKDIR /src/HRManagementSystem
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "HRManagementSystem.dll"]