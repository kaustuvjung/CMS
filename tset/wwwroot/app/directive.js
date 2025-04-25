app.directive('printbutton', [function () {
    return {
        template: '<div id="btn-print"></div>',
        link: function () {
            $("#btn-print").dcButton({
                hint: "Print",
                icon: "print",
                text: "print",
                onClick: function () {
                    $(".page .container").printThis({
                        imortCSS: false,
                        loadCSS: [rootpath + "css/print.css"],
                        beforePrint: function () {
                            $('qr').css('display', 'block');
                        },
                        afterPrint: function () {
                            $('qr').css('display', 'none');
                        }
                    });
                }

            })
        }
    }
}])

app.directive('rptprintbutton', [function () {
    return {
        template: '<div id="btn-print"></div>',
        link: function () {
        $("#btn-print").dxButton({
            hint: "Print",
            icon: "print",
            text: "print",
            onClick: function () {
                $(".report").printThis({
                    importCSS: false,
                    loadCSS: [rootPath + "css/print.css", rootPath + "css/printLandscape.css"],
                });
            }
        })
        }
    }
}])

app.directive('modalprintbutton', [function () {
    return {
        template: '<div id="brn-print"></div>',
        link: function () {
            $("#btn-print").dxButton({
                hint: "Print",
                icon: "print",
                text: "print",
                onClick: function () {
                    $(".modal-body").printThis({
                        importCSS: false,
                        loadCSS: [rootpath + "css/print.css"]
                    });
                }
            })
        }
    }
}])

app.directive('header', [function () {
    return {
        templateUrl: rootpath + 'app/views/header.html'
    }
}])
app.directive('fotter', [function () {
    return {
        templateUrl: rootpath+'app/views/fotter.html'
    }
}])