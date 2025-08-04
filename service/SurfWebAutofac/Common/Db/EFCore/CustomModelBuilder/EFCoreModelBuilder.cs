using Microsoft.EntityFrameworkCore;

namespace Common.Db.EFCore.CustomModelBuilder;

public class EFCoreModelBuilder : IEFCoreModelBuilder
{
    private readonly Action<ModelBuilder> _modelBuilder;

    public EFCoreModelBuilder(Action<ModelBuilder> modelBuilder)
    {
        _modelBuilder = modelBuilder;
    }

    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));
        _modelBuilder(modelBuilder);
    }
}

public class EFCoreModelBuilderNull : IEFCoreModelBuilder
{
    public void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 空实现
    }
}