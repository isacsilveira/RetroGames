using RetroGames.Model.Model;

namespace RetroGames.Repository;

public interface IPlataformaRepository
{
    Task<List<Plataforma>> ListarAsync();

    Task<Plataforma?> BuscarPorIdAsync(int id);

    /// <summary>Informe idIgnorar na alteração, para a plataforma não acusar a si mesma.</summary>
    Task<bool> ExisteComNomeAsync(string nome, int? idIgnorar = null);

    Task<bool> PossuiJogosAsync(int id);

    /// <summary>Devolve o id gerado pelo banco.</summary>
    Task<int> InserirAsync(Plataforma plataforma);

    Task AlterarAsync(Plataforma plataforma);

    Task ExcluirAsync(int id);
}
