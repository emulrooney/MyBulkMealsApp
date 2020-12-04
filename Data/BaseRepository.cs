using Microsoft.AspNetCore.Identity;
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
        protected readonly UserManager<ApplicationUser> userManager;

        public virtual IQueryable<TEntity> Collection
        {
            get
            {
                return context.Set<TEntity>()
                .Where(i => !i.IsAmendment)
                .OfType<TEntity>();
            }
        }

        public virtual IQueryable<TEntity> Amendments
        {
            get
            {
                return context.Set<TEntity>()
                .Where(i => i.IsAmendment)
                .OfType<TEntity>();
            }
        }


        public BaseRepository(TContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
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

        public virtual async Task<List<TEntity>> GetAmendment(int id)
        {
            return await Amendments.Where(a => a.BasedOn == id).ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAmendments()
        {
            return await Amendments.ToListAsync();
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
         
        public virtual async Task<List<TEntity>> GetAll(ApplicationUser user)
        {
            //TODO clean up
            if (user != null)
            {
                var isAdmin = userManager.GetRolesAsync(user).Result.FirstOrDefault(c => c.Contains("Admin")) != null;
                return await Collection.Where(i => isAdmin || i.IsPublic || i.CreatorId == user.Id).ToListAsync();
            } 
            else
            {
                return await Collection.Where(i => i.IsPublic).ToListAsync();
            }

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


        public virtual List<TEntity> GetSavedBy(string id)
        {
            var saved = context.Set<UserSavedItem>()
                .Where(i => i.SavedBy == id)
                .Include(i => i.UserItem); 

            var savedReturn = new List<TEntity>();

            foreach (var item in saved)
            {
                if (item.UserItem.GetType() == typeof(TEntity))
                    savedReturn.Add(item.UserItem as TEntity);
            }

            return savedReturn;
        }

        public async virtual Task<Dictionary<int, bool>> GetSavedIds(PaginatedList<TEntity> page)
        {
            var ids = page.Select(p => p.Id).ToList();
            var saved = await context.Set<UserSavedItem>()
                .Select(i => new { Key = i.UserItemId, Value = ids.Contains(i.Id) })
                .ToDictionaryAsync(k => k.Key, v => v.Value);
            return saved;
        }
        public async Task<List<TEntity>> GetAmendmentsFor(int id)
        {
            return await Amendments.Where(a => a.BasedOn == id).ToListAsync();
        }

        public async Task<bool?> ToggleSavedItem(int itemId, string userId)
        {
            var item = Collection.Where(i => i.Id == itemId).FirstOrDefault();

            var alreadySaved = context.Set<UserSavedItem>()
                .Where(i => i.UserItemId == itemId && i.SavedBy.Contains(userId)).FirstOrDefault();

            if (alreadySaved == null && item != null)
            {
                var entity = new UserSavedItem()
                {
                    SavedBy = userId,
                    UserItemId = item.Id
                };

                context.Set<UserSavedItem>().Add(entity);
                await context.SaveChangesAsync();
                return true;
            }
            else if (item != null)
            {
                context.Set<UserSavedItem>().Remove(alreadySaved);
                await context.SaveChangesAsync();

                return false;
            }

            return null;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task IncrementAmendments(int id)
        {
            var item = Collection.Where(i => i.Id == id).FirstOrDefault();
            item.AmendmentCount++;

            await context.SaveChangesAsync();
        }

        public async Task<int> Count()
        {
            return await Collection.CountAsync();
        }

        public async Task<int> GetNumberOfUnverifiedItems()
        {
            return Collection.Where(i => !i.IsVerified && i.IsPublic).Count();
        }

    }
}