-- RetroGames - Aula 1, etapa 5: consultas de exploração (executar no Workbench).
-- Aqui o JOIN é permitido: são consultas para estudar o banco, não código do Repository.

USE retrogames;

-- 1. Catálogo: jogos ativos com plataforma e condição, ordenados por nome
SELECT j.co_seq_jogo, j.no_jogo, p.no_plataforma, c.ds_condicao_jogo, j.vl_preco, j.qt_estoque
FROM tb_jogo j
JOIN tb_plataforma p ON p.co_seq_plataforma = j.co_plataforma
JOIN tb_condicao_jogo c ON c.co_condicao_jogo = j.co_condicao_jogo
WHERE j.st_ativo = TRUE
ORDER BY j.no_jogo;

-- 2. Gêneros de um jogo informado
SET @id_jogo = 1;
SELECT g.co_seq_genero, g.no_genero
FROM tb_jogo_genero jg
JOIN tb_genero g ON g.co_seq_genero = jg.co_genero
WHERE jg.co_jogo = @id_jogo
ORDER BY g.no_genero;

-- 3. Jogos de um gênero informado
SET @id_genero = 3;
SELECT j.co_seq_jogo, j.no_jogo
FROM tb_jogo_genero jg
JOIN tb_jogo j ON j.co_seq_jogo = jg.co_jogo
WHERE jg.co_genero = @id_genero
ORDER BY j.no_jogo;

-- 4. Quantidade de jogos por plataforma, incluindo plataformas sem jogos (LEFT JOIN)
SELECT p.no_plataforma, COUNT(j.co_seq_jogo) AS qt_jogos
FROM tb_plataforma p
LEFT JOIN tb_jogo j ON j.co_plataforma = p.co_seq_plataforma
GROUP BY p.co_seq_plataforma, p.no_plataforma
ORDER BY p.no_plataforma;

-- 5. Busca por parte do nome, 5 itens por página (troque o OFFSET para ver a segunda página)
SELECT co_seq_jogo, no_jogo
FROM tb_jogo
WHERE no_jogo LIKE '%Mario%'
ORDER BY no_jogo
LIMIT 5 OFFSET 0;

-- Verificações da etapa 4
SELECT (SELECT COUNT(*) FROM tb_plataforma) AS plataformas,
       (SELECT COUNT(*) FROM tb_genero)     AS generos,
       (SELECT COUNT(*) FROM tb_jogo)       AS jogos,
       (SELECT COUNT(*) FROM tb_usuario)    AS usuarios;

-- Nenhum jogo sem gênero (deve devolver 0 linhas)
SELECT j.co_seq_jogo, j.no_jogo
FROM tb_jogo j
LEFT JOIN tb_jogo_genero jg ON jg.co_jogo = j.co_seq_jogo
WHERE jg.co_jogo IS NULL;
