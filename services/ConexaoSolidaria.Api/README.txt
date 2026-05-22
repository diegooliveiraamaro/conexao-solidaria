
# CONEXÃO SOLIDÁRIA

Sistema backend de doações e campanhas desenvolvido com arquitetura baseada em microsserviços utilizando .NET 8, PostgreSQL, RabbitMQ, Docker, Kubernetes, Prometheus e Grafana.

========================================================
TECNOLOGIAS UTILIZADAS
========================================================

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- RabbitMQ
- Docker
- Kubernetes
- Prometheus
- Grafana
- Swagger
- JWT Authentication
- DDD
- CQRS
- Background Worker

========================================================
ARQUITETURA
========================================================

MICROSSERVIÇOS:

1 - API PRINCIPAL
Responsável por:
- Cadastro/Login de usuários
- Autenticação JWT
- Cadastro de campanhas
- Registro de doações
- Publicação de eventos no RabbitMQ

2 - WORKER
Responsável por:
- Consumir mensagens RabbitMQ
- Processar eventos assíncronos
- Atualizar campanhas
- Processamento em background

========================================================
ESTRUTURA DO PROJETO
========================================================

conexao-solidaria/

services/
 ├── ConexaoSolidaria.Api
 └── ConexaoSolidaria.Worker

shared/
 └── ConexaoSolidaria.Shared

k8s/

docker-compose.yml

README.md

========================================================
PRÉ-REQUISITOS
========================================================

INSTALAR:

- Docker Desktop
- Kubernetes habilitado
- WSL2 habilitado
- .NET 8 SDK

========================================================
VERIFICAR INSTALAÇÕES
========================================================

Docker:

docker --version

Kubernetes:

kubectl version --client

.NET:

dotnet --version

========================================================
CLONAR PROJETO
========================================================

git clone https://github.com/seu-repositorio/conexao-solidaria.git

cd conexao-solidaria

========================================================
RESTAURAR DEPENDÊNCIAS
========================================================

dotnet restore

========================================================
BUILDAR PROJETO
========================================================

dotnet build

========================================================
SUBIR INFRAESTRUTURA DOCKER
========================================================

docker compose up -d

========================================================
VERIFICAR CONTAINERS
========================================================

docker ps

Containers esperados:
- postgres
- rabbitmq
- grafana
- prometheus

========================================================
RODAR API
========================================================

dotnet run --project services/ConexaoSolidaria.Api

========================================================
RODAR WORKER
========================================================

dotnet run --project services/ConexaoSolidaria.Worker

========================================================
URLS
========================================================

SWAGGER:

http://localhost:5096/swagger

RABBITMQ:

http://localhost:15672

Usuário:
guest

Senha:
guest

GRAFANA:

http://localhost:3000

Usuário:
admin

Senha:
admin

PROMETHEUS:

http://localhost:9090

========================================================
DOCKER
========================================================

BUILD API:

docker build -t conexao-api -f services/ConexaoSolidaria.Api/Dockerfile .

BUILD WORKER:

docker build -t conexao-worker -f services/ConexaoSolidaria.Worker/Dockerfile .

VERIFICAR IMAGENS:

docker images

========================================================
KUBERNETES
========================================================

HABILITAR KUBERNETES:

Docker Desktop > Settings > Kubernetes > Enable Kubernetes

VERIFICAR CLUSTER:

kubectl get nodes

========================================================
APLICAR DEPLOYMENTS
========================================================

POSTGRES:

kubectl apply -f k8s/postgres-deployment.yaml

RABBITMQ:

kubectl apply -f k8s/rabbitmq-deployment.yaml

API:

kubectl apply -f k8s/api-deployment.yaml

WORKER:

kubectl apply -f k8s/worker-deployment.yaml

========================================================
VERIFICAR PODS
========================================================

kubectl get pods

========================================================
VERIFICAR SERVICES
========================================================

kubectl get svc

========================================================
LOGS
========================================================

API:

kubectl logs deployment/conexao-api -f

