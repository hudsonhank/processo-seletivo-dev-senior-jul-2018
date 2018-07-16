# **Processo Seletivo Desenvolvedor de Sistemas Sênior da Sigma/TJMT**

### API SGL - Sistema Gerenciador de Livrarias

------------------------

### O Desafio

​	Resolvi criar uma frame work de trabalhado usando padrões baseados no CQRS. A idéia era fazer com que os desenvolvedores possam escalar a aplicação de forma intuitiva assim com que os pedidos de 
evolução do sistema começem a aparece.

​	Por mais que o pedido original era pra criar um MVP. tive o cuidado de criar uma arquiteruta altamente flexivel as mudanças;

------------------------

### Como é dividido projeto?



A aplicação é dividida em 3 partes principais:

 * **Core.Abstractions**: Projeto com as classes base do projeto. A ideia é utilizar esse projeto como um Seed para qualquer novo projeto.
 * **SGL.API**:  Porta de entrada das informações. Sera responsavel pelo gerenciamento das requisições dos recursos do sistema.
 * **SGL.Core:** Onde aplicamos os pedidos que negocio que o dominio exige,
   esse projeto é resonsavel por fazer as prés validações antes de executar um comando, e tambem
   garantindo a emissão de eventos de resposta para as demais partes do sistema.
   Podemos dizer que nesse projeto se encontra os executores de todas os requisitos propostos para execuação do sistema.
 * **SGL.UnitTest**: Neste primeiro momento será escrito somente testes dos serviços do dominio. Afim de garantir que as regras solicitadas, estejam de acordo.


### Que recursos de terceiros foram utilizados


- **Dapper** - Facilitador onsultas / subistitui o EF em muitos casos .
- **Mediatr** - Gerencia a comunicação entre os projetos. Serve como um "disparado de eventos", e "controlador" dos mesmo.
- **Entity Framework** - ORM para trabalhar com o dominio.
- **Dapper** - Consultas com maior performance.
- **xUnit** - Lib para testes.
- **Fluent Assertions** - Lib com linguagem mais natural para validar as saidas dos testes.


### Como executar o sistema

1 - Instale o cliente docker
2 - clone o projeto em uma pasta de sua escolha
3 - acesse no diretorio pasta "Api" por conta das configurações do docker.
4 - abra um prompt de comando e execute os comandos docker:

`docker-compose build && docker-compose up`

5 - Aguarde as imagens terminarem se "levantar" e assim que o banco 'mssql-server' estiver apto a receber comandos, abra 
uma ferramenta de gestao de banco de dados e execute o script de criação que se encontra na pasta "data/scrip_incial.sql"
(Fiz isso porque acho muito ruim o model create do asp.net, acho muito mais simples criar por mim);

6 - Se tudo estive ok a aplicação pode ser vista no endereço: `http://localhost:50000/swagger`







