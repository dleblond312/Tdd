app.directive('gameArea', ['$window', 'CONSTANTS', 'gameService', 'roundService', 'buildOptionsService', function ($window, CONSTANTS, gameService, roundService, buildOptionsService) {
    return {
        scope: {},
        templateUrl: '/Partial/Directives/GameArea.html',
        link: function (scope, element, attrs) {

            function updateReceived() {
                scope.gameRoom = gameService.getGame();
                scope.gameRound = roundService.getRound();

                rebuildMap();
            }

            function rebuildMap() {
                //    // Draw mob paths
                //    //if (scope.gameRound && scope.gameRound.mobs) {
                //    //    context.fillStyle = '#7D26CD';
                //    //    for (var i = 0; i < scope.gameRound.mobs.length; i++) {
                //    //        if (scope.gameRound.mobs[i].path && scope.gameRound.mobs[i].path.length) {
                //    //            for (var j = 0; j < scope.gameRound.mobs[i].path.length; j++) {
                //    //                context.fillRect(scope.gameRound.mobs[i].path[j].x * scope.gameRatio, scope.gameRound.mobs[i].path[j].y * scope.gameRatio, scope.gameRatio, scope.gameRatio);
                //    //            }
                //    //        }
                //    //    }
                //    //}
            }

            updateReceived();

            element.bind('pointerup', function (event) {
                var x = parseInt(event.originalEvent.pageX / gameService.getGameRatio()) - 2;
                var y = parseInt((event.originalEvent.pageY / gameService.getGameRatio()) - 0.5);

                var selectedTower = buildOptionsService.getSelectedTower();

                if (selectedTower && x >= 0 && x < gameService.getGame().mapSize.x && y >= 0 && y < gameService.getGame().mapSize.y) {
                    gameService.buildTower(selectedTower.id, x, y);
                }
            });


            scope.$on('propertyUpdated', function (event, model) {
                updateReceived();
            });
        }
    }
}]);