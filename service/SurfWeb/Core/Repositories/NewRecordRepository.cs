using Core.IRepository;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class NewRecordRepository : BaseRepository<NewRecordModel>, INewRecordRepository
    {
        public NewRecordRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}