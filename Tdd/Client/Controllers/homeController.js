app.controller('HomeController', ['$scope', '$rootScope', 'authService', function($scope, $rootScope, authService) {
    $scope.reset = function() {
        $scope.authentication = authService.authentication;
    }
    $scope.reset();

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }


}]);