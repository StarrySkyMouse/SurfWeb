using Microsoft.EntityFrameworkCore;

namespace Common.Db.EFCore.Seed;

public class EFCoreDbSeed : IEFCoreDbSeed
{
    private readonly Action<DbContextOptionsBuilder> _seedAction;

    public EFCoreDbSeed(Action<DbContextOptionsBuilder> seedAction)
    {
        _seedAction = seedAction;
    }

    public void Seed(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder == null) throw new ArgumentNullException(nameof(optionsBuilder));
        _seedAction(optionsBuilder);
    }
}

public class EFCoreDbSeedNull : IEFCoreDbSeed
{
    public void Seed(DbContextOptionsBuilder optionsBuilder)
    {
    }
}