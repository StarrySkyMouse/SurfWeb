using IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Db.EFCore.Repository;
using Model.Models.Main;
using Bogus.DataSets;

namespace EFServices
{
    public class TestServices : ITestServices
    {
        private readonly IBaseRepository<MapModel> _mapRepository;
        private readonly IBaseRepository<PlayerCompleteModel> _playerCompleteRepository;

        public TestServices(IBaseRepository<MapModel> mapRepository,
            IBaseRepository<PlayerCompleteModel> playerCompleteRepository)
        {
            _mapRepository = mapRepository;
            _playerCompleteRepository = playerCompleteRepository;
        }

        public Task Test()
        {
            _mapRepository.Insert(new MapModel()
            {
                Name = "测试",
                Difficulty = "T1",
                Img = "123",
                SurcessNumber = 1,
                BonusNumber = 2,
                StageNumber = 3
            });
            _mapRepository.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
