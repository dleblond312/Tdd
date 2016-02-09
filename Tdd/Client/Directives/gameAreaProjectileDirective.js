
app.directive('gameAreaProjectile', ['CONSTANTS', 'gameService', 'roundService', function (CONSTANTS, gameService, roundService) {
    return {
        scope: {
            gameAreaProjectile: '=',
        },
        templateUrl: '/Partial/Directives/GameAreaProjectile.html',
        link: function (scope, element, attrs) {
            scope.$on('propertyUpdated', function (event, model) {
                scope.style = {
                    xValue: CONSTANTS.GAME_RATIO * scope.gameAreaProjectile.location.x + 'px',
                    yValue: CONSTANTS.GAME_RATIO * scope.gameAreaProjectile.location.y + 'px',
                    width: CONSTANTS.GAME_RATIO,
                    height: CONSTANTS.GAME_RATIO
                }
            });
        }
    }
}]);