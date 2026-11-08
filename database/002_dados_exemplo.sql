-- RetroGames - Aula 1, etapa 4: dados de exemplo. Execute logo depois do 001_criar_tabelas.sql.
-- tb_usuario fica vazia: o administrador será criado pela API (aula de autenticação).

USE retrogames;
SET NAMES utf8mb4;

-- Tabelas de tipo, com chaves fixas
INSERT INTO tb_condicao_jogo (co_condicao_jogo, ds_condicao_jogo) VALUES
    (1, 'Novo'),
    (2, 'Usado'),
    (3, 'Recondicionado');

INSERT INTO tb_perfil (co_perfil, ds_perfil) VALUES
    (1, 'Administrador'),
    (2, 'Cliente');

-- Plataformas (a 7 fica sem jogos de propósito, para testar a contagem por plataforma)
INSERT INTO tb_plataforma (co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento) VALUES
    (1, 'Super Nintendo', 'Nintendo', 1990),
    (2, 'Mega Drive', 'Sega', 1988),
    (3, 'PlayStation', 'Sony', 1994),
    (4, 'Nintendo 64', 'Nintendo', 1996),
    (5, 'Game Boy', 'Nintendo', 1989),
    (6, 'Atari 2600', 'Atari', 1977),
    (7, 'Master System', 'Sega', 1985);

INSERT INTO tb_genero (co_seq_genero, no_genero) VALUES
    (1, 'Ação'),
    (2, 'Aventura'),
    (3, 'Plataforma'),
    (4, 'RPG'),
    (5, 'Corrida'),
    (6, 'Luta'),
    (7, 'Esporte'),
    (8, 'Puzzle');

-- Jogos: colunas (id, nome, descrição, ano, preço, estoque, ativo, plataforma, condição)
-- Jogo 7: estoque zero. Jogo 20: inativo. Jogo 18: sem descrição (NULL).
INSERT INTO tb_jogo (co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo) VALUES
    (1,  'Super Mario World', 'Mario e Yoshi atravessam a Dinosaur Land.', 1990, 129.90, 3, TRUE, UTC_TIMESTAMP(), 1, 2),
    (2,  'The Legend of Zelda: A Link to the Past', 'Link explora Hyrule e o Mundo das Trevas.', 1991, 189.90, 2, TRUE, UTC_TIMESTAMP(), 1, 2),
    (3,  'Donkey Kong Country', 'Plataforma com gráficos pré-renderizados.', 1994, 99.90, 4, TRUE, UTC_TIMESTAMP(), 1, 2),
    (4,  'Super Metroid', 'Samus Aran explora o planeta Zebes.', 1994, 249.90, 1, TRUE, UTC_TIMESTAMP(), 1, 3),
    (5,  'Sonic the Hedgehog 2', 'Sonic e Tails contra o Dr. Robotnik.', 1992, 89.90, 5, TRUE, UTC_TIMESTAMP(), 2, 2),
    (6,  'Streets of Rage 2', 'Beat em up clássico de quatro lutadores.', 1992, 139.90, 2, TRUE, UTC_TIMESTAMP(), 2, 2),
    (7,  'Golden Axe', 'Três heróis enfrentam o exército de Death Adder.', 1989, 79.90, 0, TRUE, UTC_TIMESTAMP(), 2, 2),
    (8,  'Phantasy Star IV', 'RPG de ficção científica em Algol.', 1993, 299.90, 1, TRUE, UTC_TIMESTAMP(), 2, 3),
    (9,  'Final Fantasy VII', 'Cloud e o grupo AVALANCHE contra a Shinra.', 1997, 159.90, 3, TRUE, UTC_TIMESTAMP(), 3, 2),
    (10, 'Crash Bandicoot', 'Plataforma 3D do marsupial mais famoso do PlayStation.', 1996, 119.90, 4, TRUE, UTC_TIMESTAMP(), 3, 2),
    (11, 'Gran Turismo 2', 'Simulador de corrida com mais de 600 carros.', 1999, 69.90, 6, TRUE, UTC_TIMESTAMP(), 3, 2),
    (12, 'Tekken 3', 'Luta 3D com elenco renovado.', 1998, 89.90, 3, TRUE, UTC_TIMESTAMP(), 3, 2),
    (13, 'Super Mario 64', 'Primeira aventura 3D do Mario.', 1996, 199.90, 2, TRUE, UTC_TIMESTAMP(), 4, 2),
    (14, 'GoldenEye 007', 'Tiro em primeira pessoa baseado no filme.', 1997, 169.90, 2, TRUE, UTC_TIMESTAMP(), 4, 2),
    (15, 'Mario Kart 64', 'Corrida com itens para até quatro jogadores.', 1996, 179.90, 3, TRUE, UTC_TIMESTAMP(), 4, 2),
    (16, 'Pokémon Red', 'Capture os 151 monstrinhos de Kanto.', 1996, 219.90, 2, TRUE, UTC_TIMESTAMP(), 5, 2),
    (17, 'Tetris', 'O quebra-cabeça de blocos mais famoso do mundo.', 1989, 59.90, 8, TRUE, UTC_TIMESTAMP(), 5, 2),
    (18, 'Super Mario Land', NULL, 1989, 69.90, 3, TRUE, UTC_TIMESTAMP(), 5, 3),
    (19, 'Pitfall!', 'Explore a selva atrás de tesouros.', 1982, 49.90, 2, TRUE, UTC_TIMESTAMP(), 6, 2),
    (20, 'Pac-Man', 'Fuja dos fantasmas e coma todos os pontos.', 1982, 54.90, 1, FALSE, UTC_TIMESTAMP(), 6, 2);

-- Vínculos jogo-gênero (1 a 3 gêneros por jogo)
INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES
    (1, 3), (1, 2),
    (2, 2), (2, 1),
    (3, 3),
    (4, 1), (4, 2),
    (5, 3), (5, 1),
    (6, 1), (6, 6),
    (7, 1), (7, 6),
    (8, 4), (8, 2),
    (9, 4), (9, 2),
    (10, 3), (10, 1),
    (11, 5), (11, 7),
    (12, 6),
    (13, 3), (13, 2),
    (14, 1),
    (15, 5),
    (16, 4), (16, 2),
    (17, 8),
    (18, 3),
    (19, 3), (19, 2),
    (20, 1), (20, 8);
