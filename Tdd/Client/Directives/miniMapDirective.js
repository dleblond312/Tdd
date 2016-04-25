app.directive('miniMap', ['CONSTANTS', 'gameService', 'roundService', function (CONSTANTS, gameService, roundService) {
    return {
        templateUrl: 'Partial/Directives/MiniMap.html',
        link: function (scope, element, attrs) {
            function updateMiniMap() {
                scope.gameRoom = gameService.getGame();
                scope.gameRound = roundService.getRound();

                var canvas = element.find('#mini-map-canvas')[0];
                if (canvas) {
                    var context = canvas.getContext('2d');
                    context.clearRect(0, 0, canvas.width, canvas.height);

                    context.fillStyle = '#00FF00';
                    context.beginPath();
                    context.moveTo(scope.gameRoom.map[0].x * CONSTANTS.MINI_MAP_RATIO, scope.gameRoom.map[0].y * CONSTANTS.MINI_MAP_RATIO);
                    for (var i = 1; i < scope.gameRoom.map.length; i++) {
                        context.lineTo(scope.gameRoom.map[i].x * CONSTANTS.MINI_MAP_RATIO, scope.gameRoom.map[i].y * CONSTANTS.MINI_MAP_RATIO);
                    }
                    context.closePath();
                    context.fill();

                    context.fillStyle = '#FF0000';
                    if (scope.gameRound.mobs) {
                        for (var i = 0; i < scope.gameRound.mobs.length; i++) {
                            context.fillRect(scope.gameRound.mobs[i].currentLocation.x * CONSTANTS.MINI_MAP_RATIO, scope.gameRound.mobs[i].currentLocation.y * CONSTANTS.MINI_MAP_RATIO, CONSTANTS.MINI_MAP_RATIO, CONSTANTS.MINI_MAP_RATIO);
                        }
                    }


                }
            }
            updateMiniMap();

            scope.$on('propertyUpdated', function (event, model) {
                updateMiniMap();
            });

        }
    }
}]);