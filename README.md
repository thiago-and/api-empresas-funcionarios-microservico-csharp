# Cadastro de Empresas e Funcionários

## Visão geral
Este projeto contém duas APIs ASP.NET Core 8.0 que compartilham o mesmo banco de dados MySQL:

- `CadastroEmpresas.Api`: CRUD completo para empresas, com validações de CNPJ, telefone e endereço.
- `CadastroFuncionarios.Api`: CRUD para colaboradores, incluindo cargo, salário e data de admissão.

Ambas usam o provedor Pomelo para Entity Framework Core e foram geradas a partir da mesma solução `ApiEmpresas.sln`.

## Arquitetura
- Cada API define controllers com rotas em `api/v1/empresas` e `api/v1/funcionarios`, respectivamente.
- O `ApplicationDbContext` (em `Data/`) registra as entidades `Empresa` e `Funcionario`, que usam DTOs para criar/atualizar registros.
- Há middleware global que transforma exceções do MySQL ou do EF Core em `ProblemDetails` padronizadas.

## Banco de dados
- O projeto espera um MySQL 8.0 acessível em `localhost:3306` usando a base `empresas_db`.
- Cada API carrega `../.env` e monta a connection string a partir de `MYSQL_HOST`, `MYSQL_PORT`, `MYSQL_USER` e `MYSQL_PASSWORD` (a base continua sendo `empresas_db`).
- Defina valores diferentes no `.env` para ajustar host, porta e credenciais sem tocar no código.
- Exemplo mínimo funcional de `.env`:

```
MYSQL_HOST=localhost
MYSQL_PORT=3306
MYSQL_ROOT_PASSWORD=root
MYSQL_USER=user
MYSQL_PASSWORD=root
MYSQL_DATA_PATH=./Data
```
- Crie uma pasta `Data` na raiz do projeto.
- Mais exemplos do `MYSQL_DATA_PATH` para usar a pasta de outro local:
```
MYSQL_DATA_PATH=C:/Users/SeuUsuario/Downloads/ApiEmpresas/Data
```

## Execução
1. **Pré-requisitos**
   - .NET SDK 8.0.416.
   - Docker Desktop/Compose.

2. **Suba o MySQL**
   - Crie um arquivo `.env` na raiz com as variáveis do item anterior. Copie o exemplo acima.
   - Crie uma pasta `Data` na raiz.
   - Execute `docker compose up -d mysql` para iniciar o banco.

3. **Atualize o banco**
   - Aguarde até 1 minuto para o banco de dados iniciar completamente e execute:
   ```bash
   dotnet ef database update --project CadastroEmpresas.Api
   dotnet ef database update --project CadastroFuncionarios.Api
   ```

4. **Execute as APIs**
   ```bash
   dotnet run --project CadastroEmpresas.Api
   dotnet run --project CadastroFuncionarios.Api
   ```
   Elas escutam as URIs definidas no `.env` (por exemplo, `https://localhost:5001/swagger` para empresas e `https://localhost:5002/swagger` para funcionários).

## Endpoints principais
- `GET /api/v1/empresas` e `GET /api/v1/empresas/{id}`
- `POST /api/v1/empresas` — payload: `{ "nome": "...", "cnpj": "14 dígitos", "endereco": "...", "telefone": "5511999999999" }`
- `PUT /api/v1/empresas/{id}` — atualiza mesma estrutura.
- `DELETE /api/v1/empresas/{id}`
- `GET /api/v1/funcionarios` e `GET /api/v1/funcionarios/{id}`
- `POST /api/v1/funcionarios` — payload: `{ "nome": "...", "cargo": "...", "salario": 3500.00, "dataAdmissao": "2025-01-01" }`
- `PUT /api/v1/funcionarios/{id}` e `DELETE /api/v1/funcionarios/{id}`

## Testes rápidos
- O Swagger de cada API abre automaticamente na raiz (`/swagger`) quando o serviço está em execução.
- Os arquivos `EmpresasExemplos.json` e `FuncionariosExemplos.json` na raiz do projeto contêm exemplos de requisições.
- O arquivo `CadastroEmpresas.Api.http` (e o correspondente `CadastroFuncionarios.Api.http`) reúne sugestões de chamadas HTTP rápidas para enviar via REST Client.
