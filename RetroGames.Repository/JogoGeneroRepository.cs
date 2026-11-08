using System.Data.Common;
using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class JogoGeneroRepository(RepositoryConfig config) : RepositoryBase(config), IJogoGeneroRepository
{
    public async Task<List<JogoGenero>> ListarTodosAsync()
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "SELECT co_jogo, co_genero FROM tb_jogo_genero ORDER BY co_jogo, co_genero");

        return await LerVinculosAsync(comando);
    }

    public async Task<List<JogoGenero>> ListarPorJogosAsync(IEnumerable<int> idsJogos)
    {
        var ids = idsJogos.Distinct().ToList();
        if (ids.Count == 0)
        {
            return [];
        }

        // Um parâmetro para cada id (@idJogo0, @idJogo1...): o IN continua parametrizado.
        var nomes = ids.Select((_, indice) => $"@idJogo{indice}").ToList();

        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            $"SELECT co_jogo, co_genero FROM tb_jogo_genero WHERE co_jogo IN ({string.Join(", ", nomes)}) " +
            "ORDER BY co_jogo, co_genero");
        for (var i = 0; i < ids.Count; i++)
        {
            AdicionarParametro(comando, nomes[i], ids[i]);
        }

        return await LerVinculosAsync(comando);
    }

    public async Task InserirAsync(int idJogo, IEnumerable<int> idsGeneros)
    {
        var ids = idsGeneros.Distinct().ToList();
        if (ids.Count == 0)
        {
            return;
        }

        // Um único INSERT com uma linha de VALUES para cada gênero.
        var linhas = ids.Select((_, indice) => $"(@idJogo, @idGenero{indice})");

        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            $"INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES {string.Join(", ", linhas)}");
        AdicionarParametro(comando, "@idJogo", idJogo);
        for (var i = 0; i < ids.Count; i++)
        {
            AdicionarParametro(comando, $"@idGenero{i}", ids[i]);
        }

        await ExecutarGravacaoAsync(comando);
    }

    public async Task RemoverPorJogoAsync(int idJogo)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "DELETE FROM tb_jogo_genero WHERE co_jogo = @idJogo");
        AdicionarParametro(comando, "@idJogo", idJogo);

        await ExecutarGravacaoAsync(comando);
    }

    private static async Task<List<JogoGenero>> LerVinculosAsync(DbCommand comando)
    {
        var vinculos = new List<JogoGenero>();
        await using var leitor = await comando.ExecuteReaderAsync();
        while (await leitor.ReadAsync())
        {
            vinculos.Add(new JogoGenero
            {
                IdJogo = leitor.GetInt32(leitor.GetOrdinal("co_jogo")),
                IdGenero = leitor.GetInt32(leitor.GetOrdinal("co_genero")),
            });
        }

        return vinculos;
    }
}
