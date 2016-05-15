app.directive('playerActions', ['$timeout', '$window', '$rootScope', 'gameService', 'roundService', function ($timeout, $window, $rootScope, gameService, roundService) {
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

            scope.showTowerSelect = function () {
                console.log('show tower select');
            }

            scope.longPressTower = function () {
                console.log('long press');
            }
            
        }
    }
}]);