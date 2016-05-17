app.directive('playerActions', ['$timeout', '$window', '$rootScope', '$uibModal', 'gameService', 'buildOptionsService', 'roundService', function ($timeout, $window, $rootScope, $uibModal, gameService, buildOptionsService, roundService) {
    return {
        templateUrl: 'Partial/Directives/PlayerActions.html',
        scope: {
            playerActions: '@',
        },
        link: function (scope, element, attrs) {

            scope.gameRoom = gameService.getGame();
            scope.player = scope.gameRoom.players[scope.playerActions];
            scope.current = scope.player.id == $.connection.hub.id;

            rescalePlayerArea = function () {
                var actions = $('.player-actions');
                actions.css('padding', gameService.getGameRatio() / 2);
                actions.width(gameService.getGameRatio());
                actions.find('.player-action').height(gameService.getGameRatio()).css('margin-bottom', gameService.getGameRatio()/2);
            }


            $timeout(function () {
                rescalePlayerArea();
            }, 10);

            angular.element($window).bind('resize', function () {
                rescalePlayerArea();
            });

            scope.showMobSelect = function () {
                if (scope.current) {
                    // TODO
                }
            }

            scope.showTowerSelect = function () {
                if (scope.current) {
                    var modalInstance = $uibModal.open({
                        templateUrl: 'Partial/Modal/TowerSelect.html',
                        controller: 'TowerSelectModalController',
                        animation: false,
                    });

                    modalInstance.result.then(function (selectedTower) {
                        scope.selectedTower = buildOptionsService.getSelectedTower();
                    });
                }
            }
            
        }
    }
}]);