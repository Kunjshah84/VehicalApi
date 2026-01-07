using Microsoft.EntityFrameworkCore;
using VehicalApi.Data;

public static class DbContextFactory
{
    public static MyDbContext Create(string dbName)
    {
        var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(dbName).Options;

        return new MyDbContext(options);
    }
}
