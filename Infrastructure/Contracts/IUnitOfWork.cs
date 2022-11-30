namespace Infrastructure.Contracts
{
   public interface IUnitOfWork
   {
      /// <summary>
      /// Declare SaveChanges method.
      /// </summary>
      void SaveChanges();

      /// <summary>
      /// Declare SaveChangesAsync method.
      /// </summary>
      /// <returns></returns>
      Task SaveChangesAsync();

      ICategories CategoryRepository { get; }

      IExpenses ExpenseRepository { get; }
   }
}