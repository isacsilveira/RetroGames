using RetroGames.Model.Model;

namespace RetroGames.Repository;

public interface IGeneroRepository
{
    /// <summary>Todos os gêneros, ordenados por nome.</summary>
    Task<List<Genero>> ListarAsync();
}
