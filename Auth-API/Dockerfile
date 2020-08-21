#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#First Image used as Base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

#Work directory
WORKDIR /app

#Exposing ports 80 and 443 of Container for HTTP and HTTPS Requests 
EXPOSE 80
EXPOSE 443

#Image used for the Build process 
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

#Creating a work directory named src and copying .csproj files of Auth-API and Auth-Data projects
WORKDIR /src
COPY ["Auth-API/Auth-API.csproj", "Auth-API/"]
COPY ["Auth-Data/Auth-Data.csproj", "Auth-Data/"]

#Restoring all the packages used by the project Auth-API
RUN dotnet restore "Auth-API/Auth-API.csproj"

#Copying everything to the WORKDIR(src)
COPY . .

#New directory
WORKDIR "/src/Auth-API"

#Arg variable used with Docker Build Command with a default value
ARG ASPNETCORE_ENVIRONMENT=Debug

#Running the build command and stablishing the Compile mode and Output directory
RUN dotnet build "Auth-API.csproj" -c $ASPNETCORE_ENVIRONMENT -o /app/build

#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#First Image used as Base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

#Work directory
WORKDIR /app

#Exposing ports 80 and 443 of Container for HTTP and HTTPS Requests 
EXPOSE 80
EXPOSE 443

#Image used for the Build process 
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

#Creating a work directory named src and copying .csproj files of Auth-API and Auth-Data projects
WORKDIR /src
COPY ["Auth-API/Auth-API.csproj", "Auth-API/"]
COPY ["Auth-Data/Auth-Data.csproj", "Auth-Data/"]

#Restoring all the packages used by the project Auth-API
RUN dotnet restore "Auth-API/Auth-API.csproj"

#Copying everything to the WORKDIR(src)
COPY . .

#New directory
WORKDIR "/src/Auth-API"

#Arg variable used with Docker Build Command with a default value
ARG ASPNETCORE_ENVIRONMENT=Debug

#Running the build command and stablishing the Compile mode and Output directory
RUN dotnet build "Auth-API.csproj" -c $ASPNETCORE_ENVIRONMENT -o /app/build

FROM build as test
WORKDIR "/src/Auth-Test/"

RUN dotnet test "Auth-Test.csproj" --logger "trx;LogFileName=auth-api-tests.xml"

#Using the previous Build command output to make a Publish command 
FROM build AS publish

#Arg variable used with Docker Build Command with a default value
ARG ASPNETCORE_ENVIRONMENT=Debug

#Running the publish command and stablishing the Compile mode and Output directory 
RUN dotnet publish "Auth-API.csproj" -c $ASPNETCORE_ENVIRONMENT -o /app/publish

#Using base image to create the final Layer
FROM base AS final

#New work directory
WORKDIR /app

#Copying everything from publish directory to /app/publish
COPY --from=publish /app/publish .

#Defining what command and value use to start the container
ENTRYPOINT ["dotnet", "Auth-API.dll"]