
function LoadCategoryData() {
   let tableData = "";

   $('#table_body').empty();

      $.ajax({
         url: APIUrl + "categories",
         cache: false,
         type: "GET",
         dataType: "json",
         contentType: 'application/json',
         success: function (response) {

            response.map((values) => {
               tableData += `<tr>
                  <td>${values.name}</td>'
                  <td><button type="button" class="btn btn-primary edit" data-OID='${values.oid}'>Edit</button></td>
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

function GetCategoryNameByID(id) {
   $.ajax({
      url: APIUrl + "category/key/" + id,
      cache: false,
      type: "GET",
      dataType: "json",
      contentType: 'application/json',
      success: function (response) {

         $('#categoryname').val(response.name);
         $('#OID').val(response.oid);
         $('#goalmodal').modal('show');
      },
      failure: function (response) {
         alert(response.responseText);
      },
      error: function (response) {
         alert(response.responseText);
      }
   })
}

