app.controller("softwareController", function ($scope, $routeParams) {
    helper.setTitle("सफ्टवेयर सूची")

    $("#list").dxDataGrid({
        dataSource: helper.createStore({
            key: "id",
            loadUrl: rootPath + 'api/setup/software',
            saveUrl: rootPath + 'api/setup/savesoftware',
            deleteUrl: rootPath + 'api/setup/deletesoftware'
        }),
        editing: {
            mode: "popup",
            allowUpdating: true,
            allowAdding: true,
            allowDeleting: true,
            popup: {
                title: "SoftwareName",
                showTitle: true,
                width: 400,
                height: 250
            },
            form: {
                items: [
                    { dataField: "name", colSpan: 2 },
                    { dataField: "alias", colSpan: 2 },
                ]
            }

        },
        paging: {
            pageSize: 20
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [25, 50, 100]
        },
        filterRow: {
            visible: true,
            applyFilter: 'auto'
        },
        searchPannel: {
            visible: true,
            width: 240,
            placeholder: 'Search.....',
            highlightCaseSensitive: true
        },
        groupPannel: {
            visible: true
        },
        headerFilter: {
            visible: true
        },
        columnChooser: {
            enabled: true,
            mode: "dragAndDrop"
        },
        remoteOperations: false,
        onEditorPreparing: onEditorPreparing,
        onRowValidating: function (e) {

        },
        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        showRowLines: true,
        onToolbarPreparing: function (e) {
            e.toolbarOptions.items.unshift({
                locateInMenu: 'always',
                text: 'reset_layout',
                onClick: function () {
                    e.component.clearGrouping();
                    e.component.clearFilter();
                    e.component.clearSelection();
                    e.component.clearSorting();

                }
            })
        },
        columns: [
            {
                dataField: "name",
                caption: "software name",
                validationRules: [{ type: "required", message: 'कृपया software name राख्नुहोला' }]
            },
            {
                dataField: 'alias',
                caption: "उपनाम"
            },
        
        ]


    });


});

function onEditorPreparing(e) {
    if (e.parentType === 'datarow') {
        if (e.row.data.id > 0) {
            e.editorOptions.disabled = true;

        }
    }
}