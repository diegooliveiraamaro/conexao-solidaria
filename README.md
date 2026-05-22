# CONEXÃO SOLIDÁRIA

Sistema backend de doações e campanhas desenvolvido com arquitetura baseada em microsserviços utilizando .NET 8, PostgreSQL, RabbitMQ, Docker, Kubernetes, Prometheus e Grafana.

==================================================
TECNOLOGIAS UTILIZADAS
==================================================

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

==================================================
ARQUITETURA DO PROJETO
==================================================

MICROSSERVIÇOS

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

==================================================
ESTRUTURA DO PROJETO
==================================================

conexao-solidaria/
│
├── services/
│   ├── ConexaoSolidaria.Api
│   └── ConexaoSolidaria.Worker
│
├── shared/
│   └── ConexaoSolidaria.Shared
│
├── k8s/
│
├── docker-compose.yml
│
└── README.md

==================================================
PRÉ-REQUISITOS
==================================================

INSTALAR:

1 - Docker Desktop
https://www.docker.com/products/docker-desktop/

Habilitar:
- Kubernetes
- WSL2

==================================================
VERIFICAR INSTALAÇÕES
==================================================

DOCKER

docker --version

KUBERNETES

kubectl version --client

.NET

dotnet --version

==================================================
SUBINDO O PROJETO LOCALMENTE
==================================================

1 - CLONAR PROJETO

git clone https://github.com/seu-repositorio/conexao-solidaria.git

cd conexao-solidaria

==================================================
2 - RESTAURAR DEPENDÊNCIAS
==================================================

dotnet restore

==================================================
3 - BUILDAR PROJETO
==================================================

dotnet build

==================================================
4 - SUBIR INFRAESTRUTURA DOCKER
==================================================

docker compose up -d

==================================================
5 - VERIFICAR CONTAINERS
==================================================

docker ps

Deve aparecer:
- postgres
- rabbitmq
- grafana
- prometheus

==================================================
6 - RODAR API
==================================================

dotnet run --project services/ConexaoSolidaria.Api

==================================================
7 - RODAR WORKER
==================================================

dotnet run --project services/ConexaoSolidaria.Worker

==================================================
URLS LOCAIS
==================================================

SWAGGER

http://localhost:5096/swagger

--------------------------------------------------

RABBITMQ

http://localhost:15672

LOGIN:
usuario: guest
senha: guest

--------------------------------------------------

GRAFANA

http://localhost:3000

LOGIN:
usuario: admin
senha: admin

--------------------------------------------------

PROMETHEUS

http://localhost:9090

==================================================
DOCKER
==================================================

BUILD API

docker build -t conexao-api -f services/ConexaoSolidaria.Api/Dockerfile .

--------------------------------------------------

BUILD WORKER

docker build -t conexao-worker -f services/ConexaoSolidaria.Worker/Dockerfile .

--------------------------------------------------

VERIFICAR IMAGENS

docker images

==================================================
KUBERNETES
==================================================

HABILITAR KUBERNETES

Docker Desktop:
Settings > Kubernetes > Enable Kubernetes

--------------------------------------------------

VERIFICAR CLUSTER

kubectl get nodes

Resultado esperado:
docker-desktop Ready control-plane

==================================================
APLICAR DEPLOYMENTS
==================================================

POSTGRESQL

kubectl apply -f k8s/postgres-deployment.yaml

--------------------------------------------------

RABBITMQ

kubectl apply -f k8s/rabbitmq-deployment.yaml

--------------------------------------------------

API

kubectl apply -f k8s/api-deployment.yaml

--------------------------------------------------

WORKER

kubectl apply -f k8s/worker-deployment.yaml

==================================================
VERIFICAR PODS
==================================================

kubectl get pods

Resultado esperado:
- conexao-api
- conexao-worker
- postgres
- rabbitmq

==================================================
VERIFICAR SERVIÇOS
==================================================

kubectl get svc

==================================================
VER LOGS
==================================================

API

kubectl logs deployment/conexao-api -f

--------------------------------------------------

WORKER

kubectl logs deployment/conexao-worker -f

==================================================
REINICIAR PODS
==================================================

kubectl rollout restart deployment conexao-api

kubectl rollout restart deployment conexao-worker

