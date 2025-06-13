# Cach Flow

## Sobre o projeto

Esta API desenvolvieda utilizando .NET 8 adota os principios **Domain-Driven Design (DDD)** para oferecer uma solução estruturada e eficaz no gerenciamento de despesas pessoais. O principal objetivo é permitir que os usúarios registrem suas despesas, detalhano informações como titulo, data e hora, descrição, valor e tipo de pagamento, com os dados sendo armazenados de forma segura em uma bando de dados **MySQL**.

A Arquitetura da **API** basea-se em **REST**, utilizando métodos **HTTP** padrão para uma comunicação eficiente e simplificada. Além disso é complementada por uma documentação **Swagger**, que proporciona uma interface grafica interativa ppara que os desenvolvedores possam explorar e testar os endpoints de maneira fácil.

Dentre os pacotes NuGet utilizados, o **AutoMapper** é responsável pelo mapeamento entre objetos de dominio e requisição/resposta, reduzindo a necessidade de código repetitivo e manual. O **FluentAssertions** é utilizado nos testes de unidade para tornar as verificaçãoes mais legiveis ajudando a escrever teste claros e compreensíveis. Para as validções o **FluentValidation** é usado para implementar regras de validação de forma simples e intuitiva nas classe de requisições, mantendo o código limipo e fácil de manter. Por fim o **EntityFramework** atua como um ORM(Object Relationl Mapper) que simplifica as integraçoes com o banco de dados, permitindo o uso de objetos .NET para manipular dados diretamente sem a necessidade de lidar com consultas SQL.

![hero-image]

### Features

- **Domain-Driven Design (DDD):** Estrutura modular que facilita o entendimento e a manutenção do domínio da aplicação.
- **Teste de Unidade:** Testes abrangentes com FluentAssertions para garantir a funcionalidade e a qualidade.
- **Geração de Relatórios:** Capacidade de exportar relatórios detalhados para **PDF e Excel**, oferecendo uma análise visual e eficaz das despesas.
- **RESTful API com Documentação Swagger:** Interface documentação que facilita a integração e o teste por parte dos desenvolvedores.

### Construído com

![badge-dot-net]
![badge-mysql]
![badge-swagger]

## Getting Started

Para obter uma cópio local funcionando, siga estes passos simples.

### Requisitos

- Visual Studio versão 2022+ ou Visual Studio Code
- Winddows 10+ ou Linux/MacOS com [.NET SDK][dot-net-sdk] instalado
- MySql Server

### Instalação

1. Clone o repositório:

    ```sh
    https://github.com/marcoservio/cash-flow.git
    ```

2. Preencha as informações no arquivo `appsettings.Development.json`.
3. Execute a API e aproveite o seu teste :)

<!-- Links -->
[dot-net-sdk]: https://dotnet.microsoft.com/pt-br/download/dotnet/8.0

<!-- Images -->
[hero-image]: images/heroimage.png

<!-- Badges -->
[badge-dot-net]: https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-mysql]: https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=fff&style=for-the-badge
[badge-swagger]: https://img.shields.io/badge/Swagger-85EA2D?logo=swagger&logoColor=000&style=for-the-badge
