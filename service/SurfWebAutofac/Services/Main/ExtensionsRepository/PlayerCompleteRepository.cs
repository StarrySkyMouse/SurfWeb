using Common.Db.SqlSugar.Repository.Main;
using Common.Db.SqlSugar.Repository.Main.SugarClient;
using Model.Models.Main;
using SqlSugar;

namespace Services.Main.ExtensionsRepository;

public class PlayerCompleteRepository : MainRepository<PlayerCompleteModel>
{
    public PlayerCompleteRepository(IMainSqlSugarClient sqlSugarClient) : base(sqlSugarClient)
    {
    }

    public override ISugarQueryable<PlayerCompleteModel> Queryable()
    {
        return base.Queryable().Where(t => !t.Hide);
    }

    public ISugarQueryable<PlayerCompleteModel> QueryableNoHide()
    {
        return base.Queryable();
    }
}