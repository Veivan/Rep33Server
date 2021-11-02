FROM mcr.microsoft.com/dotnet/sdk:5.0
LABEL Description="Report33 Server" Vendor="Moscow-Cargo" Version="1.0.0"
USER root
RUN apt update && apt install -y zip && apt install -y libgdiplus
COPY /distrib/ /opt/distrib
RUN unzip /opt/distrib/rep33server.zip
RUN rm -rf /opt/distrib && mkdir /share
WORKDIR /rep33server
ENTRYPOINT dotnet ./Rep33.WEB.dll