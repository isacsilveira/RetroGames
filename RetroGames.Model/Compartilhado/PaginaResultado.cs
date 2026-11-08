namespace RetroGames.Model.Compartilhado;

/// <summary>Uma página de resultados e o total de registros encontrados (com o mesmo filtro).</summary>
public class PaginaResultado<T>
{
    public List<T> Itens { get; set; } = [];

    public int Total { get; set; }
}
