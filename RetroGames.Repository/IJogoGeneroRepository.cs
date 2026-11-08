using RetroGames.Model.Model;

namespace RetroGames.Repository;

public interface IJogoGeneroRepository
{
    Task<List<JogoGenero>> ListarTodosAsync();

    /// <summary>Vínculos de vários jogos, numa única consulta.</summary>
    Task<List<JogoGenero>> ListarPorJogosAsync(IEnumerable<int> idsJogos);

    /// <summary>Grava os vínculos de um jogo com os gêneros informados.</summary>
    Task InserirAsync(int idJogo, IEnumerable<int> idsGeneros);

    /// <summary>Apaga os vínculos de um jogo, para regravá-los na alteração.</summary>
    Task RemoverPorJogoAsync(int idJogo);
}
