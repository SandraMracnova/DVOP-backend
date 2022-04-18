FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app

COPY ../ /app
RUN dotnet restore
RUN dotnet publish -c Release -o out
RUN rm -rf /app/docker 
EXPOSE 5000
ENTRYPOINT ["dotnet", "/app/out/dvopapi-Mracnova.dll","--urls","http://0.0.0.0:5000"]