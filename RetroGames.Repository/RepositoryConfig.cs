namespace RetroGames.Repository;

/// <summary>
/// Guarda a connection string. Uma instância para toda a aplicação:
/// na aula 4 a API lê o .env e entrega o valor aqui.
/// </summary>
public class RepositoryConfig
{
    public string ConnectionString { get; set; } = string.Empty;
}
