namespace RetroGames.Repository;

/// <summary>Reúne todos os repositórios. O Service recebe só este objeto.</summary>
public interface IRepositorySet
{
    IPlataformaRepository Plataforma { get; }

    IJogoRepository Jogo { get; }

    IJogoGeneroRepository JogoGenero { get; }

    IGeneroRepository Genero { get; }

    ICondicaoJogoRepository CondicaoJogo { get; }
}
