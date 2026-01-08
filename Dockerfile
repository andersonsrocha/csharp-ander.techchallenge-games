FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copiar arquivos de projeto para melhor cache do Docker
COPY TechChallengeGames.sln ./
COPY src/TechChallengeGames.Api/TechChallengeGames.Api.csproj src/TechChallengeGames.Api/
COPY src/TechChallengeGames.Application/TechChallengeGames.Application.csproj src/TechChallengeGames.Application/
COPY src/TechChallengeGames.Data/TechChallengeGames.Data.csproj src/TechChallengeGames.Data/
COPY src/TechChallengeGames.Domain/TechChallengeGames.Domain.csproj src/TechChallengeGames.Domain/
COPY src/TechChallengeGames.Security/TechChallengeGames.Security.csproj src/TechChallengeGames.Security/
COPY src/TechChallengeGames.Elasticsearch/TechChallengeGames.Elasticsearch.csproj src/TechChallengeGames.Elasticsearch/
COPY tests/TechChallengeGames.Application.Test/TechChallengeGames.Application.Test.csproj tests/TechChallengeGames.Application.Test/

# Realizar o restore
RUN dotnet restore

# Copiar arquivos
COPY src/ src/
COPY tests/ tests/

# Publicar o projeto
RUN dotnet publish src/TechChallengeGames.Api/TechChallengeGames.Api.csproj -c Release -o /app/publish --no-restore

# Runtime stage - usando Alpine para imagem mais leve
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime

# Instalar New Relic
RUN apk update && apk add --no-cache wget tar \
    && wget https://download.newrelic.com/dot_net_agent/latest_release/newrelic-dotnet-agent_amd64.tar.gz -r \
    && tar -xzf download.newrelic.com/dot_net_agent/latest_release/newrelic-dotnet-agent_amd64.tar.gz -C /usr/local \ 
    && rm -rf download.newrelic.com

# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
    CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
    CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
    NEW_RELIC_APP_NAME="techchallenge-games-newrelic"

WORKDIR /app

# Criar non-root user (Alpine Linux)
RUN addgroup -S appuser && adduser -S appuser -G appuser

# Copiar os arquivos publicados
COPY --from=build /app/publish .

# Trocar ownership para non-root user
RUN chown -R appuser:appuser /app
USER appuser

# Expor a porta
EXPOSE 8080

ENTRYPOINT ["dotnet", "TechChallengeGames.Api.dll"]
