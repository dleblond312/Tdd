app.controller('TestController', ['$scope', '$rootScope', 'gameService', 'socketService', function ($scope, $rootScope, gameService, socketService) {
    $scope.reset = function () {
        $scope.log = 'Log started\n';
    }
    $scope.reset();

    $scope.start = function () {
        $scope.log += 'Start clicked\n';
        gameService.start();
    }

    $scope.unknown = function () {
        $scope.log += 'Unknown clicked\n';
        socketService.send('unknown');
    }
}]);