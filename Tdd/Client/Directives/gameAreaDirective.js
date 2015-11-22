app.directive('gameArea', ['gameService', 'roundService', function (gameService, roundService) {
    return {
        scope: {},
        templateUrl: '/Partial/Directives/GameArea.html',
        link: function (scope, element, attrs) {
            scope.$on('propertyUpdated', function (event, model) {
                scope.gameRoom = gameService.getGame();
                scope.gameRound = roundService.getRound();
            });
        }
    }
}]);