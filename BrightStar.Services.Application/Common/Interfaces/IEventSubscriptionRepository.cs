using BrightStar.Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Application.Common.Interfaces
{
    public interface IEventSubscriptionRepository : IRepository<EventSubscription>
    {
        void Update(EventSubscription entity);
    }
}
