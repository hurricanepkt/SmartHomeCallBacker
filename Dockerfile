FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy as build-env
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0-jammy as runtime
WORKDIR /publish

RUN apt-get update \
    && apt-get install -y curl jq 

COPY --from=build-env /publish .
ENV ASPNETCORE_URLS=http://+:80
HEALTHCHECK CMD curl --fail http://localhost:80/health || exit
EXPOSE 80
ENTRYPOINT ["dotnet", "SmartHomeCallBacker.dll"]