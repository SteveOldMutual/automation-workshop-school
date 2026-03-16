# Stage 1: Build and publish the Blazor WebAssembly app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy and restore as distinct layers
COPY ["*.csproj", "./"]
RUN dotnet restore -v d

COPY . ./
RUN ASPNETCORE_ENVIRONMENT=Docker dotnet publish -c Release -o /app/publish 

# Stage 2: Serve the app with Nginx
FROM nginx:alpine AS final
WORKDIR /var/www/myblazorapp
COPY --from=build /app/publish/wwwroot .

# Copy and overwrite appsettings.json with appsettings.Docker.json
RUN cp /var/www/myblazorapp/appsettings.Docker.json /var/www/myblazorapp/appsettings.json

# Copy custom Nginx configuration if needed
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]