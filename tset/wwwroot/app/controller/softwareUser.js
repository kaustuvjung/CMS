app.controller("softwareUserController", function ($scope, $window) {
    helper.setTitle("softwareUser ")

    const departments = context.getCachedData(context.cacheConfig.department);
    const wards = context.getCachedData(context.cacheConfig.ward);

    var lookupDesigner = {
        store: new DevExpress.data.CustomStore({
            key: "id",
            loadMode: "raw",
            load: function () {
                return $.getJSON(rootPath + 'api/setup/designers');
            }
        })
    }

    var lookupPermission = {
        store: new DevExpress.data.CustomStore({
            key: "id",
            loadMode: "raw",
            load: function () {
                return $.getJSON(rootPath + 'api/common/permissions');
            }
        }),
    }
    onEditingStart = (e) => {
        e.component.columnOption("password", "formItem.visible", false);
        e.component.columnOption("wardId", "formItem.visible", false);
        e.component.columnOption("designerId", "formItem.visible", false);
        if (e.data.departmentId.indexOf(3) != -1) {
            e.component.columnOption("wardId", "formItem.visible", true);
        }
        if (e.data.departmentId.indexOf(1) != -1) {
            e.component.columnOption("designerId", "formItem.visible", true);
        }
    }
    onInitNewRow = (e) => {
        e.component.columnOption("password", "formItem.visible", true);
        e.component.columnOption("designerId", "formItem.visible", false);
        e.component.columnOption("wardId", "formItem.visible", false);
        //if (e.data.departmentId == 3) {
        //    e.component.columnOption("wardId", "formItem.visible", true);
        //}
        //if (e.data.departmentId == 1) {
        //    e.component.columnOption("designerId", "formItem.visible", true);
        //}
    }


    var dataGrid = $("#list").dxDataGrid({
        dataSource:[],
        //    helper.createStore({
        //    key: 'id',
        //    loadUrl: rootPath + 'software/softwareusers',
        //    saveUrl: rootPath + 'software/savesoftwareuser',
        //    deleteUrl: rootPath + 'software/deletesoftwareuser'
        //}),
        editing: {
            mode: "popup",
            allowUpdating: true,
            allowAdding: true,
            allowDeleting: true,
            popup: {
                title: "User",
                showTitle: true,
                width: 600,
                height: 500
            },
            form: {
                items: [
                    { dataField: "ProvinceId", colSpan: 2 },
                    { dataField: "DistrictId", colSpan: 2 },
                    { dataField: "LocalBodyId", colSpan: 2 },
                    { dataField: "SoftwareId", colSpan: 2 },
                    { dataField: "email", colSpan: 2 },
                    { dataField: "mobileNumber", colSpan: 2 },
                ]
            }
        },
        paging: {
            pageSize: 25
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 25, 50, 100]
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: 'Search...',
        },
        groupPanel: {
            visible: true,
        },
        headerFilter: {
            visible: true,
        },
        columnChooser: {
            enabled: true,
            mode: "dragAndDrop"
        },
        remoteOperations: false,
        searchPanel: {
            visible: true,
            highlightCaseSensitive: true
        },
        allowColumnReordering: true,
        rowAlternationEnabled: true,
        showBorders: true,
        showRowLines: true,
        onEditingStart: onEditingStart,
        onInitNewRow: onInitNewRow,
        onRowUpdating: function (e) {
            if (e.newData.hasOwnProperty("departmentId")) {
                if (!$.isArray(e.newData.departmentId))
                    e.newData.departmentId = $.makeArray(e.newData.departmentId);
            } else {
                e.newData.departmentId = JSON.parse(e.oldData.departmentId);
            }

            if (e.newData.hasOwnProperty("wardId")) {
                if (!$.isArray(e.newData.wardId))
                    e.newData.wardId = $.makeArray(e.newData.wardId);
            } else {
                e.newData.wardId = JSON.parse(e.oldData.wardId);
            }
        },
        onEditorPreparing: function (e) {
            if (e.parentType === "dataRow" && e.dataField === "departmentId") {
                e.editorName = "dxTagBox";
                e.editorOptions.dataSource = departments;
                e.editorOptions.displayExpr = "name";
                e.editorOptions.valueExpr = "id";

                console.log(e.value);
                if (e.value && !$.isArray(e.value))
                    e.editorOptions.value = JSON.parse(e.value) || [];
                else
                    e.editorOptions.value = e.value || [];

                console.log(e.editorOptions.value);
                e.editorOptions.onValueChanged = function (args) {
                    e.setValue(args.value);

                    console.log(args.value);
                    var value = args.value;

                    if (value.indexOf(1) != -1) {
                        dataGrid.columnOption("designerId", "formItem.visible", true);
                    }
                    else {
                        dataGrid.columnOption("designerId", "formItem.visible", false);
                    }

                    if (value.indexOf(3) !== -1) {
                        dataGrid.columnOption("wardId", "formItem.visible", true);
                    }
                    else {
                        dataGrid.columnOption("wardId", "formItem.visible", false);
                    }
                }
            }

            if (e.parentType === "dataRow" && e.dataField === "wardId") {
                e.editorName = "dxTagBox";
                e.editorOptions.dataSource = wards;
                e.editorOptions.displayExpr = "nameNp";
                e.editorOptions.valueExpr = "id";

                if (e.value && !$.isArray(e.value))
                    e.editorOptions.value = JSON.parse(e.value) || [];
                else
                    e.editorOptions.value = e.value || [];

                e.editorOptions.onValueChanged = function (args) {
                    e.setValue(args.value);
                }
            }
        },
        onToolbarPreparing: function (e) {
            e.toolbarOptions.items.unshift({
                locateInMenu: 'always',
                text: 'Reset Layout',
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
                dataField: "ProvinceId",
                caption: "प्रदेश नाम",
                validationRules: [{ type: "required", message: 'कृपया प्रयोगकर्ता नाम राख्नुहोला' }]
            },
            {
                dataField: "districtId",
                caption: "जिल्ला",
                lookup: {
                    dataSource: lookupPermission,
                    valueExpr: "id",
                    displayExpr: "displayName"
                },
                validationRules: [{ type: "required", message: 'कृपया परमिशन छान्नुहोस्' }]
            },
            {
                dataField: "localBodyId",
                caption: "स्थानीय निकाय",
                //setCellValue: function (rowData, value) {
                //    //rowData.departmentId = value;
                //    //if (value != 1) { //Not Designer
                //    //    rowData.designerId = 0;
                //    //}
                //    //if (value != 3) { //Not Ward
                //    //    rowData.wardId = 0;
                //    //}
                //},
                calculateDisplayValue: function (e) {
                    var departmentIds, departmentNames = [];

                    if (e.departmentId && !$.isArray(e.departmentId))
                        departmentIds = JSON.parse(e.departmentId) || [];
                    else
                        departmentIds = e.departmentId || [];

                    for (var i = 0; i < departmentIds.length; i++) {
                        departmentNames.push(departments.filter(x => x.id == departmentIds[i])[0]?.name);
                    }
                    return departmentNames.join(', ');
                },
                validationRules: [{ type: "required", message: 'कृपया शाखा छान्नुहोस्' }]
            },
            {
                dataField: "softwareId",
                caption: "सफ्टवेयर",
                calculateDisplayValue: function (e) {
                    var wardIds, wardNames = [];

                    if (e.wardId && !$.isArray(e.wardId))
                        wardIds = JSON.parse(e.wardId) || [];
                    else
                        wardIds = e.wardId || [];

                    for (var i = 0; i < wardIds.length; i++) {
                        wardNames.push(wards.filter(x => x.id == wardIds[i])[0]?.nameNp);
                    }
                    return wardNames.join(', ');
                },
                validationRules: [{ type: "required", message: 'कृपया सफ्टवेयर छान्नुहोस्' }]
            },
            //{
            //    dataField: "designerId",
            //    caption: "डिजाईनर",
            //    lookup: {
            //        dataSource: lookupDesigner,
            //        valueExpr: "id",
            //        displayExpr: "name"
            //    }
            //},
            {
                dataField: "email",
                caption: "ईमेल",
                validationRules: [{
                    type: "required",
                    message: "कृपया ईमेल राख्नुहोला"
                },
                {
                    type: "email",
                    message: "Email is invalid"
                }
                ]
            },
            {
                dataField: "mobileNumber",
                caption: "मोबाइल नम्बर",
                validationRules: [{ type: "required", message: 'कृपया मोबाइल नम्बर राख्नुहोला' }]
            },
            {
                type: 'buttons',
                buttons: ['edit', 'delete',
                    {
                        icon: "fas fa-key",
                        hint: "Password Reset",
                        onClick: function (e) {
                            selectedRowData = e.row.data;
                            passwordPopUp.option("title", "Change Password#" + e.row.data.name);
                            passwordPopUp.show();
                        }
                    }, {
                        icon: "fas fa-signature",
                        hint: "Signature",
                        onClick: function (e) {
                            $window.location.href = "#!/documents/" + e.row.data.id + "/user";
                        }
                    }]
            }
        ]

    }).dxDataGrid('instance');

});