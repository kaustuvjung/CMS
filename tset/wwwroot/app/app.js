const app = angular.module("app", ["ngRoute", 'ja.qr']);
app.config(function ($routeProvider) {

    $routeProvider.when(rootPath + "user/users", {
        templateUrl: rootPath + 'app/views/setup/user-list.html',
        controller: 'userController',
        resolve: {
            permission: function (authService) {
                return authService.checkPermission('Administrator'); 
            }
        }
    });



    //$routeProvider.when(rootPath + "user/users", { templateUrl: rootPath + 'app/views/setup/user-list.html', controller: 'userController' });
    $routeProvider.when(rootPath + "setup/software", { templateUrl: rootPath + 'app/views/setup/software-list.html', controller: 'softwareController' });
    $routeProvider.when(rootPath + "software/softwareuserlist", { templateUrl: rootPath + 'app/views/setup/software-list.html', controller: 'softwareUserController' });
    $routeProvider.when(rootPath + "software/softwareuserlistDetail", { templateUrl: rootPath + 'app/views/setup/softwareuserlist.html', controller: 'softwareUserDetailsController' });
  
});

app.run(function ($rootScope, $location) {
    $rootScope.$on('$routeChangeError', function (event, current, previous, rejection) {
        $location.path(rootPath);
    });
});