==================================================
REMOVER TUDO KUBERNETES
==================================================

kubectl delete -f k8s/

==================================================
LIMPAR DOCKER COMPLETO
==================================================

CONTAINERS

docker rm -f $(docker ps -aq)

--------------------------------------------------

IMAGENS

docker rmi -f $(docker images -aq)

--------------------------------------------------

VOLUMES

docker volume prune -f

==================================================
BANCO DE DADOS
==================================================

POSTGRESQL

Tabelas principais:
- Users
- Campaigns
- __EFMigrationsHistory

==================================================
MIGRATIONS
==================================================

CRIAR MIGRATION

dotnet ef migrations add InitialCreate --project services/ConexaoSolidaria.Api

--------------------------------------------------

APLICAR MIGRATION

dotnet ef database update --project services/ConexaoSolidaria.Api

==================================================
FLUXO DO SISTEMA
==================================================

1 - Usuário autentica
2 - Cria campanha
3 - Realiza doação
4 - API publica evento RabbitMQ
5 - Worker consome evento
6 - Banco atualizado
7 - Métricas monitoradas

==================================================
ENDPOINTS
==================================================

==================================================
AUTENTICAÇÃO
==================================================

POST
/api/Auth/register

JSON

{
  "name": "Diego Amaro",
  "email": "diego@email.com",
  "password": "123456"
}

--------------------------------------------------

LOGIN

POST
/api/Auth/login

JSON

{
  "email": "diego@email.com",
  "password": "123456"
}

==================================================
CAMPAIGN
==================================================

POST
/api/Campaign

HEADER

Authorization: Bearer TOKEN

JSON

{
  "title": "Campanha do Agasalho",
  "description": "Arrecadação de roupas",
  "startDate": "2026-05-20T00:00:00Z",
  "endDate": "2026-06-20T00:00:00Z",
  "financialGoal": 10000
}

==================================================
DONATION
==================================================

POST
/api/Donation

HEADER

Authorization: Bearer TOKEN

JSON

{
  "campaignId": 1,
  "donorName": "Maria Silva",
  "email": "maria@email.com",
  "phone": "11999999999",
  "description": "Doação de roupas",
  "quantity": 10,
  "amount": 500
}

==================================================
RABBITMQ
==================================================

VERIFICAR FILAS

http://localhost:15672

Fila:
donation-queue

==================================================
PROMETHEUS
==================================================

FUNÇÃO:
- Coletar métricas
- Monitorar containers
- Monitorar APIs
- Monitorar Kubernetes

==================================================
GRAFANA
==================================================

FUNÇÃO:
- Dashboards
- Visualização de métricas
- Monitoramento em tempo real

==================================================
COMANDOS KUBERNETES IMPORTANTES
==================================================

PODS

kubectl get pods

--------------------------------------------------

SERVICES

kubectl get svc

--------------------------------------------------

DEPLOYMENTS

kubectl get deployments

--------------------------------------------------

NODES

kubectl get nodes

--------------------------------------------------

LOGS

kubectl logs NOME_DO_POD

--------------------------------------------------

MONITORAMENTO TEMPO REAL

kubectl get pods -w

==================================================
DEMONSTRAÇÃO PARA VÍDEO
==================================================

1 - Mostrar arquitetura
- Microsserviços
- RabbitMQ
- PostgreSQL
- Kubernetes

--------------------------------------------------

2 - Mostrar containers

docker ps

--------------------------------------------------

3 - Mostrar cluster

kubectl get nodes

kubectl get pods

--------------------------------------------------

4 - Mostrar Swagger

Criar:
- usuário
- login
- campanha
- doação

--------------------------------------------------

5 - Mostrar RabbitMQ

Fila:
donation-queue

--------------------------------------------------

6 - Mostrar Worker consumindo mensagens

kubectl logs deployment/conexao-worker -f

--------------------------------------------------

7 - Mostrar Grafana
- dashboards
- métricas

--------------------------------------------------

8 - Mostrar Prometheus
- targets
- métricas

==================================================
CARACTERÍSTICAS TÉCNICAS
==================================================

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

==================================================
AUTOR
==================================================

Diego Amaro

==================================================
LICENÇA
==================================================

Projeto acadêmico FIAP Tech Challenge.
