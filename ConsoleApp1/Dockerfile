FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o publish

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app
EXPOSE 80
ENV CHRROME_VERSION=93.0.4577.82-1
# https://sites.google.com/a/chromium.org/chromedriver/downloads
# https://www.ubuntuupdates.org/package/google_chrome/stable/main/base/google-chrome-stable?id=202706&page=1

RUN apt-get update && \
    apt-get install -y --no-install-recommends wget && \
    wget https://dl.google.com/linux/chrome/deb/pool/main/g/google-chrome-stable/google-chrome-stable_93.0.4577.82-1_amd64.deb && \
    apt-get install -y ./google-chrome-stable_93.0.4577.82-1_amd64.deb 
COPY --from=build app/publish .
ENTRYPOINT ["dotnet", "ConsoleApp.dll"]