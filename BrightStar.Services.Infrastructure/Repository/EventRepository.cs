using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Domain.Entities;
using BrightStar.Services.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Infrastructure.Repository
{
    public class EventRepository : Repository<Event>, IEventRepository
    {

        private readonly AppDbContext _db;
        public EventRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Event entity)
        {
            //_db.Update(entity);
            var entry = _db.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                dbSet.Attach(entity);
                entry.State = EntityState.Modified;
            }
        }
    }
}
