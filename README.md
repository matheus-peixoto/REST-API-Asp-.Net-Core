<h1 align="center">REST API Asp .Net Core</h1>
<p>API REST construída com Asp .Net Core para fazer operações de CRUD no registro de livros e autores em um relacionamento NxN entre essas tabelas utilizando dos verbos GET, POST, PUT, PATCH e DELETE.</p>

## Objetivo do projeto
Construção de uma Web API, API REST, para um possível negócio de livros fornecendo dados e funcionalidades sobre seus livros e autores.

## O que foi utilizado
Asp .Net Core Framework, banco de dados SQL Server com o ORM Entity Framework, abordagem Code First com Migrations, padrão Repository e padrão DTO com AutoMapper.

### Endpoints
Books
 * /books GET
 * /books/{id} GET
 * /books POST
 * /books POST
 * /books/{id} PUT
 * /books/{id} PATCH
 * /books/{id} DELETE
 
Authors
 * /authors GET
 * /authors/{id} GET
 * /authors POST
 * /authors POST
 * /authors/{id} PUT
 * /authors/{id} PATCH
 * /authors/{id} DELETE

### Dependências
* AutoMapper.Extensions.Microsoft.DependencyInjection versão 8.1.0
* Microsoft.AspNetCore.JsonPatch versão 3.1.11
* Microsoft.AspNetCore.Mvc.NewtonsoftJson versão 3.1.11
* Microsoft.EntityFrameworkCore versão 3.1.11
* Microsoft.EntityFrameworkCore.SqlServer versão 3.1.11
* Microsoft.EntityFrameworkCore.Tools versão 3.1.11
