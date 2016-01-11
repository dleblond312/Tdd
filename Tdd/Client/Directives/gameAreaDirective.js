app.directive('gameArea', ['CONSTANTS', 'gameService', 'roundService', 'buildOptionsService', function (CONSTANTS, gameService, roundService, buildOptionsService) {
    return {
        scope: {},
        templateUrl: '/Partial/Directives/GameArea.html',
        link: function (scope, element, attrs) {
            scope.selectedBuild = null;

            scope.$on('propertyUpdated', function (event, model) {
                scope.gameRoom = gameService.getGame();
                scope.gameRound = roundService.getRound();
            });

            scope.$on('menuAction', function (event, model) {
                if (model.type === 'buildTower' && model.value) {
                    var tower = buildOptionsService.getTower(model.value);
                    scope.selectedBuild = tower;
                    scope.selectedStyles = {
                        width: CONSTANTS.GAME_GRID,
                        height: CONSTANTS.GAME_GRID,
                        color: 'RED'
                    }
                }
            });

            scope.updateMouseMove = function (event) {
                if (scope.selectedBuild && event.target === event.currentTarget && event.offsetX && event.offsetY) {
                    scope.selectedStyles.x = (parseInt(event.offsetX / CONSTANTS.GAME_GRID) * CONSTANTS.GAME_GRID) + 'px';
                    scope.selectedStyles.y = (parseInt(event.offsetY / CONSTANTS.GAME_GRID) * CONSTANTS.GAME_GRID) + 'px';
                }
            }

            scope.performAction = function (event) {
                if (scope.selectedBuild && scope.selectedStyles) {
                    gameService.buildTower(scope.selectedBuild.id, parseInt(scope.selectedStyles.x), parseInt(scope.selectedStyles.y));
                }
            }
        }
    }
}]);