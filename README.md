# School Management System

A full-stack application for managing school operations, built with .NET Core and Blazor / React.
This has been created for the purpose of practicing test automation.
With this you are able to practice ui, api, and sql interactions.

This is not meant to be used for actual school management.

The usage assumption is that it will be hosted via docker and interacted with locally.
Sharing access to the site will require some changes to the docker compose.

## Components

- **Frontend**: Blazor Web UI or React - React is loaded into the dockerfile
- **Backend**: .NET Core API
- **Database**: SQL Server

## Prerequisites

- Docker Desktop
- Docker Compose

## Getting Started

1. Build and run the application using Docker Compose:
```bash
docker-compose up --build
```

2. Access the application:
- Frontend UI: http://localhost:5001
- API Swagger: http://localhost:5000/swagger
- Database: localhost:1433

## Architecture

The application consists of three Docker containers:
- `ui`: React / Blazor frontend running on port 5001
- `api`: .NET Core API running on port 5000
- `db`: SQL Server database running on port 1433

## Environment Variables

The following environment variables are configured in docker-compose.yml:
- API: `ASPNETCORE_ENVIRONMENT=Docker`
- Database: 
  - `ACCEPT_EULA=Y`
  - `SA_PASSWORD=YourStrong!Passw0rd`

## Stopping the Application

To stop the application and remove containers:
```bash
docker-compose down
```

To stop the application and remove containers, images, and volumes:
```bash
docker-compose down --rmi all -v
``` 