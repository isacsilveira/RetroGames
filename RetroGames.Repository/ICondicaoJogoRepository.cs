using RetroGames.Model.Model;

namespace RetroGames.Repository;

public interface ICondicaoJogoRepository
{
    /// <summary>Todas as condições de jogo, ordenadas pela chave.</summary>
    Task<List<CondicaoJogo>> ListarAsync();
}
