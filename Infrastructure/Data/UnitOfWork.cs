using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IGenericRepository<Reservation> _Reservation;
        private IGenericRepository<Site> _Site;
        private IGenericRepository<SiteType> _SiteType;
        private IGenericRepository<User> _User;
        private IGenericRepository<Fee> _Fee;
        private IGenericRepository<Role> _Role;

        public IGenericRepository<Role> Role
        {
            get
            {
                if (_Role == null)
                {
                    _Role = new GenericRepository<Role>(_dbContext);
                }
                return _Role;
            }
        }
        public IGenericRepository<Fee> Fee
        {
            get
            {
                if (_Fee == null)
                {
                    _Fee = new GenericRepository<Fee>(_dbContext);
                }
                return _Fee;
            }
        }

        public IGenericRepository<User> User
        {
            get
            {
                if (_User == null)
                {
                    _User = new GenericRepository<User>(_dbContext);
                }
                return _User;
            }
        }
        public IGenericRepository<Site> Site
        {
            get
            {
                if (_Site == null)
                {
                    _Site = new GenericRepository<Site>(_dbContext);
                }
                return _Site;
            }
        }

        public IGenericRepository<SiteType> SiteType
        {
            get
            {
                if (_SiteType == null)
                {
                    _SiteType = new GenericRepository<SiteType>(_dbContext);
                }
                return _SiteType;
            }
        }

        public IGenericRepository<Reservation> Reservation
        {
            get
            {
                if (_Reservation == null)
                {
                    _Reservation = new GenericRepository<Reservation>(_dbContext);
                }
                return _Reservation;
            }
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}