namespace Utilities.Constants
{
    public static class RouteConstants
    {
        public const string BaseRoute = "expenses-api";

        #region Category
        public const string CreateCategory = "category";

        public const string ReadCategories = "categories";

        public const string ReadCategoryByKey = "category/key/{key}";

        public const string UpdateCategory = "category/{key}";

        public const string DeleteCategory = "category/{key}";
        #endregion

        #region Expense
        public const string CreateExpense = "expense";

        public const string ReadExpenses = "expenses";

        public const string ReadExpenseByKey = "expense/key/{key}";

        public const string UpdateExpense = "expense/{key}";

        public const string DeleteExpense = "expense/{key}";
        #endregion
    }
}