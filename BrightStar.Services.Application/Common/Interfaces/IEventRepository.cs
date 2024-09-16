using BrightStar.Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Application.Common.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        void Update(Event entity);
    }
}
