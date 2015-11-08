var app = angular.module('tdd', ['ngRoute', 'ngCookies']);

app.config(['$routeProvider', '$locationProvider',
  function($routeProvider, $locationProvider) {
    $locationProvider.html5Mode(true);

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/Partial/Account/Login.html"
    }).when("/register", {
        controller: "registerController",
        templateUrl: "/Partial/Account/Register.html"
    }).when('/', {
        templateUrl: '/Partial/Home.html',
        controller: 'HomeController'
    });

}]);

app.run(['authService', function (authService) {
    authService.fillAuthData();
}])

app.run(function () {
    if (!($ && $.connection && $.connection.hub)) {
        window.alert("Can't instantiate a socket");
        return;
    }
    $.connection.hub.logging = true;

    var game = $.connection.gameHub;
    game.client.broadcastMessage = function (name, message) {
        console.log('message received:', name, message);
    }

    $.connection.hub.start().done(function () {
        console.log('Connection started');
        game.server.send('test-user-' + Math.random() * 100, "test");
    });
});

app.config(['$httpProvider', function($httpProvider) {
    $httpProvider.interceptors.push('httpRequestInterceptor');
    $httpProvider.interceptors.push('authInterceptor');
}]);