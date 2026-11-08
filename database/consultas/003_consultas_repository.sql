-- RetroGames - Aula 2: o SQL de cada método do Repository, para testar no Workbench
-- ANTES de levá-lo para o código. Os @parâmetros viram variáveis aqui.
-- Cada consulta lê uma única tabela (sem JOIN e sem subconsulta).

USE retrogames;

-- ===== PlataformaRepository =====
-- Listar
SELECT co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento
FROM tb_plataforma ORDER BY no_plataforma;

-- Buscar por id
SET @id = 1;
SELECT co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento
FROM tb_plataforma WHERE co_seq_plataforma = @id;

-- Existe com nome (sem id a ignorar, e depois ignorando a própria plataforma)
SET @nome = 'Super Nintendo'; SET @idIgnorar = NULL;
SELECT COUNT(*) FROM tb_plataforma
WHERE no_plataforma = @nome AND (@idIgnorar IS NULL OR co_seq_plataforma <> @idIgnorar);   -- 1
SET @idIgnorar = 1;
SELECT COUNT(*) FROM tb_plataforma
WHERE no_plataforma = @nome AND (@idIgnorar IS NULL OR co_seq_plataforma <> @idIgnorar);   -- 0

-- Possui jogos (1 tem jogos; 7 não tem)
SET @id = 1;
SELECT COUNT(*) FROM tb_jogo WHERE co_plataforma = @id;

-- Inserir / Alterar / Excluir (teste com uma plataforma própria e apague no fim)
INSERT INTO tb_plataforma (no_plataforma, no_fabricante, nu_ano_lancamento) VALUES ('Plataforma Teste', 'Fabricante Teste', 2000);
SET @idNovo = LAST_INSERT_ID();
UPDATE tb_plataforma SET no_plataforma = 'Plataforma Teste 2', no_fabricante = 'Fabricante Teste', nu_ano_lancamento = 2001 WHERE co_seq_plataforma = @idNovo;
DELETE FROM tb_plataforma WHERE co_seq_plataforma = @idNovo;

-- ===== JogoRepository =====
-- Listagem: sem filtro (somente ativos), página 1 com 5 itens
SELECT COUNT(*) FROM tb_jogo WHERE st_ativo = TRUE;
SELECT co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo
FROM tb_jogo WHERE st_ativo = TRUE
ORDER BY no_jogo, co_seq_jogo LIMIT 5 OFFSET 0;

-- Segunda página
SELECT co_seq_jogo, no_jogo FROM tb_jogo WHERE st_ativo = TRUE
ORDER BY no_jogo, co_seq_jogo LIMIT 5 OFFSET 5;

-- Filtro por nome
SELECT COUNT(*) FROM tb_jogo WHERE st_ativo = TRUE AND no_jogo LIKE '%Mario%';
SELECT co_seq_jogo, no_jogo FROM tb_jogo WHERE st_ativo = TRUE AND no_jogo LIKE '%Mario%'
ORDER BY no_jogo, co_seq_jogo LIMIT 5 OFFSET 0;

-- Filtro por plataforma
SELECT COUNT(*) FROM tb_jogo WHERE st_ativo = TRUE AND co_plataforma = 3;

-- Filtro por lista de ids (o Service resolve o gênero e passa os ids)
SELECT co_seq_jogo, no_jogo FROM tb_jogo WHERE st_ativo = TRUE AND co_seq_jogo IN (1, 2, 3)
ORDER BY no_jogo, co_seq_jogo LIMIT 5 OFFSET 0;

-- Incluindo inativos (traz o Pac-Man, id 20)
SELECT COUNT(*) FROM tb_jogo;

-- Buscar por id
SELECT co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo
FROM tb_jogo WHERE co_seq_jogo = 1;

-- Inserir / Alterar / Desativar (com jogo de teste; remova no fim)
INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES ('Jogo Teste', NULL, 1995, 10.00, 1, TRUE, UTC_TIMESTAMP(), 1, 2);
SET @idJogo = LAST_INSERT_ID();
UPDATE tb_jogo SET no_jogo = 'Jogo Teste 2', ds_jogo = 'Descrição', nu_ano_lancamento = 1996, vl_preco = 12.50, qt_estoque = 2, co_plataforma = 2, co_condicao_jogo = 3 WHERE co_seq_jogo = @idJogo;
UPDATE tb_jogo SET st_ativo = FALSE WHERE co_seq_jogo = @idJogo;

-- ===== JogoGeneroRepository =====
SELECT co_jogo, co_genero FROM tb_jogo_genero ORDER BY co_jogo, co_genero;
SELECT co_jogo, co_genero FROM tb_jogo_genero WHERE co_jogo IN (1, 2, 3) ORDER BY co_jogo, co_genero;
INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES (@idJogo, 1), (@idJogo, 2);
DELETE FROM tb_jogo_genero WHERE co_jogo = @idJogo;

-- Limpeza do jogo de teste (o vínculo já foi removido acima)
DELETE FROM tb_jogo WHERE co_seq_jogo = @idJogo;

-- ===== GeneroRepository e CondicaoJogoRepository =====
SELECT co_seq_genero, no_genero FROM tb_genero ORDER BY no_genero;
SELECT co_condicao_jogo, ds_condicao_jogo FROM tb_condicao_jogo ORDER BY co_condicao_jogo;
