

using Xunit;

public class MySqlSampleTests
{
    [Fact]
    public async Task GetServerTimeAsync()
    {
        var client = new MySqlSample();
        await client.GetServerTimeAsync();
    }

}