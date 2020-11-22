using MyBulkMealsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Data
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<List<T>> GetAll(ApplicationUser user);
        Task<List<T>> GetByKeyword(string keyword);
        Task<List<T>> GetByCreationTime(bool descending);
        Task<List<T>> GetRandom(int quantity);
        Task<List<T>> GetCreatedBy(string id);
        List<T> GetSavedBy(string id);
        Task<List<T>> GetAmendmentsFor(int id);
        Task<Dictionary<int, bool>> GetSavedIds(PaginatedList<T> list);
        Task<T> Get(int id);
        Task<bool?> ToggleSavedItem(int id, string userId);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
        Task IncrementAmendments(int id);
        Task<int> Count();

    }
}
