const app = angular.module("app", ["ngRoute", 'ja.qr']);
app.config(function ($routeProvider) {

    $routeProvider.when(rootPath + "user/users", { templateUrl: rootPath + 'app/views/setup/user-list.html', controller: 'userController' });
  
    //$routeProvider.when(rootpath + "user/users", { templateUrl: rootpath + 'app/views/setup/user-list.html', controller: "userController" });
    //$routeProvider.when(rootPath + "user/passwordchange", { templateUrl: rootPath + 'app/views/setup/passwordchange.html', controller: 'passwordChangeController' });
});
