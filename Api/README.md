# Processo Seletivo Arquiteto de Software Back-End da Sigma/TJMT

### API SGPE - Sistema Gerenciador de patrimônios empresarial

------------------------

### O Desafio

​	Resolvi criar uma frame work de trabalhado usando padrões baseados no CQRS. A idéia era fazer com que os desenvolvedores possam escalar a aplicação de forma intuitiva assim com que os pedidos de 
evolução do sistema começem a aparece.

​	Por mais que o pedido original era pra criar um MVP. tive o cuidado de criar uma arquiteruta altamente flexivel as mudanças;

------------------------

### Como é dividido projeto?



A aplicação é dividida em 3 partes principais:

​	* Core.Ab





   Temos 5 projeto envolvidos (API, Application, Domain, Domain.Service e Infrastructure)


1 - API {Organizacao.SGPE.WebAPI} : porta de entrada das informações. Os verbos foram alterados 
por nome de eventos, logo não teremos métodos como (POST, PUT, GET), no lugar o nome da aplicação
da ideia do que será executado no pedido.

2 - Application {Organizacao.SGPE.Application}: resonsavel por obter os comandos da api e gerencia 
as transações quando é necessário. 

3 - {Organizacao.SGPE.Domain}: Onde se encontra as definições das entidades do sistema e controla 
alguns dos seus comportamentos. Junto com a proximo projeto é onde se encontra toda a regra de negocio
envolvida no sistema. Assim garantindo uma manutenção evolutiva mais simplificada. 

4 -{Organizacao.SGPE.Domain.Service}: Onde aplicamos os pedidos que negocio que o dominio exige,
esse projeto é resonsavel por fazer as pre validações antes de executar um comando, e tambem
garantindo o a emissão de eventos de resposta para as demais partes do sistema (EventSourcing, Eventos de integraçao, etc).
Podemos dizer que nesse projeto se encontra os executores de todas os pres e pos requisitos dos sistema.

5 - Organizacao.SGPE.Infrastructure: Onde fazemos as definições de mapeamnto da entidades, criamos os acessos
as informações para obter dados ou atualizar-lo. 

------------------------


### Que recursos de terceiros foram utilizados


- **FDSDK.Core** - Biblioteca de classes base para reaproveitamento de código usado no modelo acima explicado (Biblioteca de minha autoria).
- **Dapper** - Facilitador onsultas / subistitui o EF em muitos casos .
- **Mediatr** - Gerencia a comunicação entre os projetos. Serve como um "disparado de eventos", e "controlador" dos mesmo.


### Como executar o sistema

1 - Instale o cliente docker
2 - clone o projeto em uma pasta de sua escolha
3 - acesse no diretorio dessa pasta por conta das configurações do docker.
4 - abra um prompt de comando e execute os comandos docker: 'docker-compose build'  depois  'docker-compose up'
5 - Aguarde as imagens terminarem se "levantar" e assim que o banco 'mssql-server' estiver apto a receber comandos, abra 
uma ferramenta de gestao de banco de dados e execute o script de criação que se encontra na pasta "data/scrip_incial.sql"
(Fiz isso porque acho muito ruim o model create do asp.net, acho muito mais simples criar por mim);

6 - Se tudo estive ok a aplicação pode ser vista no endereço: `http://localhost:50000/swagger`













