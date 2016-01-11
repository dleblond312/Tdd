app.service('chatService', ['socketService', 'authService', 'gameService', function (socketService, authService, gameService) {
    
    this.sendMessage = function (text) {
        var msg = {};
        msg.author = authService.getUser();
        msg.text = text;
        msg.senderType = 'Player';

        socketService.chatMessageSend(gameService.getGame().id, JSON.stringify(msg));
    }
    
}]);