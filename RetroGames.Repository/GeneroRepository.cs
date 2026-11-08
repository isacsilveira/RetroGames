using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class GeneroRepository(RepositoryConfig config) : RepositoryBase(config), IGeneroRepository
{
    public async Task<List<Genero>> ListarAsync()
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "SELECT co_seq_genero, no_genero FROM tb_genero ORDER BY no_genero");

        var generos = new List<Genero>();
        await using var leitor = await comando.ExecuteReaderAsync();
        while (await leitor.ReadAsync())
        {
            generos.Add(new Genero
            {
                Id = leitor.GetInt32(leitor.GetOrdinal("co_seq_genero")),
                Nome = leitor.GetString(leitor.GetOrdinal("no_genero")),
            });
        }

        return generos;
    }
}
