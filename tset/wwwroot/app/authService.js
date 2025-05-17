app.factory('authService', function ($q, $location) {
    return {
        checkPermission: function (requiredPermissionName) {
            const deferred = $q.defer();

            const permissions = JSON.parse(sessionStorage.getItem('permissions'));
            const currentUser = JSON.parse(sessionStorage.getItem('currentUser'));

            if (!permissions || !currentUser) {
                $location.path(rootPath);
                deferred.reject("Session data missing");
            } else {
                const userPermission = permissions.find(p => p.id === currentUser.permissionId);
                const hasPermission = currentUser.isAdmin || (userPermission && userPermission.name === requiredPermissionName);

                /*const hasPermission = currentUser.isAdmin || permissions.some(p => p.name === requiredPermissionName);*/
            

                if (hasPermission) {
                    deferred.resolve();
                } else {
                    $location.path(rootPath);
                    deferred.reject("Access denied");
                }
            }

            return deferred.promise;
        }
    };
});
