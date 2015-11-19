app.service('socketService', ['$rootScope', function ($rootScope) {
    if (!($ && $.connection && $.connection.hub)) {
        window.alert("Can't instantiate a socket");
    }
    $.connection.hub.logging = true;

    var game = $.connection.gameHub;

    $.connection.hub.start().done(function () {
        console.log('Connection opened');
    });

    this.send = function (name, message) {
        console.log('Sending ', name, message);
        game.server.send(name, message);
    }


    game.client.propertyUpdated = function (id, model) {
        $rootScope.$apply(function () {
            console.log('Update Property ', id, model);
            $rootScope.$broadcast('propertyUpdated', {
                type: id.split('-')[0],
                id: id.split('-')[1],
                value: model
            });
        })
    }

    game.client.warn = function (id, model) {
        console.warn('Received warning', id, model);
    }
}]);