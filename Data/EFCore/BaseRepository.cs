﻿using Microsoft.EntityFrameworkCore;
using MyBulkMealsApp.Data;
using MyBulkMealsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkApps.Data.EFCore
{
    public abstract class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext
    {
        protected readonly TContext context;
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
            return await context.Set<TEntity>().Where(i => i.ItemName.Contains(keyword)).ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetByCreationTime(bool descending)
        {
            if (descending)
                return await context.Set<TEntity>().OrderByDescending(e => e.CreatedTime).ToListAsync();
            else
                return await context.Set<TEntity>().OrderBy(e => e.CreatedTime).ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetRandom(int quantity)
        {
            //Throwaway GUID used for ordering
            var list = context.Set<TEntity>().OrderBy(e => Guid.NewGuid());
            return await list.Take(quantity).ToListAsync();
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
            return await context.Set<TEntity>().CountAsync();
        }

    }
}