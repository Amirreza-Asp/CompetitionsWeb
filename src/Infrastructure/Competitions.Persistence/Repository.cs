using Competitions.Application;
using Competitions.Persistence.Data;
using Competitions.SharedKernel.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Competitions.Persistence
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public Repository ( ApplicationDbContext context )
        {
            _context = context;
        }


        #region Query
        public T Find ( object id )
        {
            return FindAsync(id).GetAwaiter().GetResult();
        }
        public async Task<T> FindAsync ( object id )
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T FirstOfDefault ( ISpecification<T> specification )
        {
            return ApplySpecification(specification).FirstOrDefault();
        }
        public T FirstOrDefault ( Expression<Func<T , bool>> filter = null , Func<IQueryable<T> , IIncludableQueryable<T , object>> include = null , bool isTracking = true )
        {
            return FirstOrDefaultAsync(filter , include , isTracking).GetAwaiter().GetResult();
        }
        public U FirstOrDefaultSelect<U> ( Expression<Func<T , U>> select , Expression<Func<T , bool>> filter = null , Func<IQueryable<T> , IIncludableQueryable<T , object>> include = null , bool isTracking = true )
        {
            return FirstOrDefaultSelectAsync(select , filter , include , isTracking).GetAwaiter().GetResult();
        }
        public async Task<U> FirstOrDefaultSelectAsync<U> ( Expression<Func<T , U>> select , Expression<Func<T , bool>> filter = null , Func<IQueryable<T> , IIncludableQueryable<T , object>> include = null , bool isTracking = false )
        {
            IQueryable<T> query = _context.Set<T>();

            if ( filter != null )
                query = query.Where(filter);

            if ( include != null )
                query = include(query);

            if ( !isTracking )
                query = query.AsNoTracking();

            return await query.Select(select).FirstOrDefaultAsync();
        }
        public async Task<T> FirstOrDefaultAsync ( Expression<Func<T , bool>> filter = null , Func<IQueryable<T> , IIncludableQueryable<T , object>> include = null , bool isTracking = false )
        {
            IQueryable<T> query = _context.Set<T>();

            if ( filter != null )
                query = query.Where(filter);

            if ( include != null )
                query = include(query);

            if ( !isTracking )
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public IEnumerable<T> GetAll ( ISpecification<T> specification )
        {
            return ApplySpecification(specification);
        }
        public IEnumerable<U> GetAll<U> ( ISpecification<T> specification , Expression<Func<T , U>> select )
        {
            var entities = ApplySpecification(specification);
            return entities.Select(select);
        }

        public IEnumerable<U> GetAll<U> (
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IIncludableQueryable<T , object>> include = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            Expression<Func<T , U>> select = null ,
            bool isTracking = false ,
            int take = 0 ,
            int skip = 0 )
        {
            return GetAllAsync(filter , include , orderBy , select , isTracking , take , skip).GetAwaiter().GetResult();
        }
        public async Task<IEnumerable<U>> GetAllAsync<U> (
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IIncludableQueryable<T , object>> include = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            Expression<Func<T , U>> select = null ,
            bool isTracking = false ,
            int take = 0 ,
            int skip = 0 )
        {
            IQueryable<T> query = _context.Set<T>();

            if ( filter != null )
                query = query.Where(filter);

            if ( include != null )
                query = include(query);

            if ( orderBy != null )
                query = orderBy(query);

            if ( !isTracking )
                query = query.AsNoTracking();

            if ( skip > 0 )
                query = query.Skip(skip);

            if ( take > 0 )
                query = query.Take(take);

            if ( select != null )
                return await query.Select(select).ToListAsync();

            var entities = await query.ToListAsync();
            return ( IEnumerable<U> ) entities;
        }
        public IEnumerable<T> GetAll (
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IIncludableQueryable<T , object>> include = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            bool isTracking = false ,
            int take = 0 ,
            int skip = 0 )
        {
            return GetAllAsync(filter , include , orderBy , isTracking , take , skip).GetAwaiter().GetResult();
        }
        public async Task<IEnumerable<T>> GetAllAsync (
            Expression<Func<T , bool>> filter = null ,
            Func<IQueryable<T> , IIncludableQueryable<T , object>> include = null ,
            Func<IQueryable<T> , IOrderedQueryable<T>> orderBy = null ,
            bool isTracking = false ,
            int take = 0 ,
            int skip = 0 )
        {
            IQueryable<T> query = _context.Set<T>();

            if ( filter != null )
                query = query.Where(filter);

            if ( include != null )
                query = include(query);

            if ( orderBy != null )
                query = orderBy(query);

            if ( !isTracking )
                query = query.AsNoTracking();

            if ( skip > 0 )
                query = query.Skip(skip);

            if ( take > 0 )
                query = query.Take(take);

            return await query.ToListAsync();
        }

        public int GetCount ( Expression<Func<T , bool>> filter = null )
        {
            if ( filter != null )
            {
                return _context.Set<T>().Where(filter).Count();
            }
            return _context.Set<T>().Count();
        }
        public int GetCount ( ISpecification<T> specification ) => ApplySpecification(specification).Count();
        #endregion

        #region Command
        public void Add ( T entity )
        {
            _context.Add(entity);
        }

        public void Remove ( T entity )
        {
            _context.Remove(entity);
        }

        public virtual void Update ( T entity )
        {
            _context.Update(entity);
        }
        #endregion

        #region Utilities
        private IQueryable<T> ApplySpecification ( ISpecification<T> spec )
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable() , spec);
        }
        #endregion

        public void Save ()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync ()
        {
            await _context.SaveChangesAsync();
        }

    }
}
