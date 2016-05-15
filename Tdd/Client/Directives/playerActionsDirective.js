app.directive('playerActions', ['$timeout', '$window', '$rootScope', '$uibModal', 'gameService', 'roundService', function ($timeout, $window, $rootScope, $uibModal, gameService, roundService) {
    return {
        templateUrl: 'Partial/Directives/PlayerActions.html',
        scope: {
            playerActions: '=',
        },
        link: function (scope, element, attrs) {
            scope.gameRoom = gameService.getGame();

            rescalePlayerArea = function () {
                var actions = $('.player-actions');
                actions.css('padding', gameService.getGameRatio() / 2);
                actions.width(gameService.getGameRatio());
                actions.find('.player-action').height(gameService.getGameRatio()).css('margin-bottom', gameService.getGameRatio()/2);
            }


            $timeout(function () {
                rescalePlayerArea();
            }, 100);

            angular.element($window).bind('resize', function () {
                rescalePlayerArea();
            });

            scope.showMobSelect = function () {
            }

            scope.showTowerSelect = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'Partial/Modal/TowerSelect.html',
                    controller: 'TowerSelectModalController',
                    animation: false,
                });

                modalInstance.result.then(function (selectedTower) {
                    console.log('Tower selected!', selectedTower);
                    $scope.selectedTower = selectedTower;
                });
                
            }
            
        }
    }
}]);