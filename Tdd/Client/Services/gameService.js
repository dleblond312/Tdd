app.service('gameService', ['$q', '$rootScope', 'socketService', function ($q, $rootScope, socketService) {
    var gameRoom = {};

    this.createGame = function () {
        socketService.send('createGame');
    }

    this.startRound = function () {
        socketService.send('startRound', gameRoom.id);
    }

    this.getGame = function () {
        return gameRoom;
    }

    $rootScope.$on("propertyUpdated", function (event, model) {
        if (model.type.indexOf("GameRoom") == 0) {
            gameRoom = model.value;
        }
    })
}]);