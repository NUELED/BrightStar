using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _db;
        public UnitOfWork(AppDbContext db)
        {
           _db = db;
           AppUserRepository = new AppUserRepository(_db); 
           EventRepository = new EventRepository(_db);
           EventSubscriptionRepository = new EventSubscriptionRepository(_db);
        }
        public IAppUserRepository AppUserRepository { get; private set; }

        public IEventRepository EventRepository { get; private set; }

        public IEventSubscriptionRepository EventSubscriptionRepository { get; private set; }

        public void SaveChanges()
        {
           _db.SaveChanges();
        }
    }
}
