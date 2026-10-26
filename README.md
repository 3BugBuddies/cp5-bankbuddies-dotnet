# Banco API — CP5 (Advanced Business Development with .NET)

API REST desenvolvida com **ASP.NET Core 8** e **EF Core** — avaliação **CP5 de Advanced Business Development with .NET**, FIAP 2026.

O serviço é o **back-office de uma operação bancária simplificada**: cadastro de **Clientes** e das **Contas** que cada cliente possui, com consulta de endereço por CEP integrada a um serviço externo.

Aplica os padrões pedidos na CP5: **Clean Architecture** em 4 camadas, **Operation Result**, **erros customizados** lançados no Repository e traduzidos em HTTP na Application, **paginação estruturada com HATEOAS**, **autenticação/autorização via JWT** e **testes automatizados** (unidade + integração).

---

## Índice

1. [Integrantes do Grupo](#integrantes-do-grupo)
2. [Stack e Dependências](#stack-e-dependências)
3. [Estrutura do Projeto](#estrutura-do-projeto)
4. [Como Executar](#como-executar)
5. [Modelo de Dados](#modelo-de-dados)
6. [Recursos e Rotas](#recursos-e-rotas)
7. [Autenticação](#autenticação)
8. [Como Testar](#como-testar)
9. [Exemplos de Payload (POST)](#exemplos-de-payload-post)
10. [Vídeo de Demonstração](#vídeo-de-demonstração)

---

## Links

| | |
|---|---|
| Swagger UI (local) | `http://localhost:5299/swagger` |
| Coleção Postman | [`docs/postman/banco-api.postman_collection.json`](docs/postman/banco-api.postman_collection.json) |
| Repositório | [3BugBuddies/cp5-bankbuddies-dotnet](https://github.com/3BugBuddies/cp5-bankbuddies-dotnet) |

---

## Integrantes do Grupo

| Nome | RM |
|------|----|
| Felipe Yuiti Ishii | 565339 |
| Gabriel Nogueira Peixoto | 563925 |
| Giovanna Neri dos Santos | 566154 |
| Mariana Inoue | 565834 |

---

## Stack e Dependências

| Pacote | Versão | Descrição |
|--------|--------|-----------|
| Microsoft.EntityFrameworkCore | 8.0.26 | ORM principal |
| Microsoft.EntityFrameworkCore.Relational | 8.0.26 | Fixado explicitamente para resolver conflito de versão trazido pelo driver Oracle |
| Oracle.EntityFrameworkCore | 8.23.26200 | Driver Oracle para EF Core |
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.31 | Emissão e validação do JWT |
| AspNetCore.HealthChecks.Oracle | 8.0.1 | Health check de conexão com o Oracle |
| Serilog.AspNetCore | 8.0.3 | Log estruturado (console + arquivo) |
| Swashbuckle.AspNetCore + Annotations + Filters | 6.6.2 / 6.6.2 / 6.1.0 | Swagger / OpenAPI UI, com exemplos de request/response |
| RestSharp | 114.0.0 | Cliente HTTP para o serviço externo (ViaCEP) |

**Testes** (só nos projetos `Banco.Tests.*`): xUnit 2.5.3, Moq 4.20.72, Microsoft.EntityFrameworkCore.InMemory 8.0.30, Microsoft.AspNetCore.Mvc.Testing 8.0.30.

---

## Estrutura do Projeto

<details>
<summary>Árvore de pastas</summary>

```
cp5-bankbuddies-dotnet/
├── Banco.API/                   (Presentation)
│   ├── Config/
│   │   └── ConfigurationAPI.cs  # Serilog
│   ├── Controllers/              # ClienteController, ContaController, AuthController, UserController, HealthController
│   ├── appsettings.json
│   ├── appsettings.Development.json         # não versionado — contém credencial real
│   ├── appsettings.Development.json.example # modelo para configurar localmente
│   └── Program.cs
├── Banco.Application/
│   ├── Dtos/            # PostXxxDto (entrada), GetXxxDto (saída, com Links), PageResponseDto, LinkDto
│   ├── Interfaces/      # IClienteUseCase, IContaUseCase, IUserUseCase
│   ├── Mappers/         # ClienteMapper, ContaMapper, UserMapper (extension methods)
│   └── UseCases/        # ClienteUseCase, ContaUseCase, UserUseCase
├── Banco.Domain/
│   ├── Entities/         # ClienteEntity, ContaEntity, UserEntity, OperationResult<T>, EnderecoModel
│   ├── Errors/
│   │   └── NoContentException.cs
│   └── Interfaces/       # IClienteRepository, IContaRepository, IClienteService, IUserRepository
├── Banco.Infrastructure/
│   ├── Data/
│   │   ├── ApplicationContext.cs
│   │   ├── Migrations/
│   │   └── Repositories/  # ClienteRepository, ContaRepository, UserRepository
│   └── Service/
│       └── ClienteService.cs     # RestSharp → ViaCEP
├── Banco.IoC/
│   └── Bootstrap.cs       # Injeção de dependência
├── Banco.Tests.Unit/        # Repository (EF Core InMemory) e UseCase (Moq)
├── Banco.Tests.Integration/ # Controller (WebApplicationFactory + UseCase mockado)
├── Banco.slnx
├── .gitignore
└── README.md
```

</details>

**Camadas e dependências:** `Banco.API` → `Banco.Application`, `Banco.Domain`, `Banco.IoC` · `Banco.Application` → `Banco.Domain`, `Banco.Infrastructure` · `Banco.Infrastructure` → `Banco.Domain` · `Banco.IoC` → `Banco.Application`, `Banco.Infrastructure`. O `Banco.Domain` não referencia nenhuma outra camada.

---

## Como Executar

### Pré-requisitos

- .NET 8 SDK (compilado e testado com o SDK 10, multi-targeting `net8.0`)
- Acesso ao Oracle da FIAP (`oracle.fiap.com.br:1521/ORCL`) com usuário (RM) e senha

### 1. Configuração

Copie o arquivo de exemplo e preencha com sua credencial do Oracle FIAP:

```bash
cp Banco.API/appsettings.Development.json.example Banco.API/appsettings.Development.json
```

Edite `Banco.API/appsettings.Development.json`:

```json
"ConnectionStrings": {
  "Oracle": "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))) (CONNECT_DATA=(SERVER=DEDICATED)(SID=ORCL)));User Id=SEU_RM;Password=SUA_SENHA;"
},
"Secretkey": "qualquer string com 32+ caracteres, só para desenvolvimento"
```

> Esse arquivo está no `.gitignore` — nunca é commitado, exatamente para não vazar a senha do Oracle.

### 2. Compilar

```bash
dotnet build Banco.slnx
```

### 3. Aplicar as migrations

Cria as tabelas `tb_cliente`, `tb_conta` e `tb_usuario` no Oracle configurado acima:

```bash
dotnet ef database update --project Banco.Infrastructure/Banco.Infrastructure.csproj --startup-project Banco.API/Banco.API.csproj --context Banco.API.Infrastructure.Data.ApplicationContext
```

### 4. Rodar

```bash
dotnet run --project Banco.API/Banco.API.csproj
```

A API sobe em `http://localhost:5299` (ou na porta definida pelo ambiente), com Swagger em `/swagger`.

### 5. Pegue um token

Não há um endpoint de "token de desenvolvimento" — é preciso cadastrar um usuário e autenticar, como em produção:

```bash
curl -s -X POST http://localhost:5299/api/user -H "Content-Type: application/json" \
  -d '{"nome":"Admin","email":"admin@teste.com","password":"Senha123!","cofirmPassword":"Senha123!","role":"admin"}'

TOKEN=$(curl -s -X POST http://localhost:5299/api/auth -H "Content-Type: application/json" \
  -d '{"email":"admin@teste.com","senha":"Senha123!"}' | sed -n 's/.*"jwt":"\([^"]*\)".*/\1/p')

curl -s http://localhost:5299/api/cliente -H "Authorization: Bearer $TOKEN"
```

No Swagger, o caminho é o mesmo: `POST /api/user` → `POST /api/auth` → copiar o `jwt` da resposta → botão **Authorize** → colar `Bearer <token>`.

---

## Modelo de Dados

| Entidade | Tabela | Campos | Relação |
|---|---|---|---|
| `ClienteEntity` | `tb_cliente` | Id, Nome, Email (único), Cpf, Idade | 1 Cliente → N Contas |
| `ContaEntity` | `tb_conta` | Id, NumeroConta (único), Agencia, Saldo, TipoConta, ClienteId (FK) | N Contas → 1 Cliente (`ON DELETE CASCADE`) |
| `UserEntity` | `tb_usuario` | Id, Nome, Email, Password, Role | usado só para autenticação |

---

## Recursos e Rotas

| Recurso | Rotas | Filtros/parâmetros | Proteção | Status codes |
|---|---|---|---|---|
| Cliente | `GET` `/api/cliente` | `Deslocamento`, `RegistroRetornado` (paginação) | `Authorize(Roles = admin,gerente)` + rate limit (5/20s) | `200` `204` `400` `401` `403` `429` |
| Cliente | `GET` `/api/cliente/{id}` | — | público | `200` `404` `400` |
| Cliente | `POST` `/api/cliente` | — | `Authorize(Roles = admin)` | `200` `400` `401` `403` |
| Cliente | `PUT` `/DELETE` `/api/cliente/{id}` | — | público | `200` `404` `400` |
| Cliente | `GET` `/api/cliente/endereco/{cep}` | consome o ViaCEP | público | `200` `404` `400` |
| Conta | `GET` `/api/conta` | `Deslocamento`, `RegistroRetornado` | público | `200` `204` `400` |
| Conta | `GET` `/api/conta/{id}` | — | público | `200` `404` `400` |
| Conta | `GET` `/api/conta/cliente/{clienteId}` | — | público | `200` `204` `400` |
| Conta | `POST` `/api/conta` | — | `Authorize(Roles = admin)` | `200` `400` `401` `403` |
| Conta | `PUT` `/api/conta/{id}` | — | `Authorize(Roles = admin,gerente)` | `200` `404` `400` `401` `403` |
| Conta | `DELETE` `/api/conta/{id}` | — | `Authorize(Roles = admin)` | `200` `404` `400` `401` `403` |
| User | `POST` `/api/user` | cadastro | público | `200` `400` |
| Auth | `POST` `/api/auth` | login → retorna JWT | público | `200` `404` `400` |
| Health | `GET` `/health/live`, `/health/db` | — | público | `200` `503` |
| Health | `GET` `/api/health2/live`, `/api/health2/db` | — | público | `200` `503` |

Listagem vazia devolve `204` sem corpo. Recurso não encontrado devolve `404` com uma mensagem de erro no corpo.

---

## Autenticação

A API usa JWT `HS256`, assinado com a `Secretkey` de `appsettings.Development.json` — `AddAuthentication().AddJwtBearer()` em `Program.cs`.

- A claim de papel (`ClaimTypes.Role`) vem do campo `Role` do `UserEntity` (ex.: `admin`, `gerente`).
- Endpoints de escrita (`POST`/`PUT`/`DELETE` de Cliente e Conta) exigem `[Authorize(Roles = "...")]`: sem token → `401 Unauthorized`; token com papel incorreto → `403 Forbidden`.
- Endpoints de leitura de Conta, consulta de CEP e `health checks` são públicos.
- Token válido por 8 horas.

---

## Como Testar

### Via Swagger UI

Com um token obtido em `POST /api/auth` colado no botão **Authorize**, todos os endpoints protegidos ficam disponíveis com "Try it out" em `http://localhost:5299/swagger`.

### Via testes automatizados

Dois projetos xUnit, separados por tipo de teste:

```
Banco.Tests.Unit/
├── ClienteRepositoryTest.cs   # EF Core InMemory
├── ClienteUseCaseTest.cs      # Moq sobre o repositório
├── ContaRepositoryTest.cs     # EF Core InMemory
└── ContaUseCaseTest.cs        # Moq sobre o repositório

Banco.Tests.Integration/
├── ClienteControllerTest.cs   # WebApplicationFactory + UseCase mockado
└── ContaControllerTest.cs     # WebApplicationFactory + UseCase mockado
```

Rodar tudo:

```bash
dotnet test Banco.slnx
```

**9 testes, todos passando** (7 no `Banco.Tests.Unit`, 2 no `Banco.Tests.Integration`). Os testes de Repository rodam contra `Microsoft.EntityFrameworkCore.InMemory` — não precisam do Oracle.

### Via Postman

A coleção em [`docs/postman/banco-api.postman_collection.json`](docs/postman/banco-api.postman_collection.json) cobre todos os endpoints, organizados em pastas:

- **0 - Autenticação**: cria o usuário admin e faz login, salvando o token automaticamente.
- **1 - Cliente** / **2 - Conta**: CRUD completo, incluindo uma requisição sem token para demonstrar o `401`.
- **3 - Health**: os quatro health checks.
- **4 - Carga (Collection Runner)**: cria Cliente + Conta com dados aleatórios (`{{$randomEmail}}`, CPF e número de conta gerados por script) — pensada para rodar com muitas iterações no **Collection Runner** do Postman (ou via `newman run ... -n 50`) sem esbarrar nos índices únicos de Email/NumeroConta.
- **5 - Limpeza**: remove o cliente e a conta criados nas pastas 1 e 2 (rodar por último, manualmente).

Importe o arquivo no Postman, rode a pasta **0** primeiro (para ter um token válido) e depois as demais na ordem.

---

## Exemplos de Payload (POST)

#### `POST /api/cliente`
> Requer `Authorization: Bearer <token com role admin>`
```json
{
  "nome": "João da Silva",
  "email": "joao@email.com",
  "cpf": "12345678900",
  "idade": 30
}
```

#### `POST /api/conta`
> Requer `Authorization: Bearer <token com role admin>`
```json
{
  "numeroConta": "1001",
  "agencia": "0001",
  "saldo": 500.00,
  "tipoConta": "Corrente",
  "clienteId": 1
}
```

#### `POST /api/user`
```json
{
  "nome": "Admin",
  "email": "admin@teste.com",
  "password": "Senha123!",
  "cofirmPassword": "Senha123!",
  "role": "admin"
}
```

#### `POST /api/auth`
```json
{
  "email": "admin@teste.com",
  "senha": "Senha123!"
}
```

---

## Vídeo de Demonstração

[PREENCHER: link do vídeo (máx. 5 minutos) após a gravação]
