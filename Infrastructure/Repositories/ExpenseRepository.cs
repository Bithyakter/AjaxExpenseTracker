using Domain;
using Infrastructure.Contracts;
using Infrastructure.Sql;

namespace Infrastructure.Repositories
{
   public class ExpenseRepository : Repository<Expense>, IExpenses
   {
      protected DataContext context;

      public ExpenseRepository(DataContext dbcontext) : base(dbcontext)
      {
         this.context = dbcontext;
      }
   }
}