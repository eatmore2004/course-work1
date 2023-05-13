
using BLL.Abstractions.Interfaces;
using Core.Models;
using DAL.Abstractions;

namespace BLL.Services
{
    public abstract class GenericService<T> : IGenericService<T> where T : BaseEntity
    {
        protected readonly IRepository<T> Repository;

        protected GenericService(IRepository<T> repository)
        {
            Repository = repository;
        }

        public virtual async Task Add(T obj)
        {
            try
            {
                var result = await Repository.AddAsync(obj);

                if (!result.IsSuccessful)
                {
                    throw new Exception($"Failed to add {typeof(T).Name}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add {typeof(T).Name}. Exception: {ex.Message}");
            }
        }

        public virtual async Task Delete(Guid id)
        {
            try
            {
                var result = await Repository.DeleteAsync(id);

                if (!result.IsSuccessful)
                {
                    throw new Exception($"Failed to delete {typeof(T).Name} with Id {id}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete {typeof(T).Name} with Id {id}. Exception: {ex.Message}");
            }
        }

        public virtual async Task<T> GetById(Guid id)
        {
            try
            {
                var result = await Repository.GetByIdAsync(id);

                if (!result.IsSuccessful)
                {
                    throw new Exception($"Failed to get {typeof(T).Name} by Id {id}.");
                }

                return result.Data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get {typeof(T).Name} by Id {id}. Exception: {ex.Message}");
            }
        }

        public virtual async Task<List<T>> GetAll()
        {
            try
            {
                var result = await Repository.GetAllAsync();
                
                if (!result.IsSuccessful)
                {
                    throw new Exception($"Failed to get all {typeof(T).Name}s.");
                }

                return result.Data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get all {typeof(T).Name}s. Exception: {ex.Message}");
            }
        }

        public async Task<T> GetByPredicate(Func<T, bool> predicate)
        {
            try
            {
                var result = await Repository.GetByPredicateAsync(predicate);
                
                if (!result.IsSuccessful)
                {
                    throw new Exception($"Failed to get by predicate {typeof(T).Name}s.");
                }

                return result.Data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get by predicate {typeof(T).Name}s. Exception: {ex.Message}");
            }
        }

        public virtual async Task Update(Guid id, T obj)
        {
            try
            {
                var result = await Repository.UpdateAsync(id, obj);
                
                if (!result.IsSuccessful)
                {
                    throw new Exception($"Failed to update {typeof(T).Name} with Id {id}.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update {typeof(T).Name} with Id {id}. Exception: {ex.Message}");
            }
        }
    }
}