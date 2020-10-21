using Microsoft.EntityFrameworkCore;
using MyBulkMealsApp.Data;
using MyBulkMealsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MyBulkApps.Data
{
    public abstract class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        protected readonly TContext context;

        public virtual IQueryable<TEntity> Collection
        {
            get
            {
                return context.Set<TEntity>()
                .OfType<TEntity>();
            }
        }


        public BaseRepository(TContext context)
        {
            this.context = context;
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<TEntity>> Add(List<TEntity> entities)
        {
            context.Set<TEntity>().AddRange(entities);

            await context.SaveChangesAsync();
            return entities;
        }

        public async Task<TEntity> Verify(int id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            entity.IsVerified = true;
            entity.VerificationSubmissionTime = DateTime.Now;

            await context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<TEntity> Get(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<List<TEntity>> GetByKeyword(string keyword)
        {
            return await Collection.Where(i => i.ItemName.Contains(keyword)).ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetCreatedBy(string userId)
        {
            return await Collection.Where(i => i.CreatorId == userId).ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetByKeyword(string keyword, int quantity)
        {
            return await Collection.Where(i => i.ItemName.Contains(keyword)).Take(quantity).ToListAsync();
        }


        public virtual async Task<List<TEntity>> GetByCreationTime(bool descending)
        {
            if (descending)
                return await Collection.OrderByDescending(e => e.CreatedTime).ToListAsync();
            else
                return await Collection.OrderBy(e => e.CreatedTime).ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await Collection.ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetRandom(int quantity)
        {
            //Throwaway GUID used for ordering
            return await Collection.OrderBy(e => Guid.NewGuid()).Take(quantity).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TUserItem"></typeparam>
        /// <returns></returns>
        public virtual async Task<List<UserItem>> GetAllUserItems<TUserItem>()
        {
            return await context.Set<UserItem>().Where(i => i is TUserItem).ToListAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> Count()
        {
            return await Collection.CountAsync();
        }

    }
}