
FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["MockService/MockService.csproj", "MockService/"]
RUN dotnet restore "MockService/MockService.csproj"

COPY . .
WORKDIR "/src/MockService"
RUN dotnet build "MockService.csproj" -c Release -o /app/build -p:DefineConstants=LINUX

FROM build AS publish
RUN dotnet publish "MockService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
RUN apt-get update && apt-get install -y curl
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MockService.dll"]