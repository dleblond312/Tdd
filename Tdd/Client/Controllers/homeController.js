app.controller('HomeController', ['$scope', '$rootScope', '$timeout', 'authService', 'gameService', 'mobService', function($scope, $rootScope, $timeout, authService, gameService, mobService) {
    $scope.reset = function() {
        $scope.authentication = authService.authentication;
    }
    $scope.reset();

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }

    $scope.$on('propertyUpdated', function (event, model) {
        $scope.gameRoom = gameService.getGame();
        $scope.mobs = mobService.getMobs();
    });

}]);