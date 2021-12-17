using ChatApp.Client.Data;
using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data;

namespace ChatApp.Server.Database;

internal static class DatabaseManager
{
    private static QueryFactory s_factory = null!;

    internal static QueryFactory Factory
    {
        get
        {
            if ((s_factory.Connection.State == ConnectionState.Broken) |
                (s_factory.Connection.State == ConnectionState.Closed))
                Setup();

            return s_factory;
        }
    }

    internal static Net.Handlers.AccountRegisterHandler AccountRegisterHandler
    {
        get => default;
        set
        {
        }
    }

    public static Net.Handlers.LoginHandler LoginHandler
    {
        get => default;
        set
        {
        }
    }

    public static Net.Handlers.ChatHandler ChatHandler
    {
        get => default;
        set
        {
        }
    }

    internal static void Setup()
    {
        var connection = new MySqlConnection($"Server={Setting.Value.Host};Port={Setting.Value.Port};Database=chat;User={Setting.Value.Username};Password={Setting.Value.Password}");
        var compiler = new MySqlCompiler();
        s_factory?.Connection.Close();
        s_factory = new QueryFactory(connection, compiler);
    }
}