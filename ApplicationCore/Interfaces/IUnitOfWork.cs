//using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
    public interface IUnitOfWork
    {
        //save changes to data source
        public IGenericRepository<Site> Site { get; }
        public IGenericRepository<SiteType> SiteType { get; }
        public IGenericRepository<Reservation> Reservation { get; }
        public IGenericRepository<User> User { get; }
        public IGenericRepository<Fee> Fee { get; }

        int Commit();
        Task<int> CommitAsync();

    }
}
