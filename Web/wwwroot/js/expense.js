
function CategoryDropDown() {
   var Categorydropdown = $("#expenseCategory");

   $.ajax({
      url: APIUrl + "categories",
      cache: false,
      type: "GET",
      dataType: "json",
      contentType: 'application/json',
      success: function (response) {
         Categorydropdown.append($("<option />").val("").text("--Please Select--"));
         $.each(response, function () {
            Categorydropdown.append($("<option />").val(this.oid).text(this.name));
         });
      },
      failure: function (response) {
         alert(response.responseText);
      },
      error: function (response) {
         alert(response.responseText);
      }
   })
}

function expensesDetail(id) {
   $.ajax({
      url: APIUrl + "expense/key/" + id,
      cache: false,
      type: "GET",
      dataType: "json",
      contentType: 'application/json',
      success: function (response) {

         console.log(response);

         GetCategoryNameByID(response.categoryID);
         $('#expenseAmount').html(response.expenseAmount);
         var d = new Date(response.expenseDate);
         var month = d.getMonth() + 1;
         var day = d.getDate();
         var output = d.getFullYear() + '/' +
            (('' + month).length < 2 ? '0' : '') + month + '/' +
            (('' + day).length < 2 ? '0' : '') + day;
         $('#expenseDate').html(new Date(response.expenseDate).toLocaleDateString('en-GB'));
         if (~window.location.pathname.indexOf("update")) {
            $('#OID').val(response.oid);
            $('#ExpenseAmount').val(response.expenseAmount);
            $('#expenseCategory').val(response.categoryID);
            $('#ExpenseDate').val(new Date(response.expenseDate).toLocaleDateString('en-GB'));
         }
      },
      failure: function (response) {
         alert(response.responseText);
      },
      error: function (response) {
         alert(response.responseText);
      }
   })
}

function GetCategoryNameByID(id) {
   $.ajax({
      url: APIUrl + "category/key/" + id,
      cache: false,
      type: "GET",
      dataType: "json",
      contentType: 'application/json',
      success: function (response) {
         $('#Category').html(response.name);
      },
      failure: function (response) {
         alert(response.responseText);
      },
      error: function (response) {
         alert(response.responseText);
      }
   })
}

function LoadExpenseData() {
   let tableData = "";
   $.ajax({
      url: APIUrl + "expenses",
      cache: false,
      type: "GET",
      dataType: "json",
      contentType: 'application/json',
      success: function (response) {
         console.log(response)
         response.map((values) => {
            tableData += `<tr>
                 <td>${new Date(values.expenseDate).toLocaleDateString('en-GB')}</td>
                 <td>${values.expenseAmount}</td>  
                 <td>${values.categories.name}</td>
                 <td><a href='update/${values.oid}' class='btn btn-primary'>Edit</a>
                 <a href='details/${values.oid}' class='btn btn-primary'>Detail</a>
                 </td>

            </tr>`;
         });
         $('#table_body').append(tableData);
         $(".loader").hide();
      },
      failure: function (response) {
         alert(response.responseText);
         $(".loader").hide();
      },
      error: function (response) {
         alert(response.responseText);
         $(".loader").hide();
      }
   })
}

$(document).ready(function () {

   CategoryDropDown();

   var lastDate = new Date();

   $('#ExpenseDate').datepicker({
      dateFormat: 'dd/mm/yy',
      maxDate: new Date,
      yearRange: "-100:+0",
      changeMonth: true,
      changeYear: true,
      setDate: lastDate
   }).datepicker('option', 'dateFormat', 'dd/mm/yy');
   if (~window.location.pathname.indexOf("details") || ~window.location.pathname.indexOf("update")) {
      var ID = location.href.substring(location.href.lastIndexOf('/') + 1);
      expensesDetail(ID);
   }
   if ($('#tblData').length > 0) {
      $(".loader").show();
      LoadExpenseData();
   }

   $('#SubmitExpenses').click(function () {
      if ($('#frmExpense').valid() == false) {
         document.getElementById("ExpenseDate-error").style.color = "red";
         document.getElementById("ExpenseDate-error").innerText = "Expense Date is required";
         document.getElementById("ExpenseAmount-error").style.color = "red";
         document.getElementById("ExpenseAmount-error").innerText = "Expense Amount is required";
         if ($('#expenseCategory').valid() == false) {
            document.getElementById("expenseCategory-error").style.color = "red";
            document.getElementById("expenseCategory-error").innerText = "Expense Category is required";
            return;
         }
         return;
      }
      if ($('#expenseCategory').valid() == false) {
         document.getElementById("expenseCategory-error").style.color = "red";
         document.getElementById("expenseCategory-error").innerText = "Expense Category is required";
         return;
      }

      var urlCreateUpdate = ''
      var expenses = new Object();
      expenses.CategoryID = $('#expenseCategory').val();
      expenses.ExpenseDate = $('#ExpenseDate').val();
      var d1 = expenses.ExpenseDate;
      var d = new Date(d1.replace(/^(\d{1,2}\/)(\d{1,2}\/)(\d{4})$/, "$2$1$3"));
      var dd = d.getDate();
      var mm = d.getMonth() + 1;
      var yy = d.getFullYear();
      if (dd < 10) {
         dd = '0' + dd;
      }
      if (mm < 10) {
         mm = '0' + mm;
      }
      var newdate = yy + "-" + mm + "-" + dd;
      expenses.ExpenseDate = newdate;
      expenses.ExpenseAmount = $('#ExpenseAmount').val();
      var Method = '';
      debugger
      if ($('#OID').val() != undefined) {
         urlCreateUpdate = 'expense/' + $('#OID').val();
         expenses.OID = $('#OID').val();
         Method = 'PUT';
      } else {
         urlCreateUpdate = 'expense';
         Method = 'POST';
      }

      $.ajax({
         type: Method,
         url: APIUrl + urlCreateUpdate,
         data: JSON.stringify(expenses),
         contentType: "application/json; charset=utf-8",
         dataType: "json",
         success: function (response) {
            console.log(response);
            if (response != null) {
               bootbox.alert({
                  message: "Expense Successfully Added",
                  backdrop: true,
                  centerVertical: true,
                  closeButton: false,
                  callback: function () {
                     location.href = '/expenses/details/' + response.oid;
                     console.log(response);
                  }
               });

            }
            else {
               bootbox.alert({
                  message: "Successfully Updated",
                  backdrop: true,
                  centerVertical: true,
                  closeButton: false,
                  callback: function () {
                     location.href = '/expenses/details/' + $('#OID').val();
                  }
               });
            }
         },

         failure: function (response) {
            /*alert(response.responseText);*/
            bootbox.alert({
               title: "Error Occurred",
               message: response.responseText,
               backdrop: false,
               closeButton: false,
               centerVertical: true
            });
         },

         error: function (response) {
            /*alert(response.responseText);*/
            bootbox.alert({
               title: "Error Occurred",
               message: response.responseText,
               backdrop: false,
               closeButton: false,
               centerVertical: true
            });
         }
      });
   })
})