using RetroGames.Model.Compartilhado;
using RetroGames.Model.Model;

namespace RetroGames.Repository;

public interface IJogoRepository
{
    /// <summary>Jogos de uma página e o total encontrado; filtro e paginação no SQL.</summary>
    Task<PaginaResultado<Jogo>> ListarAsync(JogoFiltro filtro);

    Task<Jogo?> BuscarPorIdAsync(int id);

    /// <summary>Devolve o id gerado pelo banco.</summary>
    Task<int> InserirAsync(Jogo jogo);

    Task AlterarAsync(Jogo jogo);

    /// <summary>Exclusão lógica: st_ativo = 0. Nenhum método apaga jogo.</summary>
    Task DesativarAsync(int id);
}
