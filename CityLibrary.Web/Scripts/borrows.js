var borrowsGrid = function (options) {
    var self = this;

    var config = {
        form: null,
        config: null
    };

    $.extend(config, config, options || {});

    this.gridDataSource = new kendo.data.DataSource({
        transport: {
            parameterMap: function (options, type) {
                var x = {};
                x.page = options.page - 1;
                x.pageSize = options.pageSize;
                return kendo.stringify(x);
            },
            read: {
                url: config.baseUrl + 'GetBorrows',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json'
            }
        },
        schema: {
            model: {
                fields: {
                    BorrowId: { type: "number" },
                    Title: { type: "string" },
                    Author: { type: "string" },
                    FromDate: { type: "date" },
                    ToDate: { type: "date" },
                    UserFullName: { type: "string" }
                }
            },
            data: "Borrows",
            total: "Count"
        },
        pageSize: 20,
        serverPaging: true,
        serverSorting: true
    });

    // Inicialization
    this.init = function () {
        $("#grid").kendoGrid({
            dataSource: self.gridDataSource,
            groupable: false,
            resizable: true,
            sortable: true,
            pageable: true,
            columns: [
            {
                field: "BorrowId",
                width: "70px",
                title: "Id"
            },
            {
                field: "FromDate",
                width: "100px",
                title: "Od",
                template: kendo.template('#= kendo.toString(FromDate, "yyyy-MM-dd") #')
            },
            {
                field: "ToDate",
                width: "100px",
                title: "Do",
                template: kendo.template('#= kendo.toString(ToDate, "yyyy-MM-dd") #')
            },
            {
                field: "Title",
                width: "100%",
                title: "Title"
            },
            {
                field: "Author",
                width: "200px",
                title: "Author"
            },
            {
                field: "UserFullName",
                width: "200px",
                title: "User"
            },
            {
                width: "30px",
                template: kendo.template('<a href="" data-id="#= BorrowId #" title="Usuń" class="remove-borrow"><i class="icon-remove"></i></a>')
            }
            ]
        });

        ko.applyBindings(self, config.form.closest('#borrows-content')[0]);
    };

    this.init();
}