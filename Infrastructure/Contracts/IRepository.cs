using System.Linq.Expressions;

namespace Infrastructure.Contracts
{
   /// <summary>
   /// Contains signature of all generic methods.
   /// </summary>
   /// <typeparam name="T">T is a Model class</typeparam>
   public interface IRepository<T>
   {
      /// <summary>
      ///  Inserts information available in the given object.
      /// </summary>
      /// <param name="entity">Object name</param>
      /// <returns>Inserted object</returns>
      T Add(T entity);

      /// <summary>
      /// add multi object to database
      /// </summary>
      /// <param name="entities"></param>
      void AddRange(IEnumerable<T> entities);

      /// <summary>
      /// Updates database table with the information available in the given object.
      /// </summary>
      /// <param name="entity">Object to be updated.</param>
      T Update(T entity);

      /// <summary>
      /// Deletes the given object.
      /// </summary>
      /// <param name="entity">Delete to be removed</param>
      void Delete(T entity);

      /// <summary>
      /// delete multi objects
      /// </summary>
      /// <param name="entities"></param>
      void RemoveRange(IEnumerable<T> entities);

      /// <summary>
      /// Searches using primary key.
      /// </summary>
      /// <param name="Key">Primary Key of the Table</param>
      /// <returns>Retrieved row in the form of model object.</returns>
      T GetById(Guid id);

      T GetById(int Key);

      /// <summary>
      /// Searches using primary key.
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      Task<T?> GetByIdAsync(Guid id);

      /// <summary>
      /// Searches using primary key.
      /// </summary>
      /// <param name="key"></param>
      /// <returns></returns>
      Task<T?> GetByIdAsync(int key);

      /// <summary>
      ///  Loads all rows from the database table.
      /// </summary>
      /// <returns>Object list.</returns>
      IQueryable<T> GetAll();

      IQueryable<T> GetAll(string Include);
      /// <summary>
      /// Searches using the given criteria.
      /// </summary>
      /// <param name="predicate">Search criteria.</param>
      /// <returns>Object list.</returns>
      Task<IEnumerable<T>> IQueryAsync(Expression<Func<T, bool>> predicate);

      /// <summary>
      /// Searches using the given criteria.
      /// </summary>
      /// <param name="predicate">Search criteria.</param>
      /// <returns>Retrieved row in the form of model object.</returns>
      T FirstOrDefault(Expression<Func<T, bool>> predicate);

      /// <summary>
      /// Searches using the given criteria.
      /// </summary>
      /// <param name="predicate"></param>
      /// <returns></returns>
      Task<T?> FirstOrDefaultAsync(Expression<Func<T?, bool>> predicate);
   }
}