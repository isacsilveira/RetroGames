namespace RetroGames.Model.Model;

/// <summary>Tabela tb_jogo_genero (associativa N:N entre jogo e gênero).</summary>
public class JogoGenero
{
    // co_jogo
    public int IdJogo { get; set; }

    // co_genero
    public int IdGenero { get; set; }
}
