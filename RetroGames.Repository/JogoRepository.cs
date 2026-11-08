using System.Data.Common;
using RetroGames.Model.Compartilhado;
using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class JogoRepository(RepositoryConfig config) : RepositoryBase(config), IJogoRepository
{
    private const string Colunas =
        "co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, " +
        "st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo";

    public async Task<PaginaResultado<Jogo>> ListarAsync(JogoFiltro filtro)
    {
        // A mesma cláusula WHERE serve para contar e para listar a página.
        var (clausula, parametros) = MontarFiltro(filtro);
        var resultado = new PaginaResultado<Jogo>();

        await using var conexao = await CriarConexaoAsync();

        await using (var comandoTotal = CriarComando(conexao, "SELECT COUNT(*) FROM tb_jogo" + clausula))
        {
            foreach (var (nome, valor) in parametros)
            {
                AdicionarParametro(comandoTotal, nome, valor);
            }

            resultado.Total = Convert.ToInt32(await comandoTotal.ExecuteScalarAsync());
        }

        await using (var comandoPagina = CriarComando(conexao,
            $"SELECT {Colunas} FROM tb_jogo{clausula} " +
            "ORDER BY no_jogo, co_seq_jogo LIMIT @limite OFFSET @deslocamento"))
        {
            foreach (var (nome, valor) in parametros)
            {
                AdicionarParametro(comandoPagina, nome, valor);
            }

            AdicionarParametro(comandoPagina, "@limite", filtro.TamanhoPagina);
            AdicionarParametro(comandoPagina, "@deslocamento", Math.Max(filtro.Pagina - 1, 0) * filtro.TamanhoPagina);

            await using var leitor = await comandoPagina.ExecuteReaderAsync();
            while (await leitor.ReadAsync())
            {
                resultado.Itens.Add(Mapear(leitor));
            }
        }

        return resultado;
    }

    public async Task<Jogo?> BuscarPorIdAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            $"SELECT {Colunas} FROM tb_jogo WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@id", id);

        await using var leitor = await comando.ExecuteReaderAsync();
        return await leitor.ReadAsync() ? Mapear(leitor) : null;
    }

    public async Task<int> InserirAsync(Jogo jogo)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, " +
            "st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo) " +
            "VALUES (@nome, @descricao, @anoLancamento, @preco, @estoque, " +
            "TRUE, UTC_TIMESTAMP(), @idPlataforma, @idCondicaoJogo)");
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@anoLancamento", jogo.AnoLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@idPlataforma", jogo.IdPlataforma);
        AdicionarParametro(comando, "@idCondicaoJogo", jogo.IdCondicaoJogo);

        return (int)await ExecutarGravacaoAsync(comando);
    }

    public async Task AlterarAsync(Jogo jogo)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "UPDATE tb_jogo SET no_jogo = @nome, ds_jogo = @descricao, nu_ano_lancamento = @anoLancamento, " +
            "vl_preco = @preco, qt_estoque = @estoque, co_plataforma = @idPlataforma, " +
            "co_condicao_jogo = @idCondicaoJogo " +
            "WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@anoLancamento", jogo.AnoLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@idPlataforma", jogo.IdPlataforma);
        AdicionarParametro(comando, "@idCondicaoJogo", jogo.IdCondicaoJogo);
        AdicionarParametro(comando, "@id", jogo.Id);

        await ExecutarGravacaoAsync(comando);
    }

    public async Task DesativarAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "UPDATE tb_jogo SET st_ativo = FALSE WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@id", id);

        await ExecutarGravacaoAsync(comando);
    }

    // Monta o WHERE só com os filtros informados. Os valores nunca entram no texto SQL:
    // vão como parâmetros (@busca, @idPlataforma, @idJogo0, @idJogo1...).
    private static (string Clausula, List<(string Nome, object? Valor)> Parametros) MontarFiltro(JogoFiltro filtro)
    {
        var condicoes = new List<string>();
        var parametros = new List<(string Nome, object? Valor)>();

        if (!filtro.IncluirInativos)
        {
            condicoes.Add("st_ativo = TRUE");
        }

        if (!string.IsNullOrWhiteSpace(filtro.Busca))
        {
            condicoes.Add("no_jogo LIKE @busca");
            parametros.Add(("@busca", $"%{EscaparLike(filtro.Busca.Trim())}%"));
        }

        if (filtro.IdPlataforma.HasValue)
        {
            condicoes.Add("co_plataforma = @idPlataforma");
            parametros.Add(("@idPlataforma", filtro.IdPlataforma.Value));
        }

        if (filtro.IdsJogos is not null)
        {
            if (filtro.IdsJogos.Count == 0)
            {
                condicoes.Add("1 = 0");
            }
            else
            {
                var nomes = new List<string>();
                var indice = 0;
                foreach (var idJogo in filtro.IdsJogos)
                {
                    var nome = $"@idJogo{indice++}";
                    nomes.Add(nome);
                    parametros.Add((nome, idJogo));
                }

                condicoes.Add($"co_seq_jogo IN ({string.Join(", ", nomes)})");
            }
        }

        var clausula = condicoes.Count == 0 ? string.Empty : " WHERE " + string.Join(" AND ", condicoes);
        return (clausula, parametros);
    }

    // Faz % e _ digitados pelo usuário valerem como texto comum no LIKE.
    private static string EscaparLike(string texto) =>
        texto.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_");

    private static Jogo Mapear(DbDataReader leitor)
    {
        var posicaoDescricao = leitor.GetOrdinal("ds_jogo");
        return new Jogo
        {
            Id = leitor.GetInt32(leitor.GetOrdinal("co_seq_jogo")),
            Nome = leitor.GetString(leitor.GetOrdinal("no_jogo")),
            Descricao = leitor.IsDBNull(posicaoDescricao) ? null : leitor.GetString(posicaoDescricao),
            AnoLancamento = leitor.GetInt32(leitor.GetOrdinal("nu_ano_lancamento")),
            Preco = leitor.GetDecimal(leitor.GetOrdinal("vl_preco")),
            Estoque = leitor.GetInt32(leitor.GetOrdinal("qt_estoque")),
            Ativo = leitor.GetBoolean(leitor.GetOrdinal("st_ativo")),
            DataCadastro = leitor.GetDateTime(leitor.GetOrdinal("dt_cadastro")),
            IdPlataforma = leitor.GetInt32(leitor.GetOrdinal("co_plataforma")),
            IdCondicaoJogo = leitor.GetInt32(leitor.GetOrdinal("co_condicao_jogo")),
        };
    }
}
