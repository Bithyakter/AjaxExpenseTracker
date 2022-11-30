using Infrastructure.Contracts;
using Infrastructure.Sql;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
   public class UnitOfWork : IUnitOfWork
   {
      private readonly IConfiguration configuration;
      protected readonly DataContext dbcontext;

      public UnitOfWork(DataContext context, IConfiguration configuration)
      {
         this.dbcontext = context;
         this.configuration = configuration;
      }

      public void SaveChanges()
      {
         dbcontext.SaveChanges();
      }

      public async Task SaveChangesAsync()
      {
         await dbcontext.SaveChangesAsync();
      }

      #region CategoryRepository
      private ICategories categoryRepository;
      public ICategories CategoryRepository
      {
         get
         {
            if (categoryRepository == null)
               categoryRepository = new CategoryRepository(dbcontext);

            return categoryRepository;
         }
      }
      #endregion CategoryRepository

      #region ExpenseRepository
      private IExpenses expenseRepository;
      public IExpenses ExpenseRepository
      {
         get
         {
            if (expenseRepository == null)
               expenseRepository = new ExpenseRepository(dbcontext);

            return expenseRepository;
         }
      }
      #endregion ExpenseRepository
   }
}