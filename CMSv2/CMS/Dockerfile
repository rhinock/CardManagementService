FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS src
WORKDIR /app

# restore

COPY *.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done
RUN dotnet restore

# Copy everything else and build

COPY . ./
FROM src as build
ARG SERVICE_NAME
WORKDIR /app/${SERVICE_NAME}
RUN dotnet publish --no-restore -c Release -o out

# Build runtime image

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
ARG SERVICE_NAME
ENV SERVICE_NAME=${SERVICE_NAME}
WORKDIR /app
COPY --from=build /app/${SERVICE_NAME}/out .
ENTRYPOINT dotnet ${SERVICE_NAME}.dll