WORKER:

kubectl logs deployment/conexao-worker -f

========================================================
REINICIAR PODS
========================================================

kubectl rollout restart deployment conexao-api

kubectl rollout restart deployment conexao-worker

========================================================
REMOVER TUDO DO KUBERNETES
========================================================

kubectl delete -f k8s/

========================================================
LIMPAR DOCKER
========================================================

REMOVER CONTAINERS:

docker rm -f $(docker ps -aq)

REMOVER IMAGENS:

docker rmi -f $(docker images -aq)

REMOVER VOLUMES:

docker volume prune -f

========================================================
BANCO DE DADOS
========================================================

TABELAS:

- Users
- Campaigns
- __EFMigrationsHistory

========================================================
MIGRATIONS
========================================================

CRIAR MIGRATION:

dotnet ef migrations add InitialCreate --project services/ConexaoSolidaria.Api

APLICAR MIGRATION:

dotnet ef database update --project services/ConexaoSolidaria.Api

========================================================
FLUXO DO SISTEMA
========================================================

1. Usuário autentica
2. Cria campanha
3. Realiza doação
4. API publica evento RabbitMQ
5. Worker consome evento
6. Banco atualizado
7. Métricas monitoradas

========================================================
ENDPOINTS
========================================================

AUTH REGISTER

POST:
/api/Auth/register

JSON:

{
  "name": "Diego Amaro",
  "email": "diego@email.com",
  "password": "123456"
}

========================================================

AUTH LOGIN

POST:
/api/Auth/login

JSON:

{
  "email": "diego@email.com",
  "password": "123456"
}

========================================================

CAMPAIGN

POST:
/api/Campaign

HEADER:

Authorization: Bearer TOKEN

JSON:

{
  "title": "Campanha do Agasalho",
  "description": "Arrecadação de roupas",
  "startDate": "2026-05-20T00:00:00Z",
  "endDate": "2026-06-20T00:00:00Z",
  "financialGoal": 10000
}

========================================================

DONATION

POST:
/api/Donation

HEADER:

Authorization: Bearer TOKEN

JSON:

{
  "campaignId": 1,
  "donorName": "Maria Silva",
  "email": "maria@email.com",
  "phone": "11999999999",
  "description": "Doação de roupas",
  "quantity": 10,
  "amount": 500
}

========================================================
RABBITMQ
========================================================

VERIFICAR FILAS:

http://localhost:15672

Fila:
donation-queue

========================================================
PROMETHEUS
========================================================

Responsável por:
- Coletar métricas
- Monitorar containers
- Monitorar APIs
- Monitorar Kubernetes

========================================================
GRAFANA
========================================================

Responsável por:
- Dashboards
- Visualização de métricas
- Monitoramento em tempo real

========================================================
COMANDOS IMPORTANTES KUBERNETES
========================================================

PODS:

kubectl get pods

SERVICES:

kubectl get svc

DEPLOYMENTS:

kubectl get deployments

NODES:

kubectl get nodes

LOGS:

kubectl logs NOME_DO_POD

MONITORAMENTO TEMPO REAL:

kubectl get pods -w

========================================================
DEMONSTRAÇÃO PARA VÍDEO
========================================================

1. Mostrar arquitetura
2. Mostrar containers docker
3. Mostrar cluster kubernetes
4. Mostrar swagger
5. Criar usuário
6. Fazer login
7. Criar campanha
8. Realizar doação
9. Mostrar RabbitMQ
10. Mostrar worker consumindo mensagens
11. Mostrar Grafana
12. Mostrar Prometheus

========================================================
CARACTERÍSTICAS TÉCNICAS
========================================================

- Microsserviços
- Processamento assíncrono
- Mensageria
- Escalabilidade
- Observabilidade
- Containerização
- Kubernetes
- JWT
- DDD
- CQRS
- Worker Service

========================================================
AUTOR
========================================================

Diego Amaro

========================================================
LICENÇA
========================================================

Projeto acadêmico FIAP Tech Challenge.
