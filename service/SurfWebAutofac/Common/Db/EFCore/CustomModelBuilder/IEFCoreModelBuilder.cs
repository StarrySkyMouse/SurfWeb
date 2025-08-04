using Microsoft.EntityFrameworkCore;

namespace Common.Db.EFCore.CustomModelBuilder;

public interface IEFCoreModelBuilder
{
    void OnModelCreating(ModelBuilder modelBuilder);
}