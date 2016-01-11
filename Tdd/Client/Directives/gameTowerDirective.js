app.directive('gameTower', ['CONSTANTS', function (CONSTANTS) {
    return {
        scope: {
            gameTower: '='
        },
        templateUrl: '/Partial/Directives/GameTower.html',
        link: function (scope, element, attrs) {
            scope.style = {
                x: scope.gameTower.location.x + "px",
                y: scope.gameTower.location.y + "px",
                width: CONSTANTS.GAME_GRID + "px",
                height: CONSTANTS.GAME_GRID + "px",
                color: 'Green'
            }
        }
    }
}])