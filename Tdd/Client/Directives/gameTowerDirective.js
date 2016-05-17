app.directive('gameTower', ['$window', '$timeout', 'gameService', function ($window, $timeout, gameService) {
    return {
        scope: {
            gameTower: '='
        },
        templateUrl: '/Partial/Directives/GameTower.html',
        link: function (scope, element, attrs) {
            scope.calculateSize = function () {
                var gameRatio = gameService.getGameRatio();
                scope.style = {
                    x: (gameRatio * 2) + scope.gameTower.location.x * gameRatio + "px",
                    y: (gameRatio * 0.5) + scope.gameTower.location.y * gameRatio + "px",
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
}])