$(document).ready(function () {
    $("#jqGrid").jqGrid({
        url: "/Home/GetUsers",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['ID', 'Name', 'Company', 'Position', 'Mobile','Print'],
        colModel: [
            { key: true, hidden: true, name: 'Cus_Id', index: 'Cus_Id', editable: false },
            { key: false, name: 'Cus_Name', index: 'Cus_Name', editable: true },
            { key: false, name: 'Cus_Mobile', index: 'Cus_Mobile', editable: true },
            { key: false, name: 'Cus_Company', index: 'Cus_Company', editable: true },
            { key: false, name: 'Cus_Occupation', index: 'Cus_Occupation', editable: true },
            {
                name: 'action', align: 'center', width: 25, sortable: false
                , title: false, fixed: true, search: false
                , formatter: function (cellvalue, options, rowObject) {
                    var stImageLinks = '<a title="Print"   href = "/Home/Register" > <img src="../images/edit.png" /></a > ';
                    return stImageLinks;
                }
            }
        ],
              pager: jQuery('#jqControls'),
        rowNum: 10,
        rowList: [10, 20, 30, 40, 50],
        height: '100%',
        viewrecords: true,
        caption: 'Users Records',
        emptyrecords: 'No Users Records are Available to Display',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        autowidth: true,
        multiselect: false,
    }).navGrid('#jqControls', {
        edit: true, add: true, del: true, search: true,
        searchtext: "Search User", refresh: true
    },
        {
            zIndex: 100,
            url: '/Home/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            zIndex: 100,
            url: "/Home/Create",
            closeOnEscape: true,
            closeAfterAdd: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            zIndex: 100,
            url: "/Home/Delete",
            closeOnEscape: true,
            closeAfterDelete: true,
            recreateForm: true,
            msg: "Are you sure you want to delete User... ? ",
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            zIndex: 100,
            caption: "Search Users",
            sopt: ['cn']
        });
});
