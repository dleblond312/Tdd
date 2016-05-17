
app.directive('gameAreaProjectile', ['gameService', 'roundService', function (gameService, roundService) {
    return {
        scope: {
            gameAreaProjectile: '=',
        },
        templateUrl: '/Partial/Directives/GameAreaProjectile.html',
        link: function (scope, element, attrs) {
            scope.calculateSize = function () {
                var gameRatio = gameService.getGameRatio();
                scope.style = {
                    x: (gameRatio * 2) + scope.gameAreaProjectile.location.x * gameRatio + "px",
                    y: (gameRatio * 0.5) + scope.gameAreaProjectile.location.y * gameRatio + "px",
                    width: gameRatio/4 + "px",
                    height: gameRatio/4 + "px"
                }
            }
            scope.calculateSize();

            scope.$on('propertyUpdated', function (event, model) {
                scope.calculateSize();
            });

            angular.element($window).bind('resize', function () {
                $timeout(function () {
                    scope.calculateSize();
                });
            });
        }
    }
}]);