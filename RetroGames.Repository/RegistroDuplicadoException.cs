namespace RetroGames.Repository;

/// <summary>
/// Lançada quando o MySQL recusa um valor repetido numa coluna UNIQUE (erro 1062).
/// O Service converte esta exceção no erro de negócio correspondente.
/// </summary>
public class RegistroDuplicadoException : Exception
{
    public RegistroDuplicadoException(string mensagem, Exception? causa = null)
        : base(mensagem, causa)
    {
    }
}
