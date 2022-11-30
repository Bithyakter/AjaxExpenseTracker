using Domain;

namespace Infrastructure.Contracts
{
   public interface ICategories : IRepository<Category>
   {
      public Task<Category> GetCategoryByName(string Name);

      public Task<IEnumerable<Category>> GetCategories();

      public Task<Category> GetCategoryByKey(int key);
   }
}