# escape=`
FROM jsturtevant/4.7-windowsservercore-1709-builder:latest as build-agent
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

ARG CONFIGURATION=Release

# Build files
WORKDIR C:\src
COPY samples\IISLoggerApp\packages.config .\IISLoggerApp\
RUN nuget restore IISLoggerApp\packages.config -PackagesDirectory .\IISLoggerApp\packages

COPY samples\IISLoggerApp C:\src\IISLoggerApp
RUN echo $env:CONFIGURATION; ` 
    msbuild IISLoggerApp\IISLoggerApp.csproj /p:OutputPath=C:\out /p:DeployOnBuild=true /p:Configuration=$env:CONFIGURATION

## final image
FROM microsoft/aspnet:4.7.1-windowsservercore-1709

## setup machine for various types of logs
RUN Set-WebConfigurationProperty -pspath 'MACHINE/WEBROOT/APPHOST'  -filter "system.applicationHost/sites/sitedefaults/logFile" -name "logTargetW3C" -value "ETW"; `
     New-EventLog -LogName Application -Source "TestSource";

WORKDIR /inetpub/wwwroot
COPY --from=build-agent C:\out\_PublishedWebsites\IISLoggerApp .

WORKDIR /app
COPY src/ServiceProcessWatcher/ServiceProcessWatcher.Console/bin/Release/netcoreapp2.0/win10-x64/publish/ .
COPY samples/basic.json .

## override with new service process watcher
ENTRYPOINT ["C:\\app\\ServiceProcessWatcher.Console.exe","basic.json"]