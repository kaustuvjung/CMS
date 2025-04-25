const helper = {
    setTitle: function (title) {
        $(".breadcrumb [data-title]").text(title);
        $(".sidebar-overlay").click();
    },
    showMessage: function (msg) {
        alertify.Sucess(msg);
    },
    showErrorMessage: function (msg) {
        alertify.error(msg);
    },
    popupAlert: function (msg) {
        alertify.alert("message", msg);
    },
    confirm: function (msg, ok, cancel) {
        alertify.confirm('Confirmation', msg, function () {
            if (typeof ok === 'function')
                ok();
        }, function () {
            if (typeof cancel === 'function')
                cancel();
        });
    },

    createStore: function (option) {
        var cacheData = {};
        var updatingObj = {};
        var store = DevExpress.data.AspNet.createStore({
            key: option.key,
            loadUrl: option.loadUrl,
            insertUrl: option.saveUrl,
            updateUrl: option.saveUrl,
            updateMethod: 'POST',
            deleteUrl: option.deleteUrl,
            onBeforeSend: (_method, ajaxOptions) => {
                if (_method !== "load") {
                    if (_method === "delete") {
                        ajaxOptions.url = ajaxOptions.url + `?${option.key}=${this.currentKey}`;
                    }
                    else if (_method === "update") {
                        ajaxOptions.data = JSON.stringify(updatingObj);
                        ajaxOptions.contentType = "application/json";
                    }
                    else {
                        ajaxOptions.data = ajaxOptions.data.values;
                        ajaxOptions.contentType = "application/json";
                    }
                }
                ajaxOptions.xhrFields = { withCredentials: true };
            },
            OnAjaxError: (e) => {
                if (e.xhr.status == 0) {
                    window.location.reload();
                }
                this.showErrorMessage("Ooops! SomethingWent Wrong");
            },
            onInserted: () => {
                this.showMessage("Record save Sucessfully");
            },
            onUpdating: (key, values) => {
                values[option.key] = key;
                var item = $.grep(cacheData, function (i) {
                    return i[option.key] === key;
                }[0]);
                $.extend(item, values);
                updatingObj = item;
            },
            onUpdated: () => {
                this.showMessage("Record Updated Sucessfully");
            },
            onRemoving: (key) => {
                this.currentKey = key;
            },
            onRemoved: (key) => {
                this.showMessage("Record Delete Sucessfuly");
            },
            onLoaded: (results) => {
                cacheData = results;
            }
        });
        return store;
    },


    ajax: function (options) {
        const { url, async = true, method, data, callback, error, beforeSend, complete } = options;

        $.ajax({
            url: url,
            xhrFields: {
                withCredentials: true
            },
            aysnc: async,
            data: JSON.stringify(data),
            method: method,
            contentType: 'application/json',
            beforeSend: function () {
                if (typeof beforeSend === 'function')
                    beforeSend();
            },
            complete: function () {
                if (typeof complete === 'function')
                    complete();
            },
            success: (data){
                if (typeof callback === 'function')
                    callback(data);
            },
            error: function (xhr, err) {
                if (xhr.status == 0) {
                    window.location.reload();
                }
                if (typeof error === 'function')
                    error(data);
                else
                    alertify.error("Ooops! Something Went Wrong");
            }
        });
    },

    minDefinedvalueValidator: {
        type: "custom",
        message: " This Entered value is below Minimum",
        validationCallback: function (args) {
            let fieldType = args.data.fieldType;
            if (fieldType === 'Numeric') {
                let value = args.data.value;
                let definedValue = args.data.definedValue;
                if ($.isNumeric(definedValue) && Number(value) < Number(definedValue)) {
                    return false;
                }
            }
            return true;
        }
    }



}