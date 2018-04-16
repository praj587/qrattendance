
    $(function () {



        $("#jsGrid").jsGrid({
            width: "100%",
            filtering: false,
            inserting: false,
            editing: false,
            sorting: false,
            paging: true,
            autoload: true,

            pageSize: 3,
            pageButtonCount: 5,

            deleteConfirm: "Do you really want to delete User?",

            controller: {
                loadData: function (filter) {

                    return $.ajax({
                        type: "GET",
                        url: "/api/User",
                        data: filter,
                        dataType: "json",
                        contentType: 'application/json'

                    });
                },


                insertItem: function (item) {
                    return $.ajax({
                        type: "POST",
                        url: "/api/User",
                        data: JSON.stringify(item),
                        dataType: "json",
                        contentType: 'application/json'
                    });
                },

                updateItem: function (item) {
                    return $.ajax({
                        type: "PUT",
                        url: "/api/User/" + item.Cus_Id,
                        data: JSON.stringify(item),
                        dataType: "application/json; charset=UTF-8",
                        contentType: 'application/json'
                    });
                },

                deleteItem: function (item) {
                    return $.ajax({
                        type: "DELETE",
                        url: "/api/User/" + item.Cus_Id,
                        dataType: "json",
                        contentType: 'application/json'
                    });
                }
            },

            fields: [
                { name: "Cus_Id", type: "number", title: "ID",  align: "center", filter: false },
                { name: "Cus_Name", type: "text", title: "Name", align: "center" },
                { name: "Cus_Company", type: "text", title: "Company" },
                { name: "Cus_Mobile", type: "text", title: "Mobile" },
                { name: "Cus_Occupation", type: "text", title: "Position" },
                { name: "Cus_Email", type: "text", title: "Email" },
                {
                    type: "control", title: "View", editButton: false, deleteButton: false,
                    itemTemplate: function (value, item) {
                        var $result = jsGrid.fields.control.prototype.itemTemplate.apply(this, arguments);

                        var $customButton = $("<button class='btnjsview btn-xs btn-info glyphicon glyphicon-info-sign'>")
                            .text("")
                            .click(function (e) {
                                alert(item.Cus_Id);
                                e.stopPropagation();
                            });

                        return $result.add($customButton);
                    }
                },
                {
                    type: "control", title: "Print", editButton: false, deleteButton: false,
                    itemTemplate: function (value, item) {
                        var $result = jsGrid.fields.control.prototype.itemTemplate.apply(this, arguments);

                        var $customButton = $("<button class='btnjsprint btn-xs btn-info glyphicon glyphicon-print' data-toggle='modal' data-target='#myModal'>")
                            .text("")
                            .click(function (e) {
                                //alert(item.Cus_Id);
                                imageget(item.Cus_Id);
                                e.stopPropagation();
                            });

                        return $result.add($customButton);
                    }
                }
                //, { type: "control" }
            ]

        });

    });
function imageget(id) {
    $.ajax({
        type: "GET",
        url: '/Home/Qrgen',
        data: "Qrimput=" + id ,
        contentType: "image/png",
        success: function (data) {
         //   alert("Success"); // For testing
            $('#idimg').attr('src', data);
            //$('#myModal').toggle();
            $('#myModal').modal('toggle');
        }

    });
}
