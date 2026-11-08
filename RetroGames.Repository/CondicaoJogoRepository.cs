using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class CondicaoJogoRepository(RepositoryConfig config) : RepositoryBase(config), ICondicaoJogoRepository
{
    public async Task<List<CondicaoJogo>> ListarAsync()
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "SELECT co_condicao_jogo, ds_condicao_jogo FROM tb_condicao_jogo ORDER BY co_condicao_jogo");

        var condicoes = new List<CondicaoJogo>();
        await using var leitor = await comando.ExecuteReaderAsync();
        while (await leitor.ReadAsync())
        {
            condicoes.Add(new CondicaoJogo
            {
                Id = leitor.GetInt32(leitor.GetOrdinal("co_condicao_jogo")),
                Descricao = leitor.GetString(leitor.GetOrdinal("ds_condicao_jogo")),
            });
        }

        return condicoes;
    }
}
