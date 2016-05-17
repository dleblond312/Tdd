
app.directive('gameAreaMob', ['gameService', 'roundService', function (gameService, roundService) {
    return {
        scope: {
            gameAreaMob: '=',
        },
        templateUrl: '/Partial/Directives/GameAreaMob.html',
        link: function (scope, element, attrs) {
            scope.calculateSize = function () {
                var gameRatio = gameService.getGameRatio();
                scope.style = {
                    x: (gameRatio * 2) + scope.gameAreaMob.location.x * gameRatio + "px",
                    y: (gameRatio * 0.5) + scope.gameAreaMob.location.y * gameRatio + "px",
                    width: gameRatio + "px",
                    height: gameRatio + "px"
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