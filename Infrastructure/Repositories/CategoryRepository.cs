
using Domain;
using Infrastructure.Contracts;
using Infrastructure.Sql;

namespace Infrastructure.Repositories
{
   public class CategoryRepository : Repository<Category>, ICategories
   {
      private readonly DataContext context;

      public CategoryRepository(DataContext context) : base(context)
      {
         this.context = context;
      }

      public async Task<IEnumerable<Category>> GetCategories()
      {
         try
         {
            return await IQueryAsync(c => c.IsDeleted == false);
         }

         catch (Exception)
         {
            throw;
         }
      }

      public async Task<Category> GetCategoryByKey(int key)
      {
         try
         {
            return await FirstOrDefaultAsync(p => p.OID == key && p.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

      public async Task<Category> GetCategoryByName(string Name)
      {
         try
         {
            return await FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == Name.ToLower().Trim() && c.IsDeleted == false);
         }
         catch (Exception)
         {
            throw;
         }
      }

   }
}