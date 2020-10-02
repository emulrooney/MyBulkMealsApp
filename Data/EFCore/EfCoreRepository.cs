﻿using Microsoft.EntityFrameworkCore;
using MyBulkMealsApp.Data;
using MyBulkMealsApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkApps.Data.EFCore
{
    public abstract class EfCoreRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        protected readonly TContext context;
        public EfCoreRepository(TContext context)
        {
            this.context = context;
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
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

        public async Task<TEntity> Get(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TUserItem"></typeparam>
        /// <returns></returns>
        public virtual async Task<List<UserItem>> GetAllUserItems<TUserItem>()
        {
            if (typeof(Ingredient).Equals(typeof(TUserItem)))
                return await context.Set<UserItem>().Where(ui => ui.Ingredient != null).ToListAsync();
            else if (typeof(Recipe).Equals(typeof(TUserItem)))
                return await context.Set<UserItem>().Where(ui => ui.Recipe != null).ToListAsync();

            return null;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> Count()
        {
            return await context.Set<TEntity>().CountAsync();
        }

    }
}