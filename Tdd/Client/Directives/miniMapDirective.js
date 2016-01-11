app.directive('miniMap', ['gameService', 'roundService', function (gameService, roundService) {
    return {
        templateUrl: 'Partial/Directives/MiniMap.html',
        link: function (scope, element, attrs) {
            scope.$on('propertyUpdated', function (event, model) {
                scope.gameRoom = gameService.getGame();
                scope.gameRound = roundService.getRound();

                var canvas = element.find('#mini-map-canvas')[0];
                if (canvas) {
                    var context = canvas.getContext('2d');
                    context.clearRect(0, 0, canvas.width, canvas.height);
                    context.fillStyle = '#FF0000';
                    if (scope.gameRound.mobs) {
                        for (var i = 0; i < scope.gameRound.mobs.length; i++) {
                            context.fillRect(scope.gameRound.mobs[i].currentLocation.x, scope.gameRound.mobs[i].currentLocation.y, 1, 1);
                        }
                    }
                }
            });

        }
    }
}]);