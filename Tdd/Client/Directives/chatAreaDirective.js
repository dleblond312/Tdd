app.directive('chatArea', ['chatService', function(chatService) {
    return {
        templateUrl: '/Partial/Directives/ChatArea.html',
        link: function (scope, element, attrs) {
            scope.chatMessages = [];

            scope.$on('chatMessageReceived', function (event, model) {
                scope.chatMessages.push(model);
            });

            scope.sendMessage = function () {
                if (scope.chatInput) {
                    chatService.sendMessage(scope.chatInput);
                    scope.chatInput = "";
                }
            }
        }
    }
}]);