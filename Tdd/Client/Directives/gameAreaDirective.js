app.directive('gameArea', ['CONSTANTS', 'gameService', 'roundService', 'buildOptionsService', function (CONSTANTS, gameService, roundService, buildOptionsService) {
    return {
        scope: {},
        templateUrl: '/Partial/Directives/GameArea.html',
        link: function (scope, element, attrs) {
            scope.gameRoom = gameService.getGame();
            scope.selectedBuild = null;

            scope.$on('propertyUpdated', function (event, model) {
                scope.gameRoom = gameService.getGame();
                scope.gameRound = roundService.getRound();
            });

            scope.$on('menuAction', function (event, model) {
                if (model.type === 'buildTower' && model.value) {
                    var tower = buildOptionsService.getTower(model.value);
                    if (scope.selectedBuild && scope.selectedBuild === tower) { // Double click to unselect
                        scope.selectedBuild = null;
                        scope.selectedStyles = {
                            display: 'none'
                        }
                    } else {
                        scope.selectedBuild = tower;
                        scope.selectedStyles = {
                            width: CONSTANTS.GAME_GRID,
                            height: CONSTANTS.GAME_GRID,
                            color: 'RED',
                            display: 'block'
                        }
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
                    
                    // Shift lets you place more towers
                    if (!event.shiftKey) {
                        scope.selectedBuild = null;
                        scope.selectedStyles = {
                            display: 'none'
                        }
                    }
                }
            }
        }
    }
}]);