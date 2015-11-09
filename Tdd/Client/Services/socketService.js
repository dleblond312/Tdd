app.service('socketService', ['$rootScope', function ($rootScope) {
    if (!($ && $.connection && $.connection.hub)) {
        window.alert("Can't instantiate a socket");
    }
    $.connection.hub.logging = true;

    var game = $.connection.gameHub;
    //game.client.broadcastMessage = function (name, message) {
    //}

    $.connection.hub.start().done(function () {
        console.log('Connection opened');
    });

    this.send = function (name, message) {
        console.log('Sending ', name, message);
        game.server.send(name, message);
    }


    game.client.receiveCommand = function (name, message) {
        console.log('Received ', name, message);
        $rootScope.$broadcast(name, message);
    }
}]);