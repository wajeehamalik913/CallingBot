
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

WORKDIR /App

# Copy csproj and restore as distinct layers
#COPY bin/Release/net6.0/publish .


#RUN dotnet restore


COPY . /*.csproj ./

COPY Common/Common.csproj ./

#COPY . ./
#COPY . ./
RUN dotnet restore ./MeetingBot.sln
RUN dotnet publish ./MeetingBot.sln -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /App/out .
#ENV ASPNETCORE_URLS http://*:3978
#EXPOSE 3978

ENTRYPOINT ["dotnet", "MeetingBot.dll"]