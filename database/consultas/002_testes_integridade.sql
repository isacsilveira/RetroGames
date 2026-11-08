-- RetroGames - Aula 1, etapa 5: testes de integridade.
-- Execute UM comando por vez (um erro interrompe o script inteiro no Workbench).
-- Depois de executar cada um, confira o resultado e ajuste o comentário se for diferente.
-- No fim, rode a limpeza: nenhum dado de teste pode ficar no banco.

USE retrogames;

-- Teste 1: jogo com co_plataforma inexistente
-- Resultado: Erro 1452 (chave estrangeira)
INSERT INTO tb_jogo (no_jogo, nu_ano_lancamento, vl_preco, qt_estoque, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES ('Jogo de Teste', 1995, 50.00, 1, UTC_TIMESTAMP(), 999, 1);

-- Teste 2: plataforma com nome já cadastrado
-- Resultado: Erro 1062 (UNIQUE)
INSERT INTO tb_plataforma (no_plataforma, no_fabricante, nu_ano_lancamento)
VALUES ('Super Nintendo', 'Nintendo', 1990);

-- Teste 3: excluir plataforma que possui jogos
-- Resultado: Erro 1451 (chave estrangeira)
DELETE FROM tb_plataforma WHERE co_seq_plataforma = 1;

-- Teste 4: jogo com no_jogo nulo
-- Resultado: Erro 1048 (NOT NULL)
INSERT INTO tb_jogo (no_jogo, nu_ano_lancamento, vl_preco, qt_estoque, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES (NULL, 1995, 50.00, 1, UTC_TIMESTAMP(), 1, 1);

-- Teste 5: jogo com vl_preco igual a 0
-- Resultado: aceito (a regra RN06 fica no Service, não no banco)
INSERT INTO tb_jogo (no_jogo, nu_ano_lancamento, vl_preco, qt_estoque, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES ('Jogo Preço Zero', 1995, 0, 1, UTC_TIMESTAMP(), 1, 1);

-- Limpeza: remove os dados inseridos pelos testes
DELETE FROM tb_jogo WHERE no_jogo = 'Jogo Preço Zero';
SELECT COUNT(*) AS jogos_restantes FROM tb_jogo; -- deve voltar a 20
