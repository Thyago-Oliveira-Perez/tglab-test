# API de Gerenciamento de Carteira de Jogadores

## Descrição

Este projeto é uma API RESTful desenvolvida em C# (.NET 6) para gerenciar as transações e apostas de jogadores em uma plataforma. A documentação e testes dos endpoints são disponibilizados via Swagger.

## Funcionalidades

### Endpoints Principais

- **Criar Novo Jogador**: Cria um jogador com uma carteira inicial e associada a uma moeda (R$ - BRL).
- **Login de Jogador**: Permite o login de um jogador, retornando suas informações e o saldo da carteira.
- **Realizar Aposta**: Efetua uma transação de aposta, decrementando o saldo da carteira.
- **Consultar Apostas**: Retorna as apostas realizadas pelo jogador, incluindo ID, valor, situação, valor do prêmio (se houver), e data/hora da aposta.
- **Cancelar Aposta**: Permite que o jogador cancele uma aposta específica.
- **Consultar Transações**: Exibe todas as transações da carteira do jogador, com informações como ID, Data/Hora, Tipo, e Valor.

### Banco de Dados

- **Tipo**: Relacional SQL Server.
- O projeto inclui o Modelo Entidade-Relacional (MER) como recurso para facilitar a implementação.

## Validações de Dados

- O e-mail de cada jogador deve ser único.
- Verificação para garantir que o valor da aposta seja maior que zero.
- Apostas devem ter valor mínimo de R$ 1,00.
- O saldo da carteira é validado para garantir a capacidade de realizar apostas e cancelamentos.
- Apostas já canceladas não podem ser canceladas novamente.

## Regras de Negócio

- As apostas possuem uma lógica de chance de vitória, e, se ganhas, geram uma transação de prêmio com o dobro do valor apostado.
- Após 5 apostas perdidas, o jogador recebe um bônus de 10% do total gasto nas apostas perdidas.
- Paginação implementada nos endpoints de consulta de apostas e transações, com parâmetros para página específica e limite de registros.

## Requisitos

- **.NET 6**
- **Docker**

## Rodar o projeto

Para rodar o projeto temos 2 opções.

1. Rodar ele via Visual Studio.
2. Rodar pelo terminal.

Segue um exemplo de cada uma das opções.

### Visual studio

Para rodar o projeto basta selecionar o projeto TgLab.API na aba de 'StartUp Project' como a seguir:
![alt text](image.png)

### Terminal

Para rodar o projeto basta acessar a pasta TgLab.API e rodar o comando `dotnet run` como no exemplo:

```
PS D:\user\tglab-test\BackEnd> cd TgLab.API
PS D:\user\tglab-test\BackEnd\TgLab.API> dotnet run
```

## Swagger

A documentação da API foi gerada com o Swagger. Para acessá-la, abra o navegador e insira a URL `https://localhost:{port}/swagger/index.html`, substituindo {port} pela porta em que o projeto está sendo executado. Se você estiver rodando o projeto via Docker, a porta será 8080. Caso esteja rodando localmente, a porta padrão será 7199.

## Migrations

### Adicionar

```
dotnet ef migrations add [nome_migration]
```

### Aplicar

```
dotnet ef database update
```

### Remover

```
dotnet ef migrations remove
```

- Esse comando remove a migration mais recente

### Listar migrations

```
dotnet ef migrations list
```

- ref: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing?tabs=dotnet-core-cli

## Docker

Para executar o projeto na docker completamente devemos primeiro criar um certificado ssl.

```
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p
dotnet dev-certs https --trust
```

- ref: https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-8.0

Feito isso podemos subir os serviços com o seguinte comando:

```
docker-compose up -d
```

Com o banco de dados rodando, precisamos rodar as migrations para que o banco fique totalmente atualizado. Para isso acesse a pasta da projeto TgLab.API e rode `dotnet ef database update`.

```
PS D:\user\tglab-test> cd .\BackEnd\TgLab.API\
PS D:\user\tglab-test\BackEnd\TgLab.API> dotnet ef database update
```

## WebSocket

Para se connectar ao Websocket e poder visualizar o saudo do jogador sendo atualizado em tempo real basta usar está string de conexão:

```
wss://localhost:7199/ws
```

- Caso esteja rodando na docker substitua a porta.
