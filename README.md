<img width="1024" height="559" alt="image" src="https://github.com/user-attachments/assets/0dcf48fa-a746-42db-9e49-16f81670dbf0" />



# 1. Visão Geral do Sistema
O FARMACIA_GATEWAY_SYSTEM é uma plataforma corporativa modular desenvolvida para gerenciar as operações fundamentais de uma rede de farmácias. O sistema adota uma arquitetura baseada em microsserviços (APIs independentes), onde cada módulo possui responsabilidades bem delimitadas e isolamento de dados.

O ecossistema é centralizado por um API Gateway, que atua como a única porta de entrada para os clientes (aplicações web, mobile ou integrações externas), garantindo o roteamento inteligente, a segurança e a abstração da infraestrutura interna.

# 2. Componentes Principais
O sistema é dividido em quatro projetos principais, descritos a seguir:

## A. API de Catálogo (Farmacia.Catalogo.API)
Propósito: Responsável pelo gerenciamento do portfólio de produtos oferecidos pela farmácia e suas respectivas classificações comerciais.

Classes Principais (Models):

Produto: Representa o item comercializável. Contém campos como identificador único, nome, descrição, preço e o vínculo com uma categoria.

CategoriaProduto: Define as classificações dos produtos (ex.: Medicamentos, Cosméticos, Suplementos), contendo campos de identificação e nome da categoria.

Controlador (ProdutoController): Expõe os endpoints HTTP para criação, leitura, atualização e exclusão (CRUD) de produtos e categorias.

## B. API de Estoque (Farmacia.Estoque.API)
Propósito: Controla o volume físico, o armazenamento e a rastreabilidade dos produtos através de lotes e validades.

Classes Principais (Models):

LoteEstoque: Gerencia os lotes de mercadorias armazenados. Possui campos essenciais como identificador do lote, quantidade em estoque, data de fabricação, data de validade e o identificador do produto correspondente (vínculo lógico com o Catálogo).

Controlador (EstoqueController): Fornece métodos para consulta de níveis de estoque, entrada de novas mercadorias e baixa de itens.

C. API do SNGPC (Farmacia.SNGPC.API)
Propósito: Gerencia o fluxo de medicamentos controlados e antimicrobianos, atendendo às exigências regulatórias do Sistema Nacional de Gerenciamento de Produtos Controlados (SNGPC).

Classes Principais (Models):

ReceitaMedica: Modela as prescrições médicas retidas na venda de controlados. Contém campos para identificação da receita, dados do paciente, dados do médico (CRM), data de emissão e a lista de medicamentos controlados prescritos.

MongoDbSettings: Configuração técnica interna para conexão com o banco de dados de persistência.

Controlador (SngpcController): Disponibiliza métodos para validação, registro e escrituração de receitas médicas.

# 3. Fluxo de Comunicação
O fluxo de dados no FARMACIA_GATEWAY_SYSTEM segue um padrão centralizado e transparente para o usuário:

Requisição do Cliente: O cliente externo envia uma requisição HTTP para o endereço único do sistema, gerenciado pelo API Gateway (Farmacia.ApiGateway).

Interpretação e Roteamento: O Gateway intercepta a requisição, analisa o prefixo da URL (ex.: /api/produtos, /api/estoque, /api/sngpc) e consulta suas regras internas de redirecionamento.

Encaminhamento Interno: O Gateway atua como um proxy reverso, repassando a requisição para a API correspondente, que opera de forma isolada na rede interna.

Retorno da Resposta: A API interna processa a solicitação, realiza as operações no banco de dados e devolve o resultado ao Gateway, que por sua vez formata e entrega a resposta final ao cliente.

# 4. Configurações e Tecnologias
O ecossistema utiliza a plataforma .NET 10 e adota tecnologias específicas para a necessidade de cada componente:

API Gateway (Farmacia.ApiGateway): Implementado utilizando a tecnologia YARP (Yet Another Reverse Proxy) da Microsoft. Suas rotas e destinos são definidos e estruturados dinamicamente através do arquivo de configuração appsettings.json.

Catálogo e Estoque: Desenvolvidos utilizando o Entity Framework Core associado ao provedor Pomelo MySQL, realizando a persistência de dados em um banco de dados relacional MySQL. Garante alta consistência para transações financeiras e de inventário.

SNGPC: Utiliza o banco de dados NoSQL MongoDB através do MongoDB Driver. A escolha por um banco orientado a documentos confere a flexibilidade necessária para armazenar históricos de receitas e dados regulatórios complexos.

# 5. Objetivo da Arquitetura
A estrutura arquitetural escolhida para o sistema visa assegurar três pilares fundamentais:

Modularidade (Desacoplamento): Como cada API possui seu próprio banco de dados e regras de negócio, uma eventual manutenção ou falha temporária na API de Catálogo não interrompe as operações de validação de receitas na API do SNGPC.

Segurança Centralizada: O API Gateway funciona como um escudo para a infraestrutura. As APIs internas não ficam expostas diretamente à internet, reduzindo significativamente a superfície de ataques e centralizando políticas de autenticação e limitação de acessos (rate limiting).

Escalabilidade Independente: Se houver um aumento expressivo nas consultas de preços e produtos (Catálogo) devido a uma campanha promocional, apenas a API de Catálogo precisa receber mais recursos computacionais, otimizando o uso da infraestrutura sem impactar os demais serviços.
