using System.Data.Common;
using MySqlConnector;

namespace RetroGames.Repository;

/// <summary>
/// Base de todos os repositórios: abre conexão, cria comando, adiciona parâmetro
/// e executa gravação. Não contém SQL de nenhuma tabela.
/// </summary>
public abstract class RepositoryBase(RepositoryConfig config)
{
    /// <summary>
    /// Abre uma conexão. Quem chama deve fechá-la com "await using".
    /// Se houver um TransactionScope ativo (aberto pelo Service), a conexão entra nele sozinha.
    /// </summary>
    protected async Task<DbConnection> CriarConexaoAsync()
    {
        var conexao = new MySqlConnection(config.ConnectionString);
        try
        {
            await conexao.OpenAsync();
        }
        catch
        {
            await conexao.DisposeAsync();
            throw;
        }

        return conexao;
    }

    /// <summary>Cria um comando com o texto SQL informado. Quem chama deve fechá-lo com "await using".</summary>
    protected static DbCommand CriarComando(DbConnection conexao, string sql)
    {
        var comando = conexao.CreateCommand();
        comando.CommandText = sql;
        return comando;
    }

    /// <summary>Adiciona um parâmetro (@nome e valor). Valor nulo vira NULL no banco.</summary>
    protected static void AdicionarParametro(DbCommand comando, string nome, object? valor)
    {
        var parametro = comando.CreateParameter();
        parametro.ParameterName = nome;
        parametro.Value = valor ?? DBNull.Value;
        comando.Parameters.Add(parametro);
    }

    /// <summary>
    /// Executa um INSERT, UPDATE ou DELETE e devolve o último id gerado (LastInsertedId).
    /// É o único lugar que converte o erro 1062 (UNIQUE) em RegistroDuplicadoException.
    /// </summary>
    protected static async Task<long> ExecutarGravacaoAsync(DbCommand comando)
    {
        try
        {
            await comando.ExecuteNonQueryAsync();
            return comando is MySqlCommand comandoMySql ? comandoMySql.LastInsertedId : 0;
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            throw new RegistroDuplicadoException("Já existe um registro com este valor.", ex);
        }
    }
}
