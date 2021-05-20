FROM mcr.microsoft.com/dotnet/sdk:5.0
EXPOSE 80
EXPOSE 5003

WORKDIR /src

COPY ["backend/src/API/API.csproj", "API/"]

RUN dotnet restore API/API.csproj -nowarn:msb3202,nu1503

COPY ./backend/src/API ./API

WORKDIR /src/API

RUN dotnet build -c Debug
