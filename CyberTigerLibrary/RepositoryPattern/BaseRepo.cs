using CyberTigerLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CyberTigerLibrary.RepositoryPattern
{
    public interface IBaseRepo<T> where T : BaseEntity
    {
        Task<T> GetById(int id);
        Task<T> GetByUid(Guid uid);
        Task<T> GetSingle(BaseSpec<T> spec);
        Task<IList<T>> GetList(BaseSpec<T> spec, int? take = null);
        Task<(int, IList<T>)> GetPageList(BaseSpec<T> spec, int pageIndex, int pageSize);

        Task<T> GetFirst(Expression<Func<T, bool>> filter);
        Task<int> Count(Expression<Func<T, bool>> filter);
        Task<bool> Exist(Expression<Func<T, bool>> filter);

        Task<T> Add(T entity);
        Task<bool> Update(T entity);
        Task<T> Save(T entity);
        Task<bool> Delete(int id);
        Task<bool> Delete(T entity);
        Task<int> Delete(Expression<Func<T, bool>> filter);
    }
    public class BaseRepo<T> : IBaseRepo<T> where T : BaseEntity
    {
        /* add services.AddScoped<DbContext, AppDbContext>(); in startup.cs*/

        protected readonly DbContext _db;
        protected readonly DbSet<T> _set;
        protected readonly IQueryable<T> _qry;

        public BaseRepo(DbContext db)
        {
            _db = db;
            _set = _db.Set<T>();
            _qry = _set.AsQueryable();
        }

        public async Task<T> GetById(int id)
        {
            return await _qry.FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<T> GetByUid(Guid uid)
        {
            return await _qry.FirstOrDefaultAsync(q => q.Uid == uid);
        }

        public async Task<T> GetSingle(BaseSpec<T> spec)
        {
            var qry = ApplySpecification(spec);

            if (qry != null && qry.Any())
                return await qry.SingleOrDefaultAsync();

            return null;
        }

        public async Task<(int, IList<T>)> GetPageList(BaseSpec<T> spec, int pageIndex, int pageSize)
        {
            var qry = ApplySpecification(spec);

            if (qry != null && qry.Any())
            {
                return (qry.Count(), await qry.Page(pageIndex, pageSize).ToListAsync());
            }

            return (0, null);
        }

        public async Task<IList<T>> GetList(BaseSpec<T> spec, int? take = null)
        {
            var qry = ApplySpecification(spec);

            if (qry != null && qry.Any())
            {
                if (take != null)
                    qry = qry.Take((int)take);

                return await qry.ToListAsync();

            }
            return null;
        }

        public async Task<T> GetFirst(Expression<Func<T, bool>> filter)
        {
            return await _qry.FirstOrDefaultAsync(filter);
        }

        public async Task<int> Count(Expression<Func<T, bool>> filter)
        {
            return await _qry.Where(filter).CountAsync();
        }

        public async Task<bool> Exist(Expression<Func<T, bool>> filter)
        {
            return (await Count(filter)) > 0;
        }

        public async Task<T> Add(T entity)
        {
            try
            {
                entity.Uid = Guid.NewGuid();
                entity.CreatedDate = DateTime.Now;
                entity.UpdatedDate = DateTime.Now;

                _set.Add(entity);
                await _db.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                return null;
            }
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                entity.UpdatedDate = DateTime.Now;

                _db.Entry(entity).State = EntityState.Modified;
                var result = await _db.SaveChangesAsync();

                return (result > 0);
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                return false;
            }
        }

        public async Task<T> Save(T entity)
        {
            if (entity.Id == 0)
            {
                return await Add(entity);
            }
            else
            {
                await Update(entity);

                return entity;
            }

        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                _set.Remove(await _set.FindAsync(id));
                var result = await _db.SaveChangesAsync();

                return (result > 0);
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                return false;
            }
        }

        public async Task<bool> Delete(T entity)
        {
            try
            {
                _set.Remove(entity);
                var result = await _db.SaveChangesAsync();

                return (result > 0);
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                return false;
            }
        }

        public async Task<int> Delete(Expression<Func<T, bool>> filter)
        {
            try
            {
                _set.RemoveRange(_qry.Where(filter));
                return await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                return 0;
            }
        }

        protected IQueryable<T> ApplySpecification(BaseSpec<T> spec)
        {
            return _qry.Include(spec.Includes).IncludeString(spec.StringIncludes).Sort(spec.Order).Filter(spec.Filters);
        }
    }
}
