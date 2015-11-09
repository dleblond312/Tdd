app.service('gameService', ['$q', 'socketService', function ($q, socketService) {
    this.start = function () {
        socketService.send('start');
    }
}]);