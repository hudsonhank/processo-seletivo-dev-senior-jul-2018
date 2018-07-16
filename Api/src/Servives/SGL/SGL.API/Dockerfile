FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 51198
EXPOSE 44363

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/Servives/SGL/SGL.API/SGL.API.csproj src/Servives/SGL/SGL.API/
RUN dotnet restore src/Servives/SGL/SGL.API/SGL.API.csproj
COPY . .
WORKDIR /src/src/Servives/SGL/SGL.API
RUN dotnet build SGL.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SGL.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SGL.API.dll"]
