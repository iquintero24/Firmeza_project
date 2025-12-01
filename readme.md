# Firmeza Project -- README

## Overview

Firmeza is a full‑stack application built with **ASP.NET Core 8**,
**Entity Framework Core**, **PostgreSQL**, and a modern **React/Vite
frontend**.\
The project uses **Docker** and **Docker Compose** to simplify
development, deployment, and environment consistency.

This README explains: - Project architecture\
- How each layer works\
- How to build and run everything using Docker\
- Useful developer commands

------------------------------------------------------------------------

## 📁 Project Structure

    Firmeza/
    │
    ├── Application/          # Business logic (services, DTOs, interfaces)
    ├── Domain/               # Entities and domain models
    ├── Infrastructure/       # Persistence layer, repositories, database context
    ├── Firmeza.web/          # ASP.NET MVC web application
    ├── firmeza_project-client/ # React/Vite frontend (optional)
    ├── docker-compose.yaml   # Docker Orchestration file
    └── README.md             # This file

------------------------------------------------------------------------

## 🧱 Architecture

The project follows a **Clean Architecture** / **Hexagonal Layers**
pattern:

### **1. Domain Layer**

-   Contains the core business entities (e.g., Product, Customer, Sale).
-   No external dependencies.

### **2. Application Layer**

-   Contains interfaces and business services.
-   Implements use‑case logic.
-   Depends only on Domain.

### **3. Infrastructure Layer**

-   Contains:
    -   EF Core DbContext
    -   PostgreSQL integration
    -   Repositories
    -   Seeders
-   Implements interfaces from Application.

### **4. Web Layer (Firmeza.web)**

-   ASP.NET Core MVC application.
-   Authentication and Identity.
-   Routing, Controllers, and Views.

------------------------------------------------------------------------

## 🐳 Running the Project with Docker

### **1. Requirements**

Make sure you have installed:

-   Docker\
-   Docker Compose

Check with:

``` bash
docker --version
docker-compose --version
```

------------------------------------------------------------------------

## 🚀 Start the Entire Stack

From the root folder where `docker-compose.yaml` is located:

### **Run normally**

``` bash
docker-compose up
```

### **Run in background (recommended)**

``` bash
docker-compose up -d
```

### **Stop all containers**

``` bash
docker-compose down
```

------------------------------------------------------------------------

## 🔧 Rebuild Everything (clean build)

If you made code changes and need to rebuild the containers:

``` bash
docker-compose up --build -d
```

------------------------------------------------------------------------

## 📦 Docker Services (depending on your compose.yaml)

A typical configuration includes:

-   **web** → ASP.NET Core app\
-   **client** → React/Vite frontend\
-   **postgres** → PostgreSQL database\
-   **pgadmin** (optional)

Example service interaction: - The Web API connects to PostgreSQL using
environment variables. - Frontend consumes API through


------------------------------------------------------------------------

## 🗄 Database Migrations

If you need to apply EF Core migrations manually inside the dockerized
backend container:

``` bash
docker exec -it firmeza_api bash
dotnet ef database update
```

------------------------------------------------------------------------

## 🔒 Environment Variables

The project uses `.env` files + DotNetEnv loader.

Important variables include:

    SUPABASE_CONNECTION_STRING=
    SUPABASE_MIGRATION_STRING=
    ASPNETCORE_ENVIRONMENT=

------------------------------------------------------------------------

## 🧪 Development Notes

-   Hot reload is supported only if volume mounts are configured.

-   If a service does not refresh changes:

    ``` bash
    docker-compose restart web
    ```

------------------------------------------------------------------------

## 🎉 Summary

This project is a modern, containerized, clean‑architecture ASP.NET Core
application.\
Docker Compose handles the whole stack so development is consistent and
simple:

``` bash
docker-compose up -d
```

And you're ready to go.

If you need help customizing features, CI/CD, or optimizing the
Dockerfile, feel free to ask!