FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Install ca-certificates
RUN apt-get update && apt-get install -y ca-certificates

ENV ASPNETCORE_ENVIRONMENT=Docker

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Install certificates and set NuGet to trust them
RUN apt-get update && apt-get install -y ca-certificates
COPY my-ca-cert.crt /usr/local/share/ca-certificates/
RUN update-ca-certificates

# Copy and restore as distinct layers
COPY ["*.csproj", "./"]
COPY ["Nuget.Config", "./"]
RUN dotnet restore -v d

COPY . ./
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Automation_School_API.dll"]