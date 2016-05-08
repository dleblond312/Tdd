app.directive('gameArea', ['CONSTANTS', 'gameService', 'roundService', 'buildOptionsService', function (CONSTANTS, gameService, roundService, buildOptionsService) {
    return {
        scope: {},
        templateUrl: '/Partial/Directives/GameArea.html',
        link: function (scope, element, attrs) {
            scope.gameRatio = CONSTANTS.GAME_RATIO;

            function updateReceived() {
                scope.gameRoom = gameService.getGame();
                scope.gameRound = roundService.getRound();

                rebuildMap();
            }

            function rebuildMap() {
                var canvas = element.find('#map-background')[0];
                $(canvas).attr('width', scope.gameRoom.mapSize * scope.gameRatio);
                $(canvas).attr('height', scope.gameRoom.mapSize * scope.gameRatio);

                //if (canvas) {
                    
                //    var context = canvas.getContext('2d');
                //    context.clearRect(0, 0, canvas.width, canvas.height);
                //    context.fillStyle = '#00FF00';
                //    context.beginPath();
                    
                //    // Draw the game board
                //    context.moveTo(scope.gameRoom.map[0].x * scope.gameRatio, scope.gameRoom.map[0].y * scope.gameRatio);
                //    for (var i = 1; i < scope.gameRoom.map.length; i++) {
                //        context.lineTo(scope.gameRoom.map[i].x * scope.gameRatio, scope.gameRoom.map[i].y * scope.gameRatio);
                //    }
                //    context.closePath();
                //    context.fill();

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

                //    // context.fillText("(" + scope.event.offsetX + "," + scope.event.offsetY + ")", 0, canvas.height - 20);
                //}
            }

            updateReceived();

            scope.updateMouseMove = function (event) {
                if (scope.selectedBuild && event.target === event.currentTarget && event.offsetX && event.offsetY) {
                    scope.selectedStyles.x = (parseInt(event.offsetX / scope.gameRatio) * scope.gameRatio) + 'px';
                    scope.selectedStyles.y = (parseInt(event.offsetY / scope.gameRatio) * scope.gameRatio) + 'px';
                }

                updateReceived();
            }

            scope.performAction = function (event) {
                if (scope.selectedBuild && scope.selectedStyles) {
                    gameService.buildTower(scope.selectedBuild.id, parseInt(scope.selectedStyles.x) / scope.gameRatio, parseInt(scope.selectedStyles.y) / scope.gameRatio);
                    
                    // !Shift lets you place more towers
                    if (event.shiftKey) {
                        scope.selectedBuild = null;
                        scope.selectedStyles = {
                            display: 'none'
                        }
                    }
                }
            }

            scope.$on('propertyUpdated', function (event, model) {
                updateReceived();
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
                            width: scope.gameRatio,
                            height: scope.gameRatio,
                            color: 'RED',
                            display: 'block'
                        }
                    }
                }
            });

        }
    }
}]);