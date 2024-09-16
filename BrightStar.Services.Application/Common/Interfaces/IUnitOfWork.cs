using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IAppUserRepository AppUserRepository { get; }
        IEventRepository EventRepository { get; }
        IEventSubscriptionRepository EventSubscriptionRepository { get; }
        void SaveChanges();
    }
}
