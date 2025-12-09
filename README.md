# ProductPrediction.API – Execução com Docker

Este projeto é uma API em **ASP.NET Core 8** para previsão de compras de produtos, utilizando **PostgreSQL** como banco de dados e execução via **Docker** e **Docker Compose**.

---

## ✅ Requisitos

Antes de rodar o projeto, você precisa ter instalado:

- Docker Desktop
- Docker Compose
- (Opcional) Cliente SQL para acessar o banco: DBeaver / TablePlus / DataGrip / psql.

---

## 📁 Estrutura relevante

- `docker-compose.yml`
- `ProductPrediction.API/Dockerfile`

---

## ▶️ Como rodar a aplicação com Docker

### 1. Subir os containers

```bash
docker compose up --build -d
```

### 2. Verificar containers

```bash
docker compose ps
```

### 3. Acessar API

```
http://localhost:8080
```

Swagger (se habilitado):

```
http://localhost:8080/swagger
```

---

## 🗃 Acessar o banco (Com credenciais default)

- Host: localhost
- Porta: 5435
- User: appuser
- Senha: appsecret

---

## Informações adicionais

- Usuário default da aplicação: admin / admin@admin
- Usuário default já tem histórico preparado para teste de previsão

## ⏹ Parar / Remover

```bash
docker compose stop
docker compose down
docker compose down -v   # remove volume
```

---

## 🔁 Rebuild

```bash
docker compose up -d --build
```
