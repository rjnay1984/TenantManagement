
FROM mcr.microsoft.com/dotnet/sdk:3.1
EXPOSE 80

WORKDIR /src
COPY ["backend/src/Identity/Identity.csproj", "Identity/"]

RUN dotnet restore Identity/Identity.csproj -nowarn:msb3202,nu1503
COPY ./backend/src/Identity ./Identity
WORKDIR "/src/Identity"

RUN dotnet build -c Debug

# ENTRYPOINT ["dotnet", "run", "--no-build", "--no-launch-profile", "-c", "$BUILD_CONFIGURATION", "--"]
