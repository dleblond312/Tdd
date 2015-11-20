app.controller('TestController', ['$scope', '$rootScope', '$timeout', 'gameService', 'socketService', function ($scope, $rootScope, $timeout, gameService, socketService) {
    $scope.reset = function () {
        $scope.log = 'Log started\n';
    }
    $scope.reset();

    $scope.createGame = function () {
        $scope.log += 'Create clicked\n';
        gameService.createGame();
    }

    $scope.joinGame = function () {
        $scope.log += 'Join Clicked\n';
        gameService.joinGame($scope.joinGameId);
    }

    $scope.startRound = function () {
        $scope.log += 'Round started clicked\n';
        gameService.startRound();
    }

    $scope.unknown = function () {
        $scope.log += 'Unknown clicked\n';
        socketService.send('unknown');
    }

    $scope.$on("gameUpdated", function (event, model) {
        $scope.log += 'Game Room updated\n';
    });
}]);