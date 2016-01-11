app.service('gameService', ['$q', '$rootScope', 'socketService', function ($q, $rootScope, socketService) {
    var gameRoom = {};

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

    this.buildTower = function (towerId, x, y) {
        socketService.buildTower(gameRoom.id, towerId + "", x , y);
    }

    $rootScope.$on("propertyUpdated", function (event, model) {
        if (model.type.indexOf("GameRoom") == 0) {
            gameRoom = model.value;
        }
    })
}]);