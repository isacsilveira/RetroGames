namespace RetroGames.Model.Compartilhado;

/// <summary>Filtros e paginação da listagem de jogos. Cada filtro só vale quando informado.</summary>
public class JogoFiltro
{
    /// <summary>Parte do nome do jogo.</summary>
    public string? Busca { get; set; }

    public int? IdPlataforma { get; set; }

    /// <summary>
    /// Ids de jogos permitidos (o Service preenche com o resultado do filtro por gênero).
    /// Nulo = sem filtro; lista vazia = nenhum jogo.
    /// </summary>
    public IReadOnlyCollection<int>? IdsJogos { get; set; }

    /// <summary>Falso (padrão) = só jogos ativos.</summary>
    public bool IncluirInativos { get; set; }

    /// <summary>Primeira página = 1.</summary>
    public int Pagina { get; set; } = 1;

    public int TamanhoPagina { get; set; } = 12;
}
