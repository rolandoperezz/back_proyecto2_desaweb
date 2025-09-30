# STAGE 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia solución y proyectos (ajusta si tienes varios .csproj)
COPY *.sln ./
COPY api/*.csproj ./api/
RUN dotnet restore

# Copia el resto y publica
COPY api/. ./api/
WORKDIR /src/api
RUN dotnet publish -c Release -o /app/out

# STAGE 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "HelloApi.dll"]
