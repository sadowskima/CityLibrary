var booksGrid = function (options) {
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
                url: config.baseUrl + 'GetBooks',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json'
            }
        },
        schema: {
            model: {
                fields: {
                    BookId: { type: "number" },
                    Author: { type: "string" },
                    Title: { type: "string" },
                    ReleaseDate: { type: "date" },
                    ISBN: { type: "string" },
                    BookGenreId: { type: "number" },
                    Count: { type: "number" },
                    AddDate: { type: "date" },
                    ModifiedDate: { type: "date" }
                   // Action: { type: "string" }

                }
            },
            data: "Books",
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
            groupable: true,
			resizable: true,
            sortable: true,
            pageable: {
                refresh: true,
                pageSizes: true
            },
            columns: [
            {
                field: "BookId",
                width: 1,
                title: "Id"
            },
            {
                field: "Author",
                width: 10,
                title: "Author"
            },
            {
                field: "Title",
                width: 12,
                title: "Title"
            },
            {
                field: "ReleaseDate",
                width: 5,
                title: "RelDate",
                template: kendo.template('#= kendo.toString(ReleaseDate, "yyyy-MM-dd") #')
            },
            {
                field: "ISBN",
                width: 8,
                title: "ISBN"
            },
            {
                field: "BookGenreId",
                width: 3,
                title: "Genre"
            },
            {
                field: "Count",
                width: 3,
                title: "Count"
            },
            {
                field: "AddDate",
                width: 5,
                title: "AddDate",
                template: kendo.template('#= kendo.toString(AddDate, "yyyy-MM-dd") #')
            },
            {
                field: "ModifiedDate",
                width: 5,
                title: "ModifiedDate",
                template: kendo.template('#= kendo.toString(ModifiedDate, "yyyy-MM-dd") #')
            },
            {
                field: "",
                width: 2,
                title: "",
                template: kendo.template('<a data-id="#= BookId #" class="edit-book"><i class="icon-edit"></i></a>')
            }
            ]
        });

        ko.applyBindings(self, config.form.closest('#books-content')[0]);
    };

    this.init();
}