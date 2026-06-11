# Diagrama do Sistema


<img width="1152" height="648" alt="Untitled" src="https://github.com/user-attachments/assets/2090549c-e55e-486f-a471-35095e34bf7b" />


# 1. Visão Geral do Sistema
O sistema foi estruturado para fragmentar responsabilidades complexas em módulos especializados, eliminando o acoplamento excessivo comum em sistemas monolíticos. A arquitetura divide-se em serviços verticais de negócio e uma camada de infraestrutura de entrada.

Cada API possui seu próprio ciclo de vida, persistência de dados isolada e lógica de negócio independente. O ecossistema é unificado logicamente pelo API Gateway, que abstrai a complexidade da rede interna, oferecendo aos clientes uma interface única, coerente e simplificada para o consumo dos serviços.

# 2. Componentes Principais e Endpoints
## A. API de Catálogo (Farmacia.Catalogo.API)
Responsável pelo gerenciamento do portfólio global de produtos e suas respectivas classificações comerciais. Estabelece a base conceitual de quais itens a rede está autorizada a comercializar.

Classes Principais (Models):

Produto: Define o item comercializável (ID, Nome, Descrição, Preço e o vínculo com a categoria).

CategoriaProduto: Agrupa os produtos em divisões lógicas (ex.: Medicamentos, Cosméticos, Higiene).

Exemplos Práticos de Endpoints:

GET /api/produtos – Retorna a lista completa de produtos cadastrados.

POST /api/produtos – Registra um novo item no catálogo comercial.

## B. API de Estoque (Farmacia.Estoque.API)
Controla o volume físico, o armazenamento, as movimentações de inventário e a rastreabilidade direta das mercadorias através de informações críticas de fabricação.

Classes Principais (Models):

LoteEstoque: Gerencia a quantidade física de um produto específico atrelada a um lote (ID do Lote, Quantidade, Data de Fabricação, Data de Validade e ID do Produto).

Exemplos Práticos de Endpoints:

GET /api/estoque/{produtoId} – Consulta o saldo físico e os lotes disponíveis de um produto.

PUT /api/estoque/baixa – Deduz quantidades do inventário após a consolidação de uma venda.

## C. API de SNGPC (Farmacia.SNGPC.API)
Garante a conformidade legal e o fluxo de regulamentação de medicamentos controlados e antimicrobianos, operando conforme as exigências do Sistema Nacional de Gerenciamento de Produtos Controlados (SNGPC).

Classes Principais (Models):

ReceitaMedica: Modela o documento de prescrição retido no ato da venda (ID, Dados do Paciente, CRM do Médico, Data de Emissão e Medicamentos Prescritos).

Exemplos Práticos de Endpoints:

POST /api/sngpc/receitas – Realiza a validação e o lançamento de uma receita médica no sistema.

GET /api/sngpc/receitas/{id} – Recupera o histórico de uma receita escriturada para fins de auditoria.

# 3. Fluxo de Comunicação e Roteamento
A comunicação externa do sistema adota o padrão de inversão de dependência de rede por meio do API Gateway (Farmacia.ApiGateway), estabelecendo o seguinte fluxo operacional:

Emissão da Requisição: O cliente (seja uma aplicação Web, Mobile ou um PDV físico) realiza uma chamada direcionada ao endpoint público exposto pelo Gateway.

Interceptação e Proxy Dinâmico: O API Gateway avalia o padrão da URL de entrada e mapeia o destino correspondente utilizando regras de proxy reverso. As rotas internas são configuradas de forma declarativa:

Requisições para /catalogo-api/* são direcionadas para a API de Catálogo.

Requisições para /estoque-api/* são direcionadas para a API de Estoque.

Requisições para /sngpc-api/* são direcionadas para a API de SNGPC.

Resolução Interna: A API interna processa a requisição em ambiente isolado e devolve os dados estruturados ao Gateway, que repassa a resposta final ao cliente original de forma transparente.

# 4. Arquitetura de Dados e Tecnologias
O sistema utiliza abordagens tecnológicas heterogêneas (poliglota), escolhendo a melhor ferramenta de persistência para a natureza de cada dado:

Plataforma Base: Desenvolvido em .NET 10 (C#), utilizando as capacidades nativas do ASP.NET Core para APIs de alto desempenho.

Camada de Roteamento: O Gateway é construído sobre o ecossistema YARP (Yet Another Reverse Proxy), garantindo roteamento de alto nível, suporte nativo a balanceamento de carga e configuração maleável via arquivos appsettings.json.

Persistência Relacional (Catálogo e Estoque): Utiliza o Entity Framework Core com o provedor Pomelo MySQL. O banco de dados MySQL gerencia o Catálogo e o Estoque, assegurando consistência estrita (ACID), integridade referencial e segurança em transações de inventário.

Persistência Não-Relacional (SNGPC): Utiliza o MongoDB por meio do driver oficial de persistência orientado a documentos. A escolha do NoSQL para a API de SNGPC provê flexibilidade de esquema para armazenar estruturas variáveis de receitas e logs regulatórios volumosos sem penalizar o desempenho do restante do ecossistema.

# 5. Objetivo da Arquitetura
O desenho arquitetural do FARMACIA_GATEWAY_SYSTEM foi projetado para mitigar os problemas clássicos de manutenção de software, apoiando-se em três pilares primordiais:

Modularidade Absoluta: O desacoplamento garante que a lógica de negócio de um microsserviço não interfira nos demais. Uma atualização ou instabilidade na API de Catálogo não afeta a capacidade da API de SNGPC de validar receitas médicas em andamento.

Segurança por Ocultação: As APIs de negócio operam restritas à rede interna de servidores. O API Gateway funciona como uma barreira de segurança unificada, blindando o perímetro do sistema e centralizando políticas de autenticação e proteção contra ataques de negação de serviço (rate limiting).

Escalabilidade Direcionada: Permite otimizar os custos de infraestrutura. Caso as consultas de preços na API de Catálogo aumentem exponencialmente devido a picos sazonais de acesso, apenas este microsserviço precisa ser replicado ou receber mais recursos de hardware, mantendo as API de Estoque e API de SNGPC operando estavelmente em suas infraestruturas originais. políticas de autenticação e limitação de acessos (rate limiting).

Escalabilidade Independente: Se houver um aumento expressivo nas consultas de preços e produtos (Catálogo) devido a uma campanha promocional, apenas a API de Catálogo precisa receber mais recursos computacionais, otimizando o uso da infraestrutura sem impactar os demais serviços.
