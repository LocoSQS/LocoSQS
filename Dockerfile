# https://hub.docker.com/_/microsoft-dotnet
# Stage 1: Build backend
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-backend
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY LocoSQS/*.csproj ./LocoSQS/
COPY LocoSQS.Test/*.csproj ./LocoSQS.Test/
COPY LocoSQS.E2E/*.csproj ./LocoSQS.E2E/
RUN dotnet restore

# copy everything else and build app
COPY LocoSQS/. ./LocoSQS/
WORKDIR /source/LocoSQS
RUN dotnet publish -c Release -o /app

# Stage 2: Build frontend
FROM node:18-alpine as build-frontend
WORKDIR /app
COPY LocoSQS.Frontend/package*.json /app/
RUN npm install
COPY LocoSQS.Frontend/ /app/
ARG configuration=production
RUN npm run build -- --output-path=./dist/out --configuration $configuration

# Stage 3: Package
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine

EXPOSE 8080
ENV PORT=8080

# Copy app from build to package
WORKDIR /app
COPY --from=build-backend /app ./
RUN mkdir wwwroot
COPY --from=build-frontend /app/dist/out/ ./wwwroot

ENTRYPOINT ["dotnet", "LocoSQS.dll"]