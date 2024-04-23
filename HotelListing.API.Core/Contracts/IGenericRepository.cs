using HotelListingAPI.Core.Models;

namespace HotelListingAPI.Core.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(int? id);
        Task<TResult> GetAsync<TResult>(int? id); // novo
        
        Task<List<T>> GetAllAsync();
        Task<List<TResult>> GetAllAsync<TResult>(); //novo

        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);

        Task<T> AddAsync(T entity);
        Task<TResult> AddAsync<TSource, TResult>(TSource source); //novo
        Task UpdateAsync(T entity);
        Task UpdateAsync<TSource>(int id, TSource source); //novo
        Task DeleteAsync(int id);
        Task<bool>Exists(int id);
    }
}
