using Domain;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.Constants;

namespace API.Controllers
{
   /// <summary>
   /// NewExpense controller.
   /// </summary>
   [Route(RouteConstants.BaseRoute)]
   [ApiController]
   public class ExpenseController : ControllerBase
   {
      private readonly IUnitOfWork context;
      private readonly ILogger<ExpenseController> logger;

      /// <summary>
      /// Default constructor.
      /// </summary>
      /// <param name="context">Instance of the UnitOfWork.</param>
      public ExpenseController(IUnitOfWork context, ILogger<ExpenseController> logger)
      {
         this.context = context;
         this.logger = logger;
      }

      /// <summary>
      /// URL: expenses-api/expense
      /// </summary>
      /// <param name="expense">Expense object.</param>
      /// <returns>Http status code: CreatedAt.</returns>
      [HttpPost]
      [Route(RouteConstants.CreateExpense)]
      public async Task<IActionResult> CreateExpense(Expense expense)
      {
         try
         {
            expense.DateCreated = DateTime.Now;
            expense.IsDeleted = false;
            expense.IsSynced = false;

            context.ExpenseRepository.Add(expense);
            await context.SaveChangesAsync();

            return CreatedAtAction("ReadExpenseByKey", new { key = expense.OID }, expense);
         }
         catch (Exception ex)
         {
            logger.LogError(ex.Message, "Creating Expense");
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: expenses-api/expenses
      /// </summary>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadExpenses)]
      public async Task<IActionResult> ReadExpenses()
      {
         try
         {
            var expense = await context.ExpenseRepository.GetAll("Categories").ToListAsync();

            return Ok(expense);
         }
         catch (Exception ex)
         {
            logger.LogError(ex.Message, "ReadExpenses");
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: expenses-api/expense/key/{key}
      /// </summary>
      /// <param name="key">Primary key of the table Expenses.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpGet]
      [Route(RouteConstants.ReadExpenseByKey)]
      public async Task<IActionResult> ReadExpenseByKey(int key)
      {
         try
         {
            if (key <= 0)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

            var expense = await context.ExpenseRepository.GetByIdAsync(key);

            if (expense == null)
               return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

            return Ok(expense);
         }
         catch (Exception ex)
         {
            logger.LogError(ex.Message, "ReadExpenseByKey");
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: expenses-api/expense/{key}
      /// </summary>
      /// <param name="key">Primary key of the table Expenses.</param>
      /// <param name="expense">Expense to be updated.</param>
      /// <returns>Http Status Code: NoContent.</returns>
      [HttpPut]
      [Route(RouteConstants.UpdateExpense)]
      public async Task<IActionResult> UpdateExpense(int key, Expense expense)
      {
         try
         {
            if (key != expense.OID)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

            expense.DateModified = DateTime.Now;
            expense.IsSynced = false;

            context.ExpenseRepository.Update(expense);
            await context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
         }
         catch (Exception ex)
         {
            logger.LogError(ex.Message, "UpdateExpense");
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }

      /// <summary>
      /// URL: expenses-api/expense/{key}
      /// </summary>
      /// <param name="key">Primary key of the table Expenses.</param>
      /// <returns>Http status code: Ok.</returns>
      [HttpDelete]
      [Route(RouteConstants.DeleteExpense)]
      public async Task<IActionResult> DeleteExpense(int key)
      {
         try
         {

            if (key <= 0)
               return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

            var expenseInDb = context.ExpenseRepository.GetById(key);

            if (expenseInDb == null)
               return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

            expenseInDb.DateModified = DateTime.Now;
            expenseInDb.IsDeleted = true;
            expenseInDb.IsSynced = false;

            context.ExpenseRepository.Update(expenseInDb);
            await context.SaveChangesAsync();

            return Ok(expenseInDb);
         }
         catch (Exception ex)
         {
            logger.LogError(ex.Message, "DeleteExpense");
            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
         }
      }
   }
}