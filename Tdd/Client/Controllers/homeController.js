app.controller('HomeController', ['$scope', '$rootScope', '$location', '$routeParams', '$timeout', '$window', 'authService', 'gameService', 'roundService', function ($scope, $rootScope, $location, $routeParams, $timeout, $window, authService, gameService, roundService) {
    $scope.reset = function() {
        $scope.authentication = authService.authentication;
    }
    $scope.reset();

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }

    if ($routeParams.game) {
        gameService.joinGame($routeParams.game);
    }

    $scope.createGame = function () {
        gameService.createGame();
    }

    $scope.joinGame = function () {
        var result = $window.prompt('What is the game id?', "123");
        if (result) {
            gameService.joinGame(result);
        }
    }

    $scope.$on('propertyUpdated', function (event, model) {
        $scope.gameRoom = gameService.getGame();
        if ($scope.gameRoom) {
            $location.search('game', $scope.gameRoom.id);
        }

        $scope.round = roundService.getRound();
    });

}]);