app.directive('gameTower', ['gameService', function (gameService) {
    return {
        scope: {
            gameTower: '='
        },
        templateUrl: '/Partial/Directives/GameTower.html',
        link: function (scope, element, attrs) {
            var gameRatio = gameService.getGameRatio();

            scope.style = {
                x: (gameRatio * 2) + scope.gameTower.location.x * gameRatio + "px",
                y: (gameRatio * 0.5) + scope.gameTower.location.y * gameRatio + "px",
                width: gameRatio + "px",
                height: gameRatio + "px"
            }
        }
    }
}])