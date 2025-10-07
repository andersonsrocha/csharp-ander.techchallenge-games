FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo da solução
COPY TechChallengeGames.sln ./

# Copy project files
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

# Construir o projeto
RUN dotnet build -c Release --no-restore

# Publicar o projeto
RUN dotnet publish src/TechChallengeGames.Api/TechChallengeGames.Api.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Install the agent
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y 'newrelic-dotnet-agent' \
&& rm -rf /var/lib/apt/lists/*

# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
NEW_RELIC_APP_NAME="techchallenge-games-newrelic"

WORKDIR /app

# Criar non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copiar os arquivos publicados
COPY --from=build /app/publish .

# Trocar ownership para non-root user
RUN chown -R appuser:appuser /app
USER appuser

# Expor a porta
EXPOSE 8080

ENTRYPOINT ["dotnet", "TechChallengeGames.Api.dll"]
