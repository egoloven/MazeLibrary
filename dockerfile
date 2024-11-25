FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /app

COPY MazeApp/ /app

COPY MazeLibrary.dll /app

RUN dotnet build MazeApp.csproj -c Release

ENTRYPOINT ["dotnet", "bin/Release/net9.0/MazeApp.dll"]
