using FAI.Core.Entities;
using FAI.Core.Repositories;
using FAI.Persistence.Repositories.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Persistence.Repositories.Base
{
    public class BaseRepository : IBaseRepository
    {

        private MovieDbContext movieDbContext;

        public BaseRepository()
        {
            movieDbContext = new MovieDbContext();
        }

        // Dependency Injection des DbContext über den Konstruktor
        public BaseRepository(MovieDbContext movieDbContext)
        {
            // Sicherstellen, dass der DbContext nicht null ist
            movieDbContext = movieDbContext ?? throw new ArgumentNullException(nameof(movieDbContext));
        }

        public void Save()
        {
            this.movieDbContext.SaveChanges();
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await this.movieDbContext.SaveChangesAsync(cancellationToken);
        }

        #region [C]REATE
        public void Add<T>(T entity, bool saveImmediately = false)
            where T : class, IEntity
        {
            if (entity == null)
            {
                return;
            }
            this.movieDbContext.Add(entity);

            if (saveImmediately)
            {
                this.Save();
            }
        }

        public async Task AddAsync<T>(T entity, bool saveImmediately = false, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            if (entity == null)
            {
                return;
            }

            this.movieDbContext.Add(entity);

            if (saveImmediately)
            {
                await this.SaveAsync(cancellationToken);
            }
        }
        #endregion

        #region [R]EAD
        // Expression<Func<T, bool>> ist ein Lambda-Ausdruck, der eine Bedingung für die Abfrage definiert
        public IQueryable<T> QueryFrom<T>(Expression<Func<T, bool>>? whereFilter = null)
            where T : class, IEntity
        {
            var query = this.movieDbContext.Set<T>().AsQueryable();
            if (whereFilter != null)
            {
                query = query.Where(whereFilter);
            }
            return query;
        }
        #endregion

        #region [U]PDATE
        public T Update<T>(T entity, object key, bool saveImmdiately = false)
            where T : class, IEntity
        {
            if (entity == null)
            {
                return null;
            }

            var toUpdate = this.movieDbContext.Set<T>().Find(key);
            if (toUpdate != null)
            {
                this.movieDbContext.Entry(toUpdate).CurrentValues.SetValues(entity);
                
                if (saveImmdiately)
                {
                    this.Save();
                }
            }
            return toUpdate;
        }

        public async Task<T> UpdateAsync<T>(T entity, object key, bool saveImmediately = false, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            if (entity == null)
            {
                return null;
            }

            var toUpdate = await this.movieDbContext.Set<T>().FindAsync(key, cancellationToken);
            if (toUpdate != null)
            {
                this.movieDbContext.Entry(toUpdate).CurrentValues.SetValues(entity);

                if (saveImmediately)
                {
                    await this.SaveAsync(cancellationToken);
                }
            }
            return toUpdate;
        }
        #endregion

        #region [D]ELETE
        public void Remove<T>(T entity, bool saveImmediately = false)
            where T : class, IEntity
        {
            if (entity == null)
            {
                return;
            }

            this.movieDbContext.Remove(entity);

            if (saveImmediately)
            {
                this.Save();
            }
        }

        public async Task RemoveAsync<T>(T entity, bool saveImmediately = false, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            if (entity == null)
            {
                return;
            }
            this.movieDbContext.Remove(entity);
            if (saveImmediately)
            {
                await this.SaveAsync(cancellationToken);
            }
        }
     
        public void RemoveByKey<T>(object key, bool saveImmediately = false)
            where T : class, IEntity
        {
            if (key == null)
            {
                return;
            }
            
            var toRemove = this.movieDbContext.Set<T>().Find(key);
            
            if (toRemove != null)
            {
                this.movieDbContext.Remove(toRemove);
                if (saveImmediately)
                {
                    this.Save();
                }
            }
        }

        public async Task RemoveByKeyAsync<T>(object key, bool saveImmediately = false, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            if (key == null)
            {
                return;
            }
            
            var toRemove = await this.movieDbContext.Set<T>().FindAsync(key, cancellationToken);
            
            if (toRemove != null)
            {
                this.movieDbContext.Remove(toRemove);
                if (saveImmediately)
                {
                    await this.SaveAsync(cancellationToken);
                }
            }
        }
        #endregion
    }
}
