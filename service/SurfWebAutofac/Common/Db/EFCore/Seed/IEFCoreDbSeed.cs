using Microsoft.EntityFrameworkCore;

namespace Common.Db.EFCore.Seed;

public interface IEFCoreDbSeed
{
    void Seed(DbContextOptionsBuilder optionsBuilder);
}