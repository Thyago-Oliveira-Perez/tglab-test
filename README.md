# API de Gerenciamento de Carteira de Jogadores

## Descrição

Este projeto é uma API RESTful desenvolvida em C# (.NET 6) para gerenciar as transações e apostas de jogadores em uma plataforma. A API permite que jogadores criem contas, façam login, realizem apostas, e consultem suas transações e apostas. A documentação e testes dos endpoints são disponibilizados via Swagger.

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
