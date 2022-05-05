using Azure.Core;
using Azure.Identity;
using MySql.Data.MySqlClient;

public class MySqlSample
{
    private static string Host = "HOST";
    private static string User = "USER";
    private static string Database = "DATABASE";
    public async Task GetServerTimeAsync()
    {
        Console.Out.WriteLine("Getting access token from Azure Instance Metadata service...");

        DefaultAzureCredential credential = new DefaultAzureCredential();

        // Azure AD resource ID for Azure Database for MySQL is https://ossrdbms-aad.database.windows.net/
        TokenRequestContext requestContext = new TokenRequestContext(new[] { "https://ossrdbms-aad.database.windows.net" });
        var accessToken = await credential.GetTokenAsync(requestContext);

        //
        // Open a connection to the MySQL server using the access token.
        //
        var builder = new MySqlConnectionStringBuilder
        {
            Server = Host,
            Database = Database,
            UserID = User,
            Password = accessToken.Token,
            SslMode = MySqlSslMode.Required,
        };

        using (var conn = new MySqlConnection(builder.ConnectionString))
        {
            Console.Out.WriteLine("Opening connection using access token...");
            await conn.OpenAsync();

            using (var command = conn.CreateCommand())
            {
                command.CommandText = "SELECT VERSION()";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine("\nConnected!\n\nMySQL version: {0}", reader.GetString(0));
                    }
                }
            }
        }
    }

}