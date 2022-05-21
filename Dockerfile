FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

RUN mkdir MyInvest MyInvest.Domain MyInvest.Persistence

COPY ["src/MyInvest/packages.lock.json", "MyInvest/"]
COPY ["src/MyInvest/MyInvest.csproj", "MyInvest/"]

COPY ["src/MyInvest.Domain/packages.lock.json", "MyInvest.Domain/"]
COPY ["src/MyInvest.Domain/MyInvest.Domain.csproj", "MyInvest.Domain/"]

COPY ["src/MyInvest.Persistence/packages.lock.json", "MyInvest.Persistence/"]
COPY ["src/MyInvest.Persistence/MyInvest.Persistence.csproj", "MyInvest.Persistence/"]

RUN dotnet restore "MyInvest/MyInvest.csproj" --locked-mode

COPY src/MyInvest MyInvest/
COPY src/MyInvest.Domain MyInvest.Domain/
COPY src/MyInvest.Persistence MyInvest.Persistence/
WORKDIR "/src/MyInvest"
RUN dotnet build "MyInvest.csproj" --no-restore --configuration Release --output /app/build

FROM build AS publish
RUN dotnet publish "MyInvest.csproj" --configuration Release --output /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyInvest.dll"]
