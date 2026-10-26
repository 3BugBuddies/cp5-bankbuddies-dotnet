# Banco API — CP5 (Advanced Business Development with .NET)

API RESTful em **ASP.NET Core 8** para um domínio bancário simplificado, construída com **Clean Architecture**, desenvolvida como avaliação CP5 da disciplina de desenvolvimento com .NET.

## Descrição do projeto

A API gerencia **Clientes** e **Contas bancárias** (1 Cliente → N Contas), aplicando os padrões e práticas avançadas pedidas na CP5:

- Clean Architecture (Domain, Application, Infrastructure, API)
- Otimização de consultas (índices únicos, paginação, compressão de resposta, rate limiting)
- Testes automatizados (unidade e funcionais/integração)
- Padrão `OperationResult<T>`
- Erros customizados (`NoContentException`, lançados no Repository e capturados na Application)
- Chamada de serviço externo (ViaCEP, para endereço por CEP)
- Paginação estruturada com `PageResponseDto<T>` + HATEOAS (`LinkDto`)
- Autenticação e Autorização via JWT, com controle por Roles

## Arquitetura e componentes utilizados

```
Banco.slnx
├── Banco.API            (Presentation) — Controllers, Program.cs, Swagger, JWT, Health Checks
├── Banco.Application     — DTOs, Mappers, UseCases, interfaces de UseCase
├── Banco.Domain          — Entities, OperationResult<T>, NoContentException, interfaces de Repository/Service
├── Banco.Infrastructure  — ApplicationContext (EF Core + Oracle), Repositories, ClienteService (RestSharp/ViaCEP)
├── Banco.IoC             — Bootstrap.cs (injeção de dependência)
└── Banco.Tests           — Testes de Repository (EF InMemory), UseCase (Moq) e Controller (WebApplicationFactory)
```

**Principais tecnologias:** .NET 8, Entity Framework Core 8 (`Oracle.EntityFrameworkCore`), JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`), Swashbuckle (Swagger + Annotations + Filters), Serilog, RestSharp, xUnit + Moq + EF Core InMemory + `Microsoft.AspNetCore.Mvc.Testing`.

### Entidades

| Entidade | Tabela | Campos principais |
|---|---|---|
| `ClienteEntity` | `tb_cliente` | Id, Nome, Email (único), Cpf, Idade |
| `ContaEntity` | `tb_conta` | Id, NumeroConta (único), Agencia, Saldo, TipoConta, ClienteId (FK) |
| `UserEntity` | `tb_usuario` | Id, Nome, Email, Password, Role (autenticação) |

### Principais endpoints

| Método | Rota | Proteção |
|---|---|---|
| GET | `/api/cliente?Deslocamento=&RegistroRetornado=` | `Authorize(Roles = admin,gerente)` |
| GET | `/api/cliente/{id}` | público |
| POST | `/api/cliente` | `Authorize(Roles = admin)` |
| PUT/DELETE | `/api/cliente/{id}` | público / sem restrição extra |
| GET | `/api/cliente/endereco/{cep}` | público (consome ViaCEP) |
| GET | `/api/conta?Deslocamento=&RegistroRetornado=` | público |
| GET | `/api/conta/{id}` | público |
| GET | `/api/conta/cliente/{clienteId}` | público |
| POST | `/api/conta` | `Authorize(Roles = admin)` |
| PUT | `/api/conta/{id}` | `Authorize(Roles = admin,gerente)` |
| DELETE | `/api/conta/{id}` | `Authorize(Roles = admin)` |
| POST | `/api/user` | público (cadastro) |
| POST | `/api/auth` | público (login, retorna JWT) |
| GET | `/api/health2/live`, `/api/health2/db` | público |

## Configurações necessárias

### Banco de dados (Oracle)

A connection string fica em `Banco.API/appsettings.Development.json` (arquivo **não versionado** — veja `.gitignore`). Use `Banco.API/appsettings.Development.json.example` como modelo:

```json
"ConnectionStrings": {
  "Oracle": "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))) (CONNECT_DATA=(SERVER=DEDICATED)(SID=ORCL)));User Id=SEU_RM;Password=SUA_SENHA;"
}
```

Copie o arquivo e preencha com suas credenciais:

```bash
cp Banco.API/appsettings.Development.json.example Banco.API/appsettings.Development.json
```

### JWT

Defina `Secretkey` no mesmo arquivo (qualquer string com 32+ caracteres, usada apenas em desenvolvimento).

### Serviço externo

Nenhuma configuração extra é necessária — o `ClienteService` consome a API pública do ViaCEP (`https://viacep.com.br/ws/`) diretamente.

## Como rodar o projeto

```bash
# 1. Restaurar e compilar
dotnet build Banco.slnx

# 2. Aplicar as migrations no banco Oracle (cria tb_cliente, tb_conta, tb_usuario)
dotnet ef database update --project Banco.Infrastructure/Banco.Infrastructure.csproj --startup-project Banco.API/Banco.API.csproj --context Banco.API.Infrastructure.Data.ApplicationContext

# 3. Rodar a API
dotnet run --project Banco.API/Banco.API.csproj
```

A API sobe em `http://localhost:5299` (ou porta definida pelo ambiente) com Swagger disponível em `/swagger`.

### Fluxo básico de teste manual (via Swagger ou curl)

1. `POST /api/user` — cria um usuário com `role: "admin"`.
2. `POST /api/auth` — autentica e retorna o JWT.
3. Use o botão **Authorize** do Swagger (ou header `Authorization: Bearer <token>`) para os endpoints protegidos.
4. `POST /api/cliente`, depois `POST /api/conta` referenciando o `clienteId`.
5. `GET /api/cliente`, `GET /api/conta` — paginação e HATEOAS nas respostas.
6. `GET /api/cliente/endereco/{cep}` — demonstra a chamada ao serviço externo.

## Como executar os testes

```bash
dotnet test Banco.slnx
```

9 testes automatizados, cobrindo:
- **Repository** (EF Core InMemory): `ClienteRepositoryTest`, `ContaRepositoryTest`
- **UseCase** (Moq): `ClienteUseCaseTest`, `ContaUseCaseTest`
- **Controller** (integração, `WebApplicationFactory`): `ClienteControllerTest`, `ContaControllerTest`

## Vídeo de demonstração

[PREENCHER: link do vídeo (máx. 5 minutos) após a gravação]

## Integrantes do grupo

[PREENCHER: Nome completo e RM de cada integrante]
