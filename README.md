# 1. Padrão de Persistência Poliglota (Polyglot Persistence)
Em sistemas corporativos complexos, a imposição de um único modelo de base de dados para todos os domínios constitui um antipadrão. A arquitetura implementada adota o princípio de Persistência Poliglota, em que a tecnologia de armazenamento é selecionada de acordo com as exigências de leitura, escrita e estrutura de dados de cada contexto delimitado (Bounded Context).

# 1.1. Domínios Relacionais (Catálogo e Estoque)
Para os serviços de Catálogo e Estoque, optou-se pela utilização do Sistema de Gestão de Bases de Dados Relacionais (SGBDR) MySQL, orquestrado pelo mapeador objeto-relacional (ORM) Entity Framework Core.

Entidades como Produtos e Lotes de Estoque possuem fortes invariantes de domínio e dependências transacionais (conformidade ACID). O modelo relacional assegura a integridade referencial necessária para operações financeiras e de inventário.

Isolamento de Dados (Database per Service): Cada microsserviço possui o seu próprio contexto de dados (CatalogoDbContext e EstoqueDbContext). Este padrão evita o acoplamento pelo nível de base de dados, garantindo que uma sobrecarga nas consultas do catálogo não comprometa as operações de baixa no stock.

# 1.2. Domínio Orientado a Documentos (SNGPC)
Para o serviço que processa as retenções de receitas médicas (SNGPC), foi adotado o MongoDB, um banco de dados NoSQL orientado a documentos.

As receitas médicas e os registos de auditoria governamental assemelham-se a documentos imutáveis e densos em informação. A utilização do BSON permite a inserção assíncrona de alto débito (high-throughput) sem a sobrecarga do bloqueio de tabelas (table locking) típico dos SGBDRs. Adicionalmente, oferece flexibilidade de esquema caso a entidade reguladora (Anvisa) altere os requisitos dos dados no futuro.

# 2. Análise Detalhada dos Componentes e Padrões de Projeto
# 2.1. Microsserviço de Catálogo (Farmacia.Catalogo.API)
Este serviço atua como a fonte de verdade para os dados mestres dos medicamentos e produtos.

Resiliência e Padrão Retry: Na configuração da injeção de dependência do DbContext, foi implementada a política EnableRetryOnFailure (com um máximo de 5 tentativas e atraso de 30 segundos). Em arquiteturas distribuídas baseadas na nuvem, as falhas de rede são consideradas eventos expectáveis (falhas transitórias). Este padrão previne que flutuações momentâneas na latência da rede com a base de dados resultem na indisponibilidade do serviço.

Validação por Data Annotations: A classe Produto emprega atributos de validação como [Required], [StringLength(150)] e [Range]. Esta abordagem consubstancia o padrão de Validação de Entrada (Input Validation) no nível do modelo de domínio, atuando como uma Camada Anticorrupção (Anti-Corruption Layer) preliminar. Garante-se que o estado de um objeto Produto é sempre consistente antes de qualquer operação de I/O ou processamento de negócio.

# 2.2. Microsserviço SNGPC (Farmacia.SNGPC.API)
Este serviço gere o domínio submetido a estritas regulamentações governamentais.

Mapeamento de Dados e Padrão Options: A serialização BSON é garantida através de atributos nativos, mapeando a classe ReceitaMedica diretamente para a semântica do MongoDB (e.g., [BsonId], [BsonRepresentation(BsonType.ObjectId)]). Adicionalmente, a injeção da configuração da base de dados utiliza o Padrão Options (IOptions<MongoDbSettings>). Este padrão académico preconiza o encapsulamento de configurações num objeto tipado, promovendo o Princípio da Responsabilidade Única (SRP) e facilitando a cobertura por testes unitários ao injetar configurações mockadas.

Conceção RESTful: O controlador SngpcController reflete os princípios RESTful através do uso semântico de verbos HTTP (GET para listagens e POST para criação) e do retorno de códigos de estado padronizados (e.g., CreatedAtAction retornando HTTP 201 e a rota para o recurso recém-criado).

# 2.3. Padrão API Gateway (Farmacia.ApiGateway)
A presença de um projeto dedicado ao API Gateway atende a um dos desafios centrais dos sistemas distribuídos: a coesão da comunicação com os clientes externos (Aplicações Mobile, Web ou B2B).

Ponto Único de Entrada (Single Entry Point): A aplicação cliente não deve necessitar de conhecer a topologia interna da rede nem endereços IP individuais do Catálogo, do Estoque ou do SNGPC. O Gateway atua como um intermediário de roteamento.

Embora de momento o projeto esteja inicializado de forma basilar no ecossistema .NET, do ponto de vista arquitetural, o Gateway será futuramente o local apropriado para implementar padrões transversais de Sistemas Corporativos, tais como: terminação SSL, autenticação centralizada (e.g., validação de tokens JWT), agregação de chamadas e limitação de taxa (Rate Limiting).

# 2.4. Documentação de Contratos (Swagger/OpenAPI)
Todos os microsserviços implementam a especificação OpenAPI através das bibliotecas Swashbuckle (builder.Services.AddSwaggerGen()).

Num ambiente de Sistemas Corporativos sustentado por equipas multidisciplinares, o contrato da API é o artefacto mais vital da integração de sistemas. O OpenAPI fornece uma documentação viva, testável e legível por máquinas, reduzindo a fricção (integration tax) entre as equipas de Front-end e Back-end.
