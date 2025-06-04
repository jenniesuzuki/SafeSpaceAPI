# SafeSpace

A SafeSpace é um aplicativo que conecta vítimas de desastres naturais a psicólogos voluntários para atendimento remoto, auxiliando na gestão do trauma e na redução de impactos emocionais negativos, além de facilitar a distribuição e solicitação de recursos para vítimas que estão com alguma necessidade; 

## Destaques da solução
- ✅ Agendamento de sessões psicológicas remotas.
- ✅ Solicitações de ajuda (pedidos de recursos como transporte, alimentação).
- ✅ Cadastro de voluntários e ONGs.
- ✅ Cadastro de vítimas de desastres naturais.

## Funcionalidades

- *Cadastro de usuários, solicitações de ajuda e agendamentos:* Permite o registro no sistema de informações.

- *Atualização de Informações:* Possibilidade de editar ou excluir informações.

- *Visualização de Informações:* Exibe os detalhes da categoria desejada, garantindo transparência e facilidade no gerenciamento de dados.

- *Projeto em .NET*

- *Banco de Dados Oracle*

## Requisitos

- Git
- Docker

## Execução do projeto

No Azure CLI, digite: 

```json
vi deploy_script.sh
```

Digite "i" para inserir o seguinte script:
```json
RESOURCE_GROUP="meuGrupoDeRecursos"
LOCATION="brazilsouth"  # ou sua região preferida
VM_NAME="minhaVM"
ADMIN_USERNAME="azureuser"
IMAGE="almalinux:almalinux-x86_64:9-gen2:9.5.202411260"
PORT=8080

az group create --name $RESOURCE_GROUP --location $LOCATION

az vm create \
  --resource-group $RESOURCE_GROUP \
  --name $VM_NAME \
  --image $IMAGE \
  --admin-username $ADMIN_USERNAME \
  --generate-ssh-keys \
  --public-ip-sku Standard

az vm open-port --resource-group $RESOURCE_GROUP --name $VM_NAME --port $PORT

echo "Esperando provisionamento..."
sleep 10

echo "Conecte na VM via ssh e execute os comandos de instalação do Docker"

echo "ssh $ADMIN_USERNAME@IP_DA_VM"

cat << EOF
# Após conectar na VM com:
ssh $ADMIN_USERNAME@IP_DA_VM

# Execute os seguintes comandos na VM:

sudo dnf config-manager --add-repo https://download.docker.com/linux/centos/docker-ce.repo
sudo dnf install -y docker-ce docker-ce-cli containerd.io
sudo systemctl start docker
sudo systemctl enable docker
sudo yum install -y git
sudo dnf install -y maven
git clone https://github.com/felipe-2833/mottuGestor.git
cd mottuGestor
mvn clean package
docker build -t myapp-image .
docker run -d -p 8080:8080 myapp-image
curl http://IP_DA_VM:8080/swagger-ui/index.html

EOF
```

Depois de colar, pressione Esc, digite: ":wq"

```json
chmod +x deploy_script.sh
```

```json
./deploy_script.sh
```

Depois, siga as instruções listadas!


Comandos a serem executados após o a execução do script e da conexão na VM (Certifique-se que o IP da VM está correto):
```json
sudo dnf config-manager --add-repo https://download.docker.com/linux/centos/docker-ce.repo
```
```json
sudo dnf install -y docker-ce docker-ce-cli containerd.io
```
```json
sudo systemctl start docker
sudo systemctl enable docker
```
```json
sudo yum install -y git
```
```json
sudo dnf install -y maven
```
```json
git clone https://github.com/felipe-2833/mottuGestor.git
```
```json
cd mottuGestor
```
```json
mvn clean package
```
```json
sudo docker build -t myapp-image .
```
```json
sudo docker run -d -p 8080:8080 myapp-image
```
```json
curl http://(PUBLIC_IP):8080/swagger-ui/index.html
```


## Instruções para envio de requisições

Acesse o Swagger: http://(PUBLIC_IP):8080/swagger-ui/index.html

### 1. Criar Solicitação de ajuda (POST)
**Endpoint**: `POST /SolicitacaoAjuda`

Passos:

1. Clique em POST /SolicitacaoAjuda

2. Clique em "Try it out"

3. Substitua o exemplo pelo JSON abaixo:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "descricao": "Preciso de remédios",
  "dataSolicitacao": "2025-06-04T12:57:01.765Z"
}
```
4. Clique em "Execute"

### 2. Listar todas as solicitações (GET)
**Endpoint**: `GET /SolicitacaoAjuda`

### 3. Buscar solicitação por ID (GET)
**Endpoint**: `GET /SolicitacaoAjuda/{id_solicitacao}`

Passos:

1. Clique em GET /SolicitacaoAjuda/{id_solicitacao}

2. Clique em "Try it out"

3. Preencha o id_solicitacao (ex: 3fa85f64-5717-4562-b3fc-2c963f66afa6)

4. Clique em "Execute"

### 4. Atualizar solicitação (PUT)
**Endpoint**: `PUT /SolicitacaoAjuda/{id_solicitacao}`

1. Clique em PUT /SolicitacaoAjuda/{id_solicitacao}

2. Clique em "Try it out"

3. Preencha o id_solicitacao (deve bater com o ID no JSON, ex: 3fa85f64-5717-4562-b3fc-2c963f66afa6)

4. Substitua o corpo com:

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "descricao": "Preciso de alimentos",
  "dataSolicitacao": "2025-06-04T12:59:40.859Z"
}
```

5. Clique em "Execute"

### 5. Apagar solicitação (DELETE)
**Endpoint**: `/SolicitacaoAjuda/{id_solicitacao}`

1. Clique em DELETE /SolicitacaoAjuda/{id_solicitacao}

2. Clique em "Try it out"

3. Preencha o id_solicitacao (ex: 3fa85f64-5717-4562-b3fc-2c963f66afa6)

4. Clique em "Execute"


## Equipe

- Felipe Levy Stephens Fidelix - *RM: 556426*
- Jennifer Kaori Suzuki - *RM: 554661*
- Samir Hage Neto - *RM: 557260*
