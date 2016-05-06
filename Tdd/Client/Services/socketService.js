app.service('socketService', ['$rootScope', function ($rootScope) {
    if (!($ && $.connection && $.connection.hub)) {
        window.alert("Can't instantiate a socket");
    }

    $.connection.hub.logging = false;

    var game = $.connection.gameHub;

    game.client.propertyUpdated = function (id, model) {
        $rootScope.$apply(function () {
            $rootScope.$broadcast('propertyUpdated', {
                type: id.split('-')[0],
                id: id.split('-')[1],
                value: model
            });
        })
    }

    game.client.chatMessageReceived = function (model) {
        $rootScope.$apply(function () {
            $rootScope.$broadcast('chatMessageReceived', JSON.parse(model));
        });
    }

    game.client.warn = function (id, model) {
        console.warn('Received warning', id, model);
    }

    $.connection.hub.start().done(function () {
        console.log('Connection opened');
    });

    this.send = function (name, message) {
        $.connection.hub.start().done(function () {
            game.server.send(name, message);
        });
    }

    this.chatMessageSend = function (id, message) {
        game.server.chatMessageSend(id, message);
    }

    this.buildTower = function (gameRoomId, towerId, x, y) {
        game.server.buildTower(gameRoomId, towerId, x, y);
    }



}]);