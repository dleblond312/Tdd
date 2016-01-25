
app.directive('gameAreaMob', ['CONSTANTS', 'gameService', 'roundService', function (CONSTANTS, gameService, roundService) {
    return {
        scope: {
            gameAreaMob: '=',
        },
        templateUrl: '/Partial/Directives/GameAreaMob.html',
        link: function (scope, element, attrs) {
            scope.$on('propertyUpdated', function (event, model) {
                scope.style = {
                    xValue: CONSTANTS.GAME_RATIO * scope.gameAreaMob.currentLocation.x + 'px',
                    yValue: CONSTANTS.GAME_RATIO * scope.gameAreaMob.currentLocation.y + 'px',
                    width: CONSTANTS.GAME_RATIO,
                    height: CONSTANTS.GAME_RATIO
                }
            });
        }
    }
}]);