using AspNetCore.Reporting;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Web.Controllers
{
   public class ReportController : Controller
   {
      public class ReportDTO
      {
         public string Category { get; set; }
         public string Amount { get; set; }
         public string ExpenseDate { get; set; }
      }

      private readonly IWebHostEnvironment webHostEnvirnoment;
      public ReportController(IWebHostEnvironment webHostEnvirnoment)
      {
         this.webHostEnvirnoment = webHostEnvirnoment;
         System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
      }

      public IActionResult Index()
      {
         return View();
      }

      public async Task<IActionResult> Print(string FromDate, string ToDate, string Category)
      {
         var e = await GetExpenses(FromDate, ToDate, Category);

         string mimtype = "";
         int extension = 1;
         string CategoryName = "";
         string ReportName = "ExpenseReportByDate.rdlc";

         if (!string.IsNullOrEmpty(Category))
         {
            CategoryName = await getCategoryName(Category);
            ReportName = "ExpenseReportByCategory.rdlc";
         }

         var path = $"{this.webHostEnvirnoment.ContentRootPath}\\reports\\" + ReportName;
         Dictionary<string, string> parameters = new Dictionary<string, string>();

         parameters.Add("fromdate", "(" + FromDate + " - " + ToDate + ")");
         parameters.Add("IPAddress", GetLocalIPAddress());
         parameters.Add("category", CategoryName);
         parameters.Add("generateby", "Admin");

         LocalReport localReport = new LocalReport(path);
         localReport.AddDataSource("DataSet1", e);
         var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);
         return File(result.MainStream, "application/pdf");
      }

      public async Task<IActionResult> Export(string FromDate, string ToDate, string Category)
      {
         var e = await GetExpenses(FromDate, ToDate, Category);
         string mimetype = "";
         int extension = 1;

         Dictionary<string, string> parameters = new Dictionary<string, string>();
         parameters.Add("fromdate", "(" + FromDate + " - " + ToDate + ")");
         string CategoryName = "";
         string ReportName = "ExpenseReportByDate.rdlc";
         if (!string.IsNullOrEmpty(Category))
         {
            CategoryName = await getCategoryName(Category);
            ReportName = "ExpenseReportByCategory.rdlc";
         }

         var path = $"{this.webHostEnvirnoment.ContentRootPath}\\reports\\" + ReportName;
         parameters.Add("category", CategoryName);
         parameters.Add("generateby", "Admin");
         parameters.Add("IPAddress", GetLocalIPAddress());
         LocalReport lr = new LocalReport(path);
         lr.AddDataSource("DataSet1", e);
         var result = lr.Execute(RenderType.Excel, extension, parameters, mimetype);
         return File(result.MainStream, "application/msexcel", "Export.xls");
      }

      public async Task<DataTable> GetExpenses(string FromDate, string ToDate, string Category)
      {
         try
         {
            DateTime From = Convert.ToDateTime(FromDate);
            DateTime To = Convert.ToDateTime(ToDate);
            int CategoryID = Convert.ToInt16(Category);
            using var client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7193/expenses-api/expenses");

            string result = await response.Content.ReadAsStringAsync();
            var expenses = JsonConvert.DeserializeObject<List<Expense>>(result);
            List<ReportDTO> reportDTOs = new List<ReportDTO>();
            if (string.IsNullOrEmpty(Category))
            {
               reportDTOs = expenses.Where(x => x.ExpenseDate >= From.Date && x.ExpenseDate <= To.Date).Select(x => new ReportDTO
               {
                  Amount = x.ExpenseAmount.ToString(),
                  Category = x.CategoryID.ToString(),
                  ExpenseDate = x.ExpenseDate.Value.ToString("dd/MM/yyyy")
               }).ToList();
            }
            else
            {
               reportDTOs = expenses.Where(x => (x.ExpenseDate >= From.Date && x.ExpenseDate <= To.Date) && x.CategoryID == Convert.ToInt16(Category)).Select(x => new ReportDTO
               {
                  Amount = x.ExpenseAmount.ToString(),
                  Category = x.CategoryID.ToString(),
                  ExpenseDate = x.ExpenseDate.Value.ToString("dd/MM/yyyy")
               }).ToList();
            }
            foreach (var item in reportDTOs)
            {
               using var client2 = new HttpClient();
               var responses = await client2.GetAsync("https://localhost:7212/expenses-api/category/key/" + item.Category);
               string results = await responses.Content.ReadAsStringAsync();
               var CategoryName = JsonConvert.DeserializeObject<Category>(results);
               item.Category = CategoryName.Name;

            }
            var dt = ToDataTable(reportDTOs);
            return dt;
         }
         catch (Exception)
         {
            throw;
         }
      }

      public static DataTable ToDataTable<T>(List<T> items)
      {
         DataTable dataTable = new DataTable(typeof(T).Name);

         //Get all the properties
         PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
         foreach (PropertyInfo prop in Props)
         {
            //Defining type of data column gives proper data table 
            var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
            //Setting column names as Property names
            dataTable.Columns.Add(prop.Name, type);
         }
         foreach (T item in items)
         {
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {
               //inserting property values to datatable rows
               values[i] = Props[i].GetValue(item, null);
            }
            dataTable.Rows.Add(values);
         }
         //put a breakpoint here and check datatable
         return dataTable;
      }

      public async Task<string> getCategoryName(string CategoryID)
      {
         using var client2 = new HttpClient();
         var responses = await client2.GetAsync("https://localhost:7212/expenses-api/category/key/" + CategoryID);

         string results = await responses.Content.ReadAsStringAsync();
         var CategoryName = JsonConvert.DeserializeObject<Category>(results);
         return CategoryName.Name;
      }

      public static string GetLocalIPAddress()
      {
         var host = Dns.GetHostEntry(Dns.GetHostName());
         foreach (var ip in host.AddressList)
         {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
               return ip.ToString();
            }
         }
         throw new Exception("No network adapters with an IPv4 address in the system!");
      }
   }
}