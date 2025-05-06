const context = {
    cacheConfig: {
        ward: {
            key: 'ward',
            url: rootPath+ 'api/common/wards'
        },
        province: {
            key: 'province',
            url: rootPath +'api/common/provinces'
        },
        district: {
            key: 'district',
            url: rootPath+ 'api/common/districts'
        },
        localBody: {
            key: 'localbody',
            url: rootPath+'api/common/localbodies'
        },
        department: {
            key: 'department',
            url: rootPath + 'api/common/departments'
        },
        currentUser: {
            key: 'currentUser',
            url: rootPath + 'api/common/currentuser'
        }
       
    },
    init: function () {
        var _this = this;
        Object.keys(_this.cacheConfig).forEach(function (x) {
            helper.ajax({
                url: _this.cacheConfig[x].url,
                method: 'GET',
                callback: function (resp) {
                    sessionStorage.setItem(_this.cacheConfig[x].key, JSON.stringify(resp))

                    //if (_this.cacheConfig[x].key == 'companyInfo') {
                    //    var setting = context.getCachedData(context.cacheConfig.companyInfo);
                    //}

                    if (_this.cacheConfig[x].key == 'setting') {
                        utils.settings = context.getCachedData(context.cacheConfig.setting);
                        utils.contentEditable = utils.settings.filter(x => x.Name == 'ContentEditable')[0]?.value == 'true' ?? false;
                    }
                }
            });
        });
    },

    getCachedData: function (item) {
        let list = [];
        list = JSON.parse(sessionStorage.getItem(item.key));
        return list;
    }
};

context.init();