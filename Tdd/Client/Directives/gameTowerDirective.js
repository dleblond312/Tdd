app.directive('gameTower', ['CONSTANTS', function (CONSTANTS) {
    return {
        scope: {
            gameTower: '='
        },
        templateUrl: '/Partial/Directives/GameTower.html',
        link: function (scope, element, attrs) {
            scope.style = {
                x: scope.gameTower.location.x * CONSTANTS.GAME_RATIO + "px",
                y: scope.gameTower.location.y * CONSTANTS.GAME_RATIO + "px",
                width: CONSTANTS.GAME_RATIO + "px",
                height: CONSTANTS.GAME_RATIO + "px"
            }
        }
    }
}])