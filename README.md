<div align="center">

<h1>
  <br/>
  <br/>
  <div>🎮</div>
  <b>Fiap Cloud Games</b>
  <br/>
  <br/>
  <br/>
</h1>

**Fiap Cloud Games** é uma poderosa plataforma de jogos em nuvem. A aplicação conta com arquitetura Domain-Driven Design (DDD), ASP.NET Core 8, autenticação via JWT e banco de dados PostgreSQL, além de contar uma boas práticas de arquitetura, segurança e escalabilidade com Azure Cloud.

</div>

> \[!NOTE]
>
> Este projeto visa oferecer uma aplicação robusta, escalável e segura. O desenvolvimento deste projeto é baseado exclusivamente nas suas necessidades guiadas pelo curso de pós graduação Fiap.

<div align="center">

![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=flat&logo=postgresql&logoColor=white)
![xUnit](https://img.shields.io/badge/xUnit-512BD4?style=flat&logo=.net&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat&logo=docker&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=flat&logo=jsonwebtokens&logoColor=white)
![DDD](https://img.shields.io/badge/DDD-Domain--Driven%20Design-FF6B6B?style=flat)

</div>

<details>

<summary>
  <b>Table of contents</b>
</summary>

#### TOC

- [📦 Começando](#-começando)
- [🖱️ Primeiro acesso](#️-primeiro-acesso)
- [🚧 Contruindo e publicando a aplicação](#-contruindo-e-publicando-a-aplicação)
- [✨ Características](#-características)
- [🚀 Recursos](#-recursos)

####

</details>

## 📦 Começando

Comece clonando o repositório `csharp-ander.techchallenge-users`, executando o comando:

```bash
git clone https://github.com/andersonsrocha/csharp-ander.techchallenge-users.git
```

Agora acesse o projeto usando:

```bash
cd csharp-ander.techchallenge-users
```

Atualize a string de conexão do banco em `appsettings.json` e realize a restauração dos pacotes:

```bash
dotnet restore
```

Agora precisaremos aplicar as migrações, para isso acesse a pasta `src` e depois execute o comando:

```bash
dotnet ef database update -p TechChallengeUsers.Data -s TechChallengeUsers.Api
```

Ainda dentro da pasta `src`, execute o comando abaixo para iniciar a aplicação:

```bash
dotnet run -p TechChallengeUsers.Api
```

E por fim poderá acessar a aplicação atráves do link [Documentação](http://localhost:5167/swagger/index.html).

<br/>

## 🖱️ Primeiro acesso

Para o primeiro acesso utilize as credenciais abaixo:

```bash
{
  "email": "admin@fiap.com.br",
  "password": "*_7hg613"
}
```

## 🚧 Contruindo e publicando a aplicação

Agora para construirmos a aplicação, basta executar o comando abaixo no diretório raiz do projeto:

```bash
dotnet build
```

E por fim, para publicar a aplicação:

> \[!TIP]
>
> É possível trocar a pasta de destino substituindo `./publish` pelo diretório desejado.

```bash
dotnet publish -c Release -o ./publish
```

## ✨ Características

- [x] ~~Usuário admin.~~
- [x] ~~Banco de dados.~~
- [x] ~~Login com autenticação JWT.~~
- [x] ~~Funções admin e user.~~
- [x] ~~Testes unitários.~~
  - [x] ~~Validação de senha.~~
  - [x] ~~Validação de e-mail.~~
  - [x] ~~Autenticação.~~
  - [x] ~~Criação de usuário.~~
- [x] ~~Criação de arquivo Dockerfile.~~
- [x] ~~Domain-Driven Design.~~
- [x] ~~Criação de usuário.~~
- [x] ~~Criação de jogos.~~
- [x] ~~Criação de migrations.~~
- [x] ~~Pipeline de CI/CD~~

<br/>

## 🚀 Recursos

- 🎨 **.NET 8 SDK**: Framework moderno e multiplataforma da Microsoft que oferece alta performance, suporte nativo para contêineres, APIs mínimas e recursos avançados de desenvolvimento. Inclui melhorias significativas em performance, garbage collection otimizado e suporte completo para desenvolvimento de aplicações web robustas e escaláveis.
- 🗄️ **PostgreSQL**: Banco de dados relacional open-source robusto, conhecido por sua confiabilidade, recursos avançados e conformidade com padrões SQL. Oferece suporte para dados JSON, transações ACID e alta disponibilidade.
- 🧪 **xUnit**: Framework de testes unitários para .NET que fornece uma base sólida para testes automatizados, com suporte para testes parametrizados, fixtures e execução paralela.
- 🐳 **Docker**: Containerização da aplicação para garantir consistência entre ambientes de desenvolvimento, teste e produção, facilitando deploy e escalabilidade.
- 🔐 **JWT Authentication**: Sistema de autenticação baseado em tokens seguros e stateless, permitindo autorização distribuída e controle de acesso granular.
- 🏗️ **Domain-Driven Design (DDD)**: Arquitetura que foca no domínio do negócio, promovendo código mais organizando, manutenível e alinhado com as regras de negócio.

<br/>

Copyright © 2025.
