app.service('gameService', ['$window', '$q', '$rootScope', '$timeout', 'socketService', function ($window, $q, $rootScope, $timeout, socketService) {
    var gameRoom = {};
    var gameRatio = 40;
    this.createGame = function () {
        socketService.send('createGame');
    }

    this.joinGame = function (roomId) {
        gameRoom.id = roomId;
        socketService.send('joinGame', gameRoom.id);
    }

    this.startRound = function () {
        socketService.send('startRound', gameRoom.id);
    }

    this.getGame = function () {
        return gameRoom;
    }

    this.getGameRatio = function () {
        return gameRatio;
    }

    this.buildTower = function (towerId, x, y) {
        socketService.buildTower(gameRoom.id, towerId + "", x , y);
    }

    rescaleGameArea = function () {
        if (gameRoom && gameRoom.mapSize) {
            gameRatio = Math.min(window.innerWidth || document.body.clientWidth, window.innerHeight || document.body.clientHeight) / (gameRoom.mapSize.y + 1);
            var canvas = $('#map-background')[0];
            $(canvas).attr('width', (gameRoom.mapSize.x * gameRatio) + (gameRatio * 4));
            $(canvas).attr('height', gameRoom.mapSize.y * gameRatio);
            $(canvas).css('padding-top', gameRatio / 2);
            $(canvas).css('padding-bottom', gameRatio / 2);
            $('html').height((gameRoom.mapSize.y + 1) * gameRatio);
            $('html').width((gameRoom.mapSize.x * gameRatio) + (gameRatio * 4));
        }
    }

    angular.element($window).bind('resize', function () {
        rescaleGameArea();
    });

    $timeout(function () {
        rescaleGameArea();
    });

    $rootScope.$on("propertyUpdated", function (event, model) {
        if (model.type.indexOf("GameRoom") == 0) {
            gameRoom = model.value;
            rescaleGameArea();
        }
    })
}]);