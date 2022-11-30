/*
 * Created by: Phoenix(2)
 * Date created: 22.11.2022
 * Modified by:
 * Last modified:
 */

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
                <td>${values.name}</td>
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

$(document).ready(function () {
    $(".loader").show();
    LoadCategoryData();
    $('#goalmodal').on('hidden.bs.modal', function () {
        $('#categoryname').val('');
        $('#OID').val(0);
    });
$('#btn').on('click', function () {
    $('#goalmodal').modal('show');
    $('#dismiss').modal('hide');
});
    $(document).on('click', '.edit', function () {

        var OID = $(this).attr('data-oid');
        GetCategoryNameByID(OID);

    })

    $('#SubmitCategory').click(function () {
        debugger
    var Name = $('#categoryname').val();
    var OID = $('#OID').val();
    var Category = new Object();
    Category.Name = $('#categoryname').val();
        Category.OID = OID;
        var Method = '';
        if ($('#categoryname').val() == "") {
            document.getElementById("category_Text").innerHTML = "Field required";
            document.getElementById("category_Text").style.color = "red";
        }
        else {

            var urlCreateUpdate = ''
            if (OID != '0') {
                urlCreateUpdate = 'category/' + $('#OID').val();
                Method = 'PUT';
            } else {
                urlCreateUpdate = 'category';
                Method = 'POST';
            }
            $.ajax({
                type: Method,
                url: APIUrl + urlCreateUpdate,
                data: JSON.stringify(Category),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response != null) {
                        console.log(response);
                     
                        bootbox.alert({
                            message: "Category Successfully Added",
                            backdrop: true,
                            centerVertical: true,
                            closeButton: false,
                            callback: function () {
                                $('#goalmodal').modal('hide');
                                LoadCategoryData();

                            }
                        }); 

                        $('#categoryname').val('');
                        $('#OID').val(0);
                    } else {
                   
                        bootbox.alert({
                            message: "Category Successfully Updated",
                            backdrop: true,
                            centerVertical: true,
                            closeButton: false,
                            callback: function () {
                                $('#goalmodal').modal('hide');
                                LoadCategoryData();

                            }
                        }); 
                    }
                },
                failure: function (response) {
                    bootbox.alert({
                        title: "Error Occurred",
                        message: response.responseText,
                        backdrop: true,
                        centerVertical: true
                    });
                },
                error: function (response) {
                    bootbox.alert({
                        title: "Error Occurred",
                        message: response.responseText,
                        backdrop: true,
                        centerVertical: true
                    });
                }
            });
        }
    })
